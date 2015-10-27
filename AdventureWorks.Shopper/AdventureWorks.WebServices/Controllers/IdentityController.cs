// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Security;
using AdventureWorks.WebServices.Models;
using System.Runtime.Caching;
using System.Net;

namespace AdventureWorks.WebServices.Controllers
{
    public class IdentityController : ApiController
    {
        // Credentials store
        private static readonly Dictionary<string, string> Identities = new Dictionary<string, string>
            {
                {"JohnDoe", "pwd"},
                {"user", "pwd"}
            };

        // Recently sent password challenges, use cache for thread safety and time-based expiration
        private static MemoryCache ChallengeCache = new MemoryCache("Challenges");

        // GET /api/Identity/GetPasswordChallenge?requestId={requestId}
        public string GetPasswordChallenge(string requestId)
        {
            if (requestId == null)
                return null;
            using (var generator = new RNGCryptoServiceProvider())
            {
                var challengeBytes = new byte[16];
                generator.GetBytes(challengeBytes);
                if (ChallengeCache.Contains(requestId))
                {
                    ChallengeCache[requestId] = challengeBytes;
                }
                else
                {
                    CacheItemPolicy policy = new CacheItemPolicy
                        {
                            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
                        };
                    ChallengeCache.Add(requestId, challengeBytes, policy);
                }
                return EncodeToHexString(challengeBytes);
            }
        }

        // GET /api/Identity/id?requestId={requestId}&passwordHash={passwordHash}
        public UserInfo GetIsValid(string id, string requestId, string passwordHash)
        {
            byte[] challenge = null;
            if (requestId != null && ChallengeCache.Contains(requestId))
            {
                // Retrieve the saved challenge bytes
                challenge = (byte[])ChallengeCache[requestId];
                // Delete saved challenge (each challenge is used just one time).
                ChallengeCache.Remove(requestId);
            }

            lock (Identities)
            {
                // Check that credentials are valid.
                if (challenge != null && id != null && passwordHash != null && Identities.ContainsKey(id))
                {
                    // Compute hash for the previously issued challenge string using the password from the server's credentials store as the key.
                    var serverPassword = Encoding.UTF8.GetBytes(Identities[id]);
                    using (var provider = new HMACSHA512(serverPassword))
                    {
                        var serverHashBytes = provider.ComputeHash(challenge);
                        // Authentication succeeds only if client and server have computed the same hash for the challenge string.
                        var clientHashBytes = DecodeFromHexString(passwordHash);
                        if (!serverHashBytes.SequenceEqual(clientHashBytes))
                            throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    }

                    if (HttpContext.Current != null)
                        FormsAuthentication.SetAuthCookie(id, false);
                    return new UserInfo { UserName = id };
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
        }
        // GET /api/Identity/GetIsValidSession
        [Authorize]
        public bool GetIsValidSession()
        {
            return true;
        }

        // Output matches CryptographicBuffer.DecodeFromHexString in Windows Runtime.
        private static byte[] DecodeFromHexString(string hex)
        {
            var raw = new byte[hex.Length / 2];
            for (var i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        // Output matches CryptographicBuffer.EncodeToHexString in Windows Runtime.
        private static string EncodeToHexString(byte[] hexBytes)
        {
            var sb = new StringBuilder(hexBytes.Length * 2);
            foreach (var b in hexBytes)
            {
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }
    }
}
