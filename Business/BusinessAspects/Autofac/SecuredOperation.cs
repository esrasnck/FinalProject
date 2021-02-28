using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Business.Constants;

namespace Business.BusinessAspects.Autofac
{
    // bu securedOperation JWT için.
    public class SecuredOperation : MethodInterception //yetki kontrolü burada yapılıyor.
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;


        // biz jwt'de configürationI enjekte edebilmiştik. ama aspect'e edemiyoruz. biz de onun  yerine bunu yapıyoruz yamulmuyorsam.
        public SecuredOperation(string roles) // bana rolleri ver diyoruz. admin mi user mi vs. roller virgulle ayrılarak geliyor. attribute için
        {
            _roles = roles.Split(','); // metini belirttiğim karaktere göre ayırıp array'e atıyor. 
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        // IHttpContextAccessor : jwt ile istek yapıyoruz. her istek için bir httpcontext'i oluşur. herkese o noktada bir thread oluşur. Microsoft.AspNetCore.Http;'den gelir.
        protected override void OnBefore(IInvocation invocation) // metot interceptiondan geliyor. bu adamın yetkisi var mı diye bak diyor
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role)) // bana gönderdiğin role varsa
                {
                    return; // metoda devam et
                }
            }
            throw new Exception(Messages.AuthorizationDenied); // ama yoksa yetkin yok hatası ver diyor.
        }


        // GetService için bussiness a paket kurcaz. Autofaclerle ilgili paketler ve Microsoft.Extensions.DependencyInjection bir de bundan
    }
}
