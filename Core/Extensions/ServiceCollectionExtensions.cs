using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions // extension olduğu için
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ICoreModule[] modules)
        {
            // this => neyi genişletmek istiyoz. onu yazıyoruz.
            foreach (var module in modules)
            {
                module.Load(services); // tüm modullerimi .net core'a eklemiş olacam
            }

            return ServiceTool.Create(services); 
        }

        // bu metodla, istediğim her modülü ekleyip, döngüde dönüp create metodu ile(ServiceTool'daki) üretiyim diyor.
    }
}
