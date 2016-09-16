using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Utilities.Encryption
{
    /// <summary>
    /// 常用的加密字符串辅助类
    /// </summary>
    public class Encryption
    {
        public Encryption() { }

        /// <summary>
        /// MD5 hash加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Md5Encode(string s)
        {
            var md5 = new MD5CryptoServiceProvider();
            var result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(s.Trim())));
            return result;
        }


        #region DES加密解密
        //默认密钥向量
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string DesEncode(string encryptString, string encryptKey = "www.mochoujun.com")
        {
            encryptKey = encryptKey.Substring(0, 8).PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            var dCSP = new DESCryptoServiceProvider();
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return System.Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string DesDecode(string decryptString, string decryptKey = "www.mochoujun.com")
        {
            try
            {
                decryptKey = decryptKey.Substring(0, 8).PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = System.Convert.FromBase64String(decryptString);
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }

        }
        #endregion
    }
}
