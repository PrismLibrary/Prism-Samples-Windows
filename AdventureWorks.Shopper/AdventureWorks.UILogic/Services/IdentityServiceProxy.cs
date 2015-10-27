// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Models;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Security;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Web.Http;

namespace AdventureWorks.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        private readonly string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Identity/", Constants.ServerAddress);

        public async Task<LogOnResult> LogOnAsync(string userId, string password)
        {
            using (var client = new HttpClient())
            {
                // Ask the server for a password challenge string
                var requestId = CryptographicBuffer.EncodeToHexString(CryptographicBuffer.GenerateRandom(4));
                var challengeResponse = await client.GetAsync(new Uri(_clientBaseUrl + "GetPasswordChallenge?requestId=" + requestId));
                challengeResponse.EnsureSuccessStatusCode();
                var challengeEncoded = await challengeResponse.Content.ReadAsStringAsync();
                challengeEncoded = challengeEncoded.Replace(@"""", string.Empty);
                var challengeBuffer = CryptographicBuffer.DecodeFromHexString(challengeEncoded);

                // Use HMAC_SHA512 hash to encode the challenge string using the password being authenticated as the key.
                var provider = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha512);
                var passwordBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
                var hmacKey = provider.CreateKey(passwordBuffer);
                var buffHmac = CryptographicEngine.Sign(hmacKey, challengeBuffer);
                var hmacString = CryptographicBuffer.EncodeToHexString(buffHmac);

                // Send the encoded challenge to the server for authentication (to avoid sending the password itself)
                var response = await client.GetAsync(new Uri(_clientBaseUrl + userId + "?requestID=" + requestId + "&passwordHash=" + hmacString));

                // Raise exception if sign in failed
                response.EnsureSuccessStatusCode();

                // On success, return sign in results from the server response packet
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserInfo>(responseContent);
                var serverUri = new Uri(Constants.ServerAddress);
                return new LogOnResult { UserInfo = result };
            }
        }

        public async Task<bool> VerifyActiveSessionAsync(string userId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(_clientBaseUrl + "GetIsValidSession"));
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SecurityException();
                }

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(responseContent);
            }
        }
    }
}
