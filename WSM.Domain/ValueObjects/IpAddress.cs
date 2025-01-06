using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.ValueObjects
{
    public class IpAddress
    {
        private string? _value;
        public IpAddress(string value)
        {
            _value = value;
        }
        public string? Value { get => _value;set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value), "IP address cannot be null.");
                }
                if (IsValid(value))
                    _value = value;
                else
                    throw new ArgumentException("Invalid IP Address.");
            } }


        public override string ToString() => _value;

        public static bool IsValid(string address)
        {
            var parts = address.Split(',');
            foreach (var part in parts)
            {
                if (!IPAddress.TryParse(part.Split('/')[0], out _))
                    return false;
                if (part.Contains("/") && (!int.TryParse(part.Split('/')[1], out var prefix) || prefix < 0 || prefix > 128))
                    return false;
            }
            return true;
        }
        // Implicit conversion from string to Base64EncodedKey
        public static implicit operator IpAddress(string ipAddress)
        {
            return new IpAddress(ipAddress);
        }

        // Implicit conversion from Base64EncodedKey to string
        public static implicit operator string(IpAddress ipAddress)
        {
            return ipAddress.Value;
        }
    }
}
