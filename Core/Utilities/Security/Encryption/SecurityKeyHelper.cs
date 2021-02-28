using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryption
{
     public class SecurityKeyHelper
    {
        // işin içerisinde şifreleme olan sistemlerde, bizim herşeyi bir byte[] formatında oluşturmamız gerek. basit bir string ile key oluşturamıyoruz. bunları asp.net'in json web token servislerinin anlayacağı hale getirmemiz gerekiyor.

        // appsettings.json'da oluşturduğumuz securitykey 
        // string securitykey : appsetting.json'a yazdığımız key
        // bu SecurityKey; Microsoft.IdentityModel.Tokens dan geliyor.
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
