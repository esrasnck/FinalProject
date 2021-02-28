using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
   public class HashingHelper
    {

        public static void CreatePasswordHash(string password, out byte [] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // string'in byte hali :)
            }

        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) // bu sınıfa bu salt'ı anahtarı kullanması gerektiğini söylüyorz
            {
                // hesaplanan hash
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // gelen password'ü tekrar hashlemek gerek ki karşılaştırabilelim.

                for (int i = 0; i < computedHash.Length; i++) // hesaplanan passwordün değerlerini tek tek dolaş.
                {
                    if (computedHash[i] != passwordHash[i]) // hesaplanan hash'in i. değeri, veritabanından gönderilen hash in i. değerine, false döndür
                    {
                        return false;
                    }
                }

                return true; // eşit ise true döndür. if'e hiç girmezse demekki hepsi aynıdır. true döner gibi.
            }

        }
    }
}
