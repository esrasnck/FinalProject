using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken // erişim anahtarı
    {
        public string Token { get; set; } // anahtar(jason web token) kullanıcı bize password ve user name verecek. biz de ona token ve bitiş tarihi vercez gibi.

        public DateTime Expiration { get; set; } // bitiş süresini verdiğimiz değer.
    }
}
