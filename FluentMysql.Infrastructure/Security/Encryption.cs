using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FluentMysql.Infrastructure.Security
{
    public class Encryption
    {

        public string password = "";
        public string salt = "";

        public string EncryptString(string Str)
        {
            if (password.Trim() != "" && this.salt.Trim() != "")
            {
                return EncryptString(Str, this.password, this.salt);
            }
            else
            {
                return "";
            }
        }

        public string EncryptString(string Str, string Password, string Salt)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Password, Encoding.UTF8.GetBytes(Salt));
                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    aes.IV = aes.Key;

                    using (MemoryStream encryptionStream = new MemoryStream())
                    {
                        using (CryptoStream encrypt = new CryptoStream(encryptionStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] utfD1 = UTF8Encoding.UTF8.GetBytes(Str);
                            encrypt.Write(utfD1, 0, utfD1.Length);
                            encrypt.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(encryptionStream.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        
        public string DecryptString(string Str)
        {
            if (password.Trim() != "" && this.salt.Trim() != "")
            {
                return DecryptString(Str, this.password, this.salt);
            }
            else
            {
                return "";
            }
        }

        public string DecryptString(string Str, string Password, string Salt)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Password, Encoding.UTF8.GetBytes(Salt));

                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    aes.IV = aes.Key;

                    using (MemoryStream decryptionStream = new MemoryStream())
                    {
                        using (CryptoStream decrypt = new CryptoStream(decryptionStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            byte[] encryptedData = Convert.FromBase64String(Str);

                            decrypt.Write(encryptedData, 0, encryptedData.Length);
                            decrypt.Flush();
                        }

                        byte[] decryptedData = decryptionStream.ToArray();

                        return UTF8Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
