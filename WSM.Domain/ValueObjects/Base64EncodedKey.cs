using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.ValueObjects
{
    public class Base64EncodedKey
    {
        private string? _encodedKey;
        public Base64EncodedKey(string encodedKey)
        {
            _encodedKey = encodedKey;
        }
        public string EncodedKey
        {
            get => _encodedKey; set
            {
                var decoded = Convert.FromBase64String(value);
                if (decoded.Length != 32)
                {
                    throw new ArgumentException("Key must be 32 bytes long");
                }
            }

        }
        /// <summary>
        /// Returns the Base64EncodedKey  as a string.
        /// </summary>
        public override string ToString() => _encodedKey;
        // Implicit conversion from string to Base64EncodedKey
        public static implicit operator Base64EncodedKey(string encodedKey)
        {
            return new Base64EncodedKey(encodedKey);
        }

        // Implicit conversion from Base64EncodedKey to string
        public static implicit operator string(Base64EncodedKey base64EncodedKey)
        {
            return base64EncodedKey.EncodedKey;
        }
    }
}
