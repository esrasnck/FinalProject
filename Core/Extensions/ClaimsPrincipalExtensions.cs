using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions // bir kişinin claimlerini ararken, .net bizi biraz uğraştırıyor. o yüzden bizim bu kodları yazmamız gerekir.
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            // json web tokenden gelen claimleri okumak için, claimPrincipal'ı genişletiyoruz. 
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;

            // hangi claimType : roller ini bulmak için yazmam gerekiyor. ?'de nullable demek. claimType yoksa null olabilir demek gibi...

        }

        // ClaimsPrincipal : bir kişinin jwtden gelen claimlerini ulaşmak/okumak için .net ile gelen class. onu da burada exten ediyoruz ( genişletiyoruz.) System.Security.Claims'den gelir.
        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);

            // rolleri istediğimde de bunu döndürüyor.
        }
    }
}
