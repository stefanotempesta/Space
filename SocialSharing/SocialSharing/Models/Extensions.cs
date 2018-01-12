using SocialSharing.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocialSharing.Models
{
    public static class Extensions
    {
        public static string Encrypt(this string clearMessage, string key, string vector)
        {
            byte[] message = Encoding.Default.GetBytes(clearMessage);
            string cipher = string.Empty;

            SymmetricAlgorithm des3 = SymmetricAlgorithm.Create("3DES");
            des3.Key = Encoding.Default.GetBytes(key);
            des3.IV = Encoding.Default.GetBytes(vector);

            using (MemoryStream mstream = new MemoryStream())
            {
                using (CryptoStream cstream = new CryptoStream(mstream, des3.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cstream.Write(message, 0, message.Length);
                }

                byte[] cryptostream = mstream.ToArray();
                cipher = Convert.ToBase64String(cryptostream);
            }

            return cipher;
        }

        public static string Decrypt(this string encryptedMessage, string key, string vector)
        {
            byte[] cipher = Convert.FromBase64String(encryptedMessage);
            string message = null;

            SymmetricAlgorithm des3 = SymmetricAlgorithm.Create("3DES");
            des3.Key = Encoding.Default.GetBytes(key);
            des3.IV = Encoding.Default.GetBytes(vector);

            using (MemoryStream mstream = new MemoryStream(cipher))
            {
                using (CryptoStream cstream = new CryptoStream(mstream, des3.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] clearstream = new byte[cipher.Length];
                    cstream.Read(clearstream, 0, cipher.Length);

                    message = Encoding.Default.GetString(clearstream).TrimEnd('\0');
                }
            }

            return message;
        }

        public static ExpandoObject ToDynamic(this object data)
        {
            dynamic expando = new ExpandoObject();

            foreach (PropertyInfo property in data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanRead)
                {
                    (expando as IDictionary<string, object>).Add(property.Name, property.GetValue(data, null));
                }
            }

            return expando;
        }

        public async static Task GeolocateIpAddress(this AuditTrailEntry entry, IGeolocator geolocator)
        {
            using (var db = new DatabaseContext())
            {
                var info = await geolocator.Geolocate(entry.UserHostAddress);
                if (info != null)
                {
                    entry.UserCountry = info.Country;
                    db.Entry(entry).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
