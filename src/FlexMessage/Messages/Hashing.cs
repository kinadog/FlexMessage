using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FlexMessage.Configs;

#pragma warning disable SYSLIB0041

namespace FlexMessage.Messages;

public static class Hashing
{
    #region Field

    private const string saltString = Config.HashKey;
    private const int iterations = 10000;
    private const int keyLength = 32;
    private static readonly byte[] salt = Encoding.UTF8.GetBytes(saltString);

    #endregion

    #region Method

    /// <summary>
    ///     SignalR ConnectionId 암호화
    /// </summary>
    public static string Encrypt(string plainText)
    {
        var iv = new byte[16];
        byte[] array;

        using (var aes = Aes.Create())
        {
            aes.Key = GenerateKey(saltString);
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                array = ms.ToArray();
            }
        }

        return Convert.ToBase64String(array);
    }

    /// <summary>
    ///     SignalR ConnectionId 복호화
    /// </summary>
    public static string? Decrypt(string cipherText)
    {
        try
        {
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = GenerateKey(saltString);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    ///     AES용 Key 생성
    /// </summary>
    private static byte[] GenerateKey(string password)
    {
        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations);
        return rfc2898DeriveBytes.GetBytes(keyLength);
    }

    #endregion
}