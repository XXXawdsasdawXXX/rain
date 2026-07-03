using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Core.Common.Extensions;
using Core.Save.Models;
using UnityEngine;

namespace Core.Save
{
    internal static class SaveService
    {
        private static readonly string Password = _generateKey();
        //private static readonly string HmacKey = "HMAC_SECRET_KEY_123";
        
        private const bool _useEncryption = false;
        
        
        public static void Save<T>(T data) where T : SavedModel
        {
            try
            {
                string json = data.AsJson();

                File.WriteAllText(Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.dat"), json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e.Message}");
            }
        }

        public static bool Load<T>(out T data) where T : SavedModel
        {
            data = null;

            string path = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.dat");

            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                string json = File.ReadAllText(path);

                data = json.AsData<T>();

                return data != null;
            }
            catch
            {
                return false;
            }
        }

        private static string _generateKey()
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            using SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(deviceId + "_extra_salt"));
            return Convert.ToBase64String(hash);
        }

        private static byte[] _encrypt(string plainText, string password)
        {
            using Aes aes = Aes.Create();
            byte[] key = _deriveKey(password, aes.KeySize / 8);
            aes.Key = key;
            aes.GenerateIV();

            using ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            byte[] result = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);
            return result;
        }

        private static string _decrypt(byte[] encryptedData, string password)
        {
            using Aes aes = Aes.Create();
            byte[] key = _deriveKey(password, aes.KeySize / 8);
            aes.Key = key;

            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] cipherBytes = new byte[encryptedData.Length - iv.Length];

            Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedData, iv.Length, cipherBytes, 0, cipherBytes.Length);
            aes.IV = iv;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        private static byte[] _deriveKey(string password, int keySize)
        {
            using Rfc2898DeriveBytes pbkdf2 =
                new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("_fixed_salt_"), 10000);
            return pbkdf2.GetBytes(keySize);
        }

        private static byte[] _computeHmac(byte[] data, string key)
        {
            using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return hmac.ComputeHash(data);
        }

        private static bool _compareBytes(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }
    }
}