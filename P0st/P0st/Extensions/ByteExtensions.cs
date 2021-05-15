using System;
using System.Linq;

namespace P0st.Extensions
{
    public static class ByteExtensions
    {
        public static byte[] FromUri(this string duri)
        {
            return Convert.FromBase64String(duri.Split(',').Last());
        }
        
    }
}