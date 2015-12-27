using System;

namespace docconverterlib
{
    public class Hash
    {

        public static string GetEncodedHash(Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .Substring(0, 22)               //exclude the last 2 "=="
                .Replace("+", "-")
                .Replace("/", "_");
        }

    }
}
