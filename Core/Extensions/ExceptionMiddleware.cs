using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    // api'ler; middleware denen bir kavram var. bu sarmanlamaya yarıyor. Olur da bir hata olursa,
    // nasıl davranaymın kodu Exception middle ware'de olacak.

    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // bütün kodları try-catch içine alıyor
            try
            {
                // hata olmazsa yardır
                await _next(httpContext);
            }
            catch (Exception e)
            {
                // hata olursa, dur bakalım ben seni handle edecem demek.
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            // hata olursa, kontrolden geçiriyoruz. 
            httpContext.Response.ContentType = "application/json";
            // tarayıca ben sana bir json gönderiyorum demek.
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // bir hata olursa, statu kodunu bu vermişiz. gönderilen hata kodu

            //mesajı da bu vermişiz.
            string message = "Internal Server Error";
            // hata listesi oluşturuyorum. dönen şey fluent validationdan validation failure / errors listesi
            // yani ben dönen şeyleri bir listemi oluşturup/ içine koyacam. bunu nasıl yakalayabilirim?
            IEnumerable<ValidationFailure> errors;

            if (e.GetType() == typeof(ValidationException)) // aldığım hata validation ise, ona göre bir mesaj gidiyor gibi. bunu da yukardan alıyoruz. InvokeAysnc'den
            {
                // hata buysa, mesajı böyle demişiz.
                message = e.Message;
                // errors'ü doldurup / döndürmek istiyorum.
                errors = ((ValidationException)e).Errors; // validation hatası olduğundan, internal server error değil o yüzden statu code değişcek
                httpContext.Response.StatusCode = 400; // bu sistemsel olarak değil, bad request olarak verdim.

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    // bu sadece benim objem. üst taraftaki daha bir çokomelli
                    // doğrulama hatasına göre bir dönüş gerçekleşmiş oldu.
                    StatusCode = 400,
                    Message =message,  // mesajı sistemle ilgili verme!
                    Errors = errors // validationa uygun olan bir hata nesnesi oluşturup içine ekledim.

                }.ToString()); // jsona çevirsin diye

            }

            // response'u da bu şekilde yardır demek.bu benim için sistemsel bir hata. ama üste farklı birşey yapmam gerek.
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
