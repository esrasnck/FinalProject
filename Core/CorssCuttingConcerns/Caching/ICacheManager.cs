using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CorssCuttingConcerns.Caching
{
   public interface ICacheManager
    {
        // redist gibi farklı teknikler var. devarch da var. gibi
        T Get<T>(string key);
        // hangi tipte dönüştürülmesi gerektiğini söylüyor olacaz.
        object Get(string key);
        // generic olmayan versiyonu bu. ama tip dönüşümünü unutma.
        void Add(string key, object data, int duration);
        // herşeyin base'i obje olduğundan data obje. bir de duration olarak zaman verecez.
        bool IsAdd(string key);
        // cache de var mı diye kontrol yapabilmemizi sağlıyor
        void Remove(string key);

        // cache den uçurma. ben sana bir key versem, sen onu cache'den ucurur musun?
        void RemoveByPattern(string pattern);
        // desem ki ismi get ile başlayanları uçur gibi bir parametre versem. (regex pattern)
        // başı sonu önemli değil, içinde get ya da category vs. olan gibi.
    }
}
// bu interface, tüm alternatiflerimin interface'i olacak. yarın öbür gün redist ya da elastic search ün log station'ı entegre etmek istersek, gelip bunu implement edebiliriz. bu herhangi bir teknolojiden bağımsız olacak.