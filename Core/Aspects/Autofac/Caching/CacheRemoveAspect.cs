using Core.CorssCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect:MethodInterception
    {
        string _pattern;
        ICacheManager _cacheManager;
        public CacheRemoveAspect(string pattern)
        {

            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }
        // data bozulduğu zaman kullanılır. veriyi manupile eden metotlarına uygularsın.
        protected override void OnSuccess(IInvocation invocation) //=> ürün eklenme, güncelleme, silme operasyonlarında çalıştırmamızda fayda var. çünkü yeni ürün eklendi. cache'in temizlenmesi gerekiyor..
        {
            _cacheManager.RemoveByPattern(_pattern);
        }

    }
}

// getService => using Microsoft.Extensions.DependencyInjection;