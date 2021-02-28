using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
   public static class ClaimExtensions // bir extention metot yazabilmek için, hem metot hem de class'ın static olması gerekiyor(c# da)
    {
        // claim nesnesini (system.security.claims'den gelen) ICollection var mesela. Böyle birşey görürsek, bunun anlamı,bu metot, claim nesnesinin içine eklenecek demek. ne ICollection ne de claim bize ait. .net de var. böyle birşey gördüğüm anda, bunu genişletiyorum demek.
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            // System.IdentityModel.Tokens.Jwt;'den JwtRegisteredClaimNames buradan gelir.
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            // bana verilen rolleri listeye çevir. tek tek dolaş ve claim'e ekle... 
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
