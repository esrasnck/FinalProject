using Core.CorssCuttingConcerns.Caching;
using Core.CorssCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            // start-up da yazdığımı kod
            services.AddMemoryCache();
            // .net'in kendisinin.Memory cache. başka bir sistem istersem, burayı değiştircem. IMemoryCache için  services.AddMemoryCache(); yapacam. gibi. 
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            // using Core.CorssCuttingConcerns.Caching;    => usingler bunlar.
            // using Core.CorssCuttingConcerns.Caching.Microsoft;

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //=> using Microsoft.AspNetCore.Http;
            services.AddSingleton<Stopwatch>();
        }
    }
}

//   ICoreModule => using Core.Utilities.IoC;  startup da services add dediğim şeyleri (merkezi olanları) buraya ekleyeceğim
