using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ApiAspNetCore6.Services
{
    public class HashService
    {
        public LearningAboutHash Hash(string text)
        {
            var salt = new byte[16];
            using(var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return Hash(text, salt);
        }
        public LearningAboutHash Hash(string text, byte[] salt)
        {
            var key = KeyDerivation.Pbkdf2(
                password: text,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32
                );
            var hash = Convert.ToBase64String(key);
            return new LearningAboutHash
            {
                Hash = hash,
                Salt = salt
            };
        }
    }

}
