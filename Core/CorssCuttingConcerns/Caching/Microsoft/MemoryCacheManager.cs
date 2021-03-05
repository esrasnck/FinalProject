using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Core.CorssCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {

        // .net de gelen kodu, kendi sistemime uyarlıyorum. var olan birşeyi kendi sistemime uyarlıyorum. ben sana göre değil, sen benim sistemime göre çalışcaksın diyorum. buna da ADAPTER PATTERN diyoruz.
        IMemoryCache _cache;
        public MemoryCacheManager()
        {

            // IMemoryCache bir interface. çözmem için bunu service tool kullancam. injekte edemiyorum. çünkü zincir şu şekilde ileriliyor: webapi=> business=> dataccess. aspect bambaşka bir zincirin içinde. bağımlılık zinciri içinde değil. o yüzden bunun için bir servicetool yazdık. onun için de dependency resolvers da bunu belirtmem gerekiyor. senden ICahcheManager isterse, ona microsoft un memorycach manageri ver diyoruz. (core module de)
           _cache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
            // redist yazıp, injectionı değiştirdiğiniz anda herşey değişcek. bu ImemoryCache microsofttan geliyor.
        }

        // IMemoryCache => using Microsoft.Extensions.Caching.Memory;

        // GetService =>  microsoft.extensions.dependencyInjection 5.0.1 => using Microsoft.Extensions.DependencyInjection; elle verince geliyor.

        public void Add(string key, object data, int duration)
        {
            //set ile cache e değer ekleyebiliyoruz.
            _cache.Set(key, data, TimeSpan.FromDays(duration)); // => duration dakika demek
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public bool IsAdd(string key)
        {
            // bellekte benim için böyle bir cache değeri var mı yok mu ona bakıyor. birşey döndürmek istemediğim için de out olarak bunu veriyoruz. hem bool hem de o değeri döndürmem gerkeirken, birşey döndürmek istemdiğimde out_ şeklinde yazılır.cache de ki değeri verme diyorum kısacası.
            return _cache.TryGetValue(key, out _); //=> benim için var mı yok mu onu kontrol et. değer de döndürebilirim.
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        // çalışma anında bellekten silmeye çalışıyor. bellekte sınıfın instance ı var. çalışma anında müdahele etmek istiyorum. bunun için reflaction kullanılıyor.
        // reflaction çalışma anında oluşturma, müdahele etme gibi şeyleri biz bununla yapıyoruz.
        public void RemoveByPattern(string pattern) // belli bir paterne uyan cacheleri bellekkten silmek istiyoruz.
        {
            //git belleğe bak. orada memorycahe türünde olan, entriescollection (microsfot ben bunu cahcelediğimde, ben bunu buraya atıyorum diyor (entriesCollection ismi ile tutuyormuş) buluyoruz(bellekteti) 
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // entriesCollection isimli bir property var. biz ona ulaşmaya çalışıyoruz. Koleksiyona reflection ile ulaşıyorum.


            // definitaion ı memorycache olanı bul
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_cache) as dynamic;


            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>(); // her bir cache girişi diye bir liste yapıyorum

            // her bir cache elemanını ger.
            foreach (var cacheItem in cacheEntriesCollection)
            {
                // her bir değeri okuyarak her bir cache'i tek tek buna atıyorum.
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);


                cacheCollectionValues.Add(cacheItemValue);
            }

            // string olarak gönderdiğim bir regex oluşturuyorum
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //her bir cache elemenaından şu kurala uyanları bul. bu kurala uyanlar, silme işlemini gerçekleştirirken vereceğim elemanlara uyacak. anahtardan benim gezdiğim key' uygun olan varsa, onları foreach ile tek tek geziyorum. benim bulduğum değer değerlerin keylerine uyanları buluyorum. sonra da bellekten uçuruyorum.
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            // listede oluştrduğum listedeki tüm elemanları regex'e göre bulup ona atıyorum. listede oluşturduğum tüm keyleri benim gönderdiğim pattern'a uygun olan bir listede yakalamış oldum

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }
    }
}

// regex => using System.Text.RegularExpressions;
