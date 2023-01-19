using System.Security.Cryptography;
using UnityEngine.Events;
using System;


namespace Tenacity.Utility.Methods
{
    public static class Hasher
    {
        private static readonly System.DateTime Jan1st1970 = new System.DateTime
            (1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);


        public static string GenerateHash()
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(
                System.Text.Encoding.Default.GetBytes(GenerateString())
            );

            return System.BitConverter.ToString(data);
        }

        public static string GenerateString()
        {
            return System.Guid.NewGuid().ToString() + CurrentTimeMillis();
        }

        public static long CurrentTimeMillis()
        {
            return (long)(System.DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static byte[] GenerateHashArray()
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(
                System.Text.Encoding.Default.GetBytes(GenerateString())
            );

            return data;
        }

        public static string GenerateHash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(
                System.Text.Encoding.Default.GetBytes(input)
            );

            return System.BitConverter.ToString(data);
        }

        public static byte[] GenerateHashArray(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(
                System.Text.Encoding.Default.GetBytes(input)
            );

            return data;
        }

    }

    
    /// <summary>
    /// Float extension class.
    /// </summary>
    public static class FloatHelper
    {
        /// <summary>
        /// Maps float value from interval (istart, istop) to (ostart, ostop).
        /// </summary>
        /// <param name="value">Value to be mapped.</param>
        /// <param name="istart">Original min value.</param>
        /// <param name="istop">Original max value.</param>
        /// <param name="ostart">Relative min value.</param>
        /// <param name="ostop">Relative max value.</param>
        /// <returns>Mapped float value.</returns>
        public static float Map(this float value, float istart, float istop, float ostart, float ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }
    }

    public static class ButtonHelper
    {
        public static void SetAction(this UnityEvent evt, Action act) {
            evt.RemoveAllListeners();
            if (act != null)
            {
                evt.AddListener(new UnityAction(act));                
            }
        }
    }
}