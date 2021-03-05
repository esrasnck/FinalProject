using Castle.DynamicProxy;
using Core.CorssCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect:MethodInterception
    {
        // duration'a ihtiyac var. bu cache'i kaç dakika tutmak istiyorsun. ikincisi bu metodun kendisi

        int _duration;
        ICacheManager _cacheManager;

        public CacheAspect(int duration=60) // => değer girmezse default 60 dk
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}"); // class ve metodun ismini vermek için. key değeri başlangıcı
            var arguments = invocation.Arguments.ToList(); //=> arguman listem

            var key = $"{methodName}({string.Join(",",arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (_cacheManager.IsAdd(key))
            {
                // eğer bu varsa, bu metodu çalıştırma
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            // yoksa

            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration); //=> son çalışan metodu alıp cache ekle diyoruz.
        }
    }
    // bunun görüntüsü ProductManager.GetByCategory(1,asdadsa) => key'in görünümü. farklı parametreler de gelirse, key değişmiş olacak gibi.

    // Getservice using Microsoft.Extensions.DependencyInjection;
}
