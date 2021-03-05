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

        public CacheAspect(int duration=60) // => değer girmezse default 60 dk => süre vermezsek, 60 dk diyoruz. 
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>(); // aspect olduğu için injection yapamıyoruz. eğer farklı bir sistem kullanırsak, buraya dokunmuyoruz. mesela Redis'e geçmek istediğimiz zaman, yapılacak tek şey redis klasörünü oluşturmak. Core module'e gidip onu vermek. başka birşey yapmıyoruz.
        }

        // key oluşturcam mesela. ilk olarak, metodun ismini bulmaya çalışıyorum.
        public override void Intercept(IInvocation invocation) // life cycle'ı harekete geçir dedik. ezdiık. invocation metot. getall çalışmadan bu kodları çalıştıryoruz.
        {
            // örnek: key oluştururken. mesela getall. reflectedType demek, name spacesini + managerı al demek. sonra nokta yok. metotun adını al.
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}"); // class ve metodun ismini vermek için. key değeri başlangıcı
            var arguments = invocation.Arguments.ToList(); //=> arguman listem varsa metodun parametrelerini listeye çevir

            var key = $"{methodName}({string.Join(",",arguments.Select(x => x?.ToString() ?? "<Null>"))})"; // metodun parametre değeri varsa, o parametre değeri tek tek metodun içine ekle demek
            if (_cacheManager.IsAdd(key)) // böyle bir key oluşturduk. diyorum ki git bak. bellekte böyle bir metot var mı?
            {
                // eğer bu varsa, bu metodu çalıştırma
                invocation.ReturnValue = _cacheManager.Get(key); // metodu çalıştırmadna geri dön demek
                // veritabanından gitmesin de onun değeri cache deki data olsun demek : returnValue
                return;
            }
            // yoksa

            invocation.Proceed(); // metod çalıştı
            _cacheManager.Add(key, invocation.ReturnValue, _duration); //=> son çalışan metodu alıp cache ekle diyoruz.
        }
    }
    // bunun görüntüsü ProductManager.GetByCategory(1,asdadsa) => key'in görünümü. farklı parametreler de gelirse, key değişmiş olacak gibi.

    // Getservice using Microsoft.Extensions.DependencyInjection;
}
