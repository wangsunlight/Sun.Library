﻿using Sun.Core.DataEncryption.Encryptions;

namespace Sun.Core.Extensions
{
    /// <summary>
    /// 字符串加密拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 字符串的 MD5
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static string ToMD5Encrypt(this string text)
        {
            return MD5Encryption.Encrypt(text);
        }

        /// <summary>
        /// 字符串的 MD5
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hash"></param>
        /// <returns>string</returns>
        public static bool ToMD5Compare(this string text, string hash)
        {
            return MD5Encryption.Compare(text, hash);
        }

        /// <summary>
        /// 字符串 AES 加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <param name="skey"></param>
        /// <returns>string</returns>
        public static string ToAESEncrypt(this string text, string skey)
        {
            return AESEncryption.Encrypt(text, skey);
        }

        /// <summary>
        /// 字符串 AES 解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="skey"></param>
        /// <returns>string</returns>
        public static string ToAESDecrypt(this string text, string skey)
        {
            return AESEncryption.Decrypt(text, skey);
        }

        /// <summary>
        /// 字符串 DESC 加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <param name="skey">密钥</param>
        /// <returns>string</returns>
        public static string ToDESCEncrypt(this string text, string skey)
        {
            return DESCEncryption.Encrypt(text, skey);
        }

        /// <summary>
        /// 字符串 DESC 解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="skey">密钥</param>
        /// <returns>string</returns>
        public static string ToDESCDecrypt(this string text, string skey)
        {
            return DESCEncryption.Decrypt(text, skey);
        }

       
    }
}
