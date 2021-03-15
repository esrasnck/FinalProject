using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); ///=> her yapılan istek ile ilgili oluşan context. isteğin başlangıcından bitişine kadar(requesten response'a kadar) olan işlemin takibini bu vatandaş yapıyor. Bu injection noktasında devreye giremediği için, servicetool'u kullandık. => bunu farklı projelerd de kullanacağmız için core'a Ioc'ye aldık.

            services.AddCors(); // => Cors injection.
            #region IProduct service gibi bir bağımlılı gösterirse, arka planda bana productmanager'ı newle demek. Bu mevzu kendi alt yapısı
            //services.AddSingleton<IProductService, ProductManager>();
            // services.AddSingleton<IProductDal,EfProductDal>();

            #endregion

            //asp.net web api'ya diyoruz ki, bu sistemde jwtbearer kullanılacak. haberin olsun diyoruz. 
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });

            // sadece core modulü değil, başka modülleri de buraya eklemek için böyle birşey yapıyorum.
            // gelmediği için Iservice interface'ini extend ediyoruz.
            services.AddDependencyResolvers(new ICoreModule[]   // using Core.Extensions
            {
                // burada bir sürü modül oluşturup/ ekleyebiliriz. httpContext'i buraya ekledik gibi.
                new CoreModule(),
            });


            //ServiceTool.Create(services); => dependency resolution ile ilgili servisler devreye girmiyor. ben servicetool'u kullanarak bunu haber verdim. servislerin, autofac'in haberdar olacağı noktaya çekmiş oldum

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // middeleware, asp.net uygulamalarında hangsine ihtiyac varsa sırayla devreye tokuyoruz.
        {
            //ServiceTool.ServiceProvider = app.ApplicationServices;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // cors configuration. buradaki sıra önemli.
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
