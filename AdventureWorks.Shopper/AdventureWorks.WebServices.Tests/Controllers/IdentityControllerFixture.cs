

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using AdventureWorks.WebServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using System.Net;

namespace AdventureWorks.WebServices.Tests.Controllers
{
    [TestClass]
    public class IdentityControllerFixture
    {
        #region Simulation of Client Side Processing

        private static byte[] DecodeFromHexString(string hex)
        {
            var raw = new byte[hex.Length / 2];
            for (var i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private static string EncodeToHexString(byte[] hexBytes)
        {
            var sb = new StringBuilder(hexBytes.Length * 2);
            foreach (var b in hexBytes)
            {
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        private static string CreatePasswordHash(string password, string challengeString)
        {
            var passwordBuffer = Encoding.UTF8.GetBytes(password);

            // same as MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha512).CreateKey(passwordBuffer) in Windows Runtime.
            var provider = new HMACSHA512(passwordBuffer);

            // Same as CryptographicBuffer.DecodeFromHexString in Windows Runtime.
            var challengeBuffer = DecodeFromHexString(challengeString);

            // same as CryptographicEngine.Sign(provider, challengeBuffer) in Windows Runtime.
            var hmacBytes = provider.ComputeHash(challengeBuffer);

            // Same as CryptographicBuffer.EncodeToHexString in Windows Runtime.
            return EncodeToHexString(hmacBytes);
        }
        #endregion

        // Scenario 1: valid user, valid password
        [TestMethod]
        public void ValidateUserNameValidPassword()
        {
            var controller = new IdentityController();

            // 1- Get a random password challenge string from the web service.
            const string requestId = "ec609a4f";
            var challengeString = controller.GetPasswordChallenge(requestId);
            Assert.IsFalse(string.IsNullOrEmpty(challengeString));

            // 2 - Hash the challenge string with the correct password and ask the web service to validate the hash.
            var result = controller.GetIsValid("JohnDoe", requestId, CreatePasswordHash("pwd", challengeString));

            // 3- Verify that credentials were validated.
            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "JohnDoe");
        }

        // Scenario 2: valid user, invalid password
        [TestMethod]
        public void ValidateUserNameInvalidPassword()
        {
            var sawException = false;
            var controller = new IdentityController();

            // 1- Get a random password challenge string from the web service.
            const string requestId = "ec609a4f";
            var challengeString = controller.GetPasswordChallenge(requestId);
            Assert.IsFalse(string.IsNullOrEmpty(challengeString));

            try
            {
                // 2 - Hash the challenge string with an invalid password and ask the web service to validate the hash.
                var result = controller.GetIsValid("JohnDoe", requestId, CreatePasswordHash("InvalidPassword", challengeString));
            }
            catch (HttpResponseException ex)
            {
                // 3- Verify that a 401 Status code was returned through the exception (handled by ASP.NET)
                Assert.AreEqual(HttpStatusCode.Unauthorized, ex.Response.StatusCode);
                sawException = true;
            }

            // Verify that authentication failed for invalid password
            Assert.IsTrue(sawException);
        }

        // Scenario 3: invalid user
        [TestMethod]
        public void ValidateUserNameInvalidUser()
        {
            var sawException = false;
            var controller = new IdentityController();

            // 1- Get a random password challenge string from the web service.
            const string requestId = "ec609a4f";
            var challengeString = controller.GetPasswordChallenge(requestId);
            Assert.IsFalse(string.IsNullOrEmpty(challengeString));

            try
            {
                // 2 - Ask the web service to validate an invalid user id.
                var result = controller.GetIsValid("UnknownUser", requestId, CreatePasswordHash("pwd", challengeString));

            }
            catch (HttpResponseException ex)
            {
                // 3- Verify that a 401 Status code was returned through the exception (handled by ASP.NET)
                Assert.AreEqual(HttpStatusCode.Unauthorized, ex.Response.StatusCode);
                sawException = true;
            }
            // Verify that authentication failed for unknown user
            Assert.IsTrue(sawException);
        }
    }
}
