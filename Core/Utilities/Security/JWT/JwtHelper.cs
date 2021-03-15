using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper:ITokenHelper
    {
        public IConfiguration Configuration { get; } // appsetting.json'ı okumamıza yarıyor. bunu enjekte ettiğimizde bilgiye erişebilyo. Microsoft.Extensions.Configuration;'dan  geliyor.
        private TokenOptions _tokenOptions; // appsettingste okuduğum şeyleri bir nesneye atacam. o nesne configurasyonda okuğum şeyleri atacağım class olacak.
        private DateTime _accessTokenExpiration; // access token ne zaman geçersizleşecek.
        public JwtHelper(IConfiguration configuration) // bunu bana .net core veriyor. enjekte ttim.
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>(); // benim değerlerim için, configurasyondaki(appsetting.jason) alanı bu. oradan tokenOptions bölümünü al ve onu oluşturduğum sınıf ile maple diyorum. GetSection  Microsoft.Extensions.Configuration; 'dan alıyor. castle'dan değil. ona dikkat.

        }

       
        
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            // bir tokenda olması gerekenler
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            // şu andan itibaren dakika ekle. ne kadar süre ekle? tokenOptions'a verdiğimiz süre.(appjsonsettings kısmında. 10dk bunun için)
            //bunu configurasyondan alıyor. bu configurasyon ne? bizim buraya enjekte ettiğimiz yapı. bizim api uygulamamızdaki appsettingjason dosyasını okumamıza yarıyor.
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey); // yazdığım helperdaki metot.
            // tokenOption(appsetting.jason) ordaki securitykey'i kullanarak, securityKey formatında vermem gerek. onu daha önce yazdık. encripton=> security key helper da byte[]'a çevirdik stringi
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);  // yazdığım helperdaki metot. 
            // hangi anahtar/ algorimayı kullanacağımızı söylüyor. (yazmıştık)
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            // ortaya böylece bir jwt(jason web token) çıkıyor. bunu üretmek için bazı parametreleri kullanıyoruz. tokenOption(appsettingde), kullanıcı bilgisi(hangi kullanıcı için), bu kullanıcın claimleri neler? neyi kullanarak yazacak(signingcredentials) => bunu bir metot vasıtası ile aşağıda oluşturduk biz.
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }


     

        // token'ı burada oluşturuyoruz. biz bu bilgileri vererek, oluşturuyoruz. JwtSecurityToken; System.IdentityModel.Tokens.Jwt'kütüphanesinden geliyor. jwt'leri oluşturmak, serileştirmek ve doğrulamak için kullanılan yapıdır.
        // signingCredentials; ise Microsoft.IdentityModel.Tokens'dan geliyor.
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken( // JwrSecurityToken oluşturmasını sağlıyor bu metot.
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now, // şu andan önceki değer verilemez demek.
                claims: SetClaims(user, operationClaims), // kullanıcının bilgileri rolleri. bunun için de bir metot oluşturduk. aşağıda
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        // bunları parametre alarak bir claim listesi oluştur diyoruz. (yukarıda)
        // bir jwt'de sadece yetkiler olmuyor. claimler de oluyor. bunlar userID, email, isim soyadı, rolü olabilir.
        // claim = system.security.claims'den geliyor
        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            // claim sınıfında bunlar yok : addNameIdentifier vs. .net'de var olan bir nesneye yeni metotlar ekleyebiliyoruz.(jwtToken'da ekleyebiliriz gbi. Buna "extension" = genişletmek deniyor.
            var claims = new List<Claim>(); // base'i
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray()); // kullanıcın veritabanından çektğim rolleri çekip, array'a basıp çekebiliyorum.

            return claims;
        }
    }
}
