using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    // burada da kendi middleWare'imizi yazmak istiyoruz. 
    // middle ware ise => startup daki configure metotları hazırları kullandık. başka istersek, kendi middleWare lerimizi kullanmamız gerekir.
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            // start-up daki app'leri exten ediyoruz. genişletiyoruz. bununla configure deki yaşam döngüsünün içerisine hata yakalamayı da ekle diyorum
            app.UseMiddleware<ExceptionMiddleware>();

            // sonra da exceptionMiddleware'ı yakala diyorum. .net bunu benim için yazmış. ben de o yaşam döngüsünde, yazmak istediğim kodu entegre ediyorum.
        }
    }
}
