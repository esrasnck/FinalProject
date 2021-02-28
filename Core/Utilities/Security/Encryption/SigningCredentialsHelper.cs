using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper // imzalama
    {
        // web api'nin kullanabileceği, jason web tokenlarının oluşturulabilmesi için, credential'a yani anahtara ihtiyaç var. o yüzden securty key verecez anahtar olarak. o da bize imzalama yetkisini veriyor olacak
        // credential: bir sisteme girebilmek için elimizde olanlardır.
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);

            // asp.net'e, sen bir hashing işlemi yapacaksın. anahtar olarak bu securityKey'i kullan. şifreleme olarak da güvenlik algoritmalarından, hmacsha512'yi kullan diyoruz burada. biz bunu hashing de söyledik. ama bunu asp net'e de söylemeye ihtiyac var. o yüzden burayı yapıyoruz.
        }
    }
}
