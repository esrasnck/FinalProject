using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Business.CCS;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Http;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule: Module
    {
        protected override void Load(ContainerBuilder builder) // uygulama hayata geçtiği zaman, yayınlandığı zaman, uygulama ayağa kalktığı zaman çalışan metot
        {
            // bu alttaki metot, start up dosyasında yaptığımız services kısmına karşılık geliyor.
            // biri senden IProductService isterse,  sen ona ProductManageri newleyip ver demek.
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();
            
            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<OrderManager>().As<IOrderService>().SingleInstance();
            builder.RegisterType<EfOrderDal>().As<IOrderDal>().SingleInstance();

            builder.RegisterType<CustomerManager>().As<ICustomerService>().SingleInstance();
            builder.RegisterType<EFCustomerDal>().As<ICustomerDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

           // builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

            //     builder.RegisterType<FileLogger>().As<ILogger>().SingleInstance(); //=> bu mevzuyu reflection ile yapıyor.


            // bütün sınıflar için önce bu çalışıyor(aşağıdaki)

            var assembly = System.Reflection.Assembly.GetExecutingAssembly(); // çalışan uygulama içinde

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces() // implemente edilmiş interfaceleri bul
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector() // onlar için bunu çağır diyor
                }).SingleInstance();

        }

    }
}
