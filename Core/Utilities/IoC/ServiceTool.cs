using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
   public static class ServiceTool // sistemin servislerini merkezi noktadan kontrol ediyor olacaz. Aynı zamanda bu bir ioc resolver olacak.
    {
        // Ioc= inversion of control. injectionları kontrol edebildiiğimiz yer.

        // dependencyleri yakalayabilmek için yazdık. injection alt yapımızı okuyabilmeye yarayan bir araç bu.
        public static IServiceProvider ServiceProvider { get; private set; }

        //.net'in servicelerini al ve onları kendin build et. bu kod benim api da oluşturudğum injectionları oluşturabilmeye yarıyor. bundan sonra ben, I..servicelerin hepsini, bu tool üzerinden yakalayabilceğim. (tüm servisleri)
        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }

        // bu vasıta ile, .netCore'un servislerine erişebiliyorum.
        

        // IServiceCollection : Microsoft.Extensions.DependencyInjection;'dan geliyor.
    }
}
