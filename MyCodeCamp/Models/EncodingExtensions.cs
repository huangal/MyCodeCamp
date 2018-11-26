using System;
using System.Text;
namespace MyCodeCamp.Models
{
    public static class EncodingExtensions
    {
        
        public static string Base64Encode(this string value)
        {
            if (!value.IsValidString()) return null;
            return Base64Encode(value, Encoding.UTF8);
        }


        public static string Base64Encode(this string value, Encoding encoder)
        {
            if (!value.IsValidString()) return null;
            var plainTextBytes = encoder.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(this string value)
        {
            if (!value.IsValidString()) return null;
            return Base64Decode(value, Encoding.UTF8);
        }


        public static string Base64Decode(this string value, Encoding encoder)
        {
            if (!value.IsValidString()) return null;
            var base64EncodedBytes = Convert.FromBase64String(value);
            return encoder.GetString(base64EncodedBytes);
        }


        public static bool IsValidString(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }


    }
}
