using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JPush.Core.Extensions
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    public static class Extension
    {
        #region MD5
        /// <summary>
        /// Converts a string to MD5.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.InvalidOperationException">ToMD5</exception>
        public static string ToMD5(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                byte[] data = encoding.GetBytes(stringObject);
                return ToMD5(data).ToUpperInvariant();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ToMD5", ex);
            }
        }

        /// <summary>
        /// Converts a byte array to MD5.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.InvalidOperationException">ToMD5</exception>
        public static string ToMD5(this byte[] bytes)
        {
            try
            {
                var md5Provider = new MD5CryptoServiceProvider();
                byte[] hashBytes = md5Provider.ComputeHash(bytes);
                string result = BitConverter.ToString(hashBytes);
                return result.Replace("-", "").ToUpperInvariant();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ToMD5", ex);
            }
        }
        #endregion

        /// <summary>
        /// To base64. Default encoding is UTF-8.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64(this string source, Encoding encoding = null)
        {
            try
            {
                byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(source);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Merges the specified container.
        /// </summary>
        /// <typeparam name="TKey">The type of the attribute key.</typeparam>
        /// <typeparam name="TValue">The type of the attribute value.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> container, TKey key, TValue value)
        {
            if (container != null)
            {
                if (container.ContainsKey(key))
                {
                    container[key] = value;
                }
                else
                {
                    container.Add(key, value);
                }
            }
        }
    }
}