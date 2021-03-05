using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public interface ICoreModule  //=> framework katmanı. her projemizde kullandığımız modul kavramıdır.
    {
        void Load(IServiceCollection services); // genel bağımlılıkları yükleyeceği için yükleme isimli bir tane metod imzası ekliuyoruz. bu parametre olarak, servise collention alsın. yani o servisleri bu amca yüklüyor olsun.
    }
}
// IserviceCollection => using Microsoft.Extensions.DependencyInjection; 'dan geliyor.

// startup dosyasındaki ConfigureService'in parametresi olan, IServiceCollection'ı vereceğiz. O yükleme işini o yapacak.