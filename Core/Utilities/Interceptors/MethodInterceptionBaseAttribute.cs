using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]

    // anlamı : classlara, metotlara attribute olarak verebilirsin. birden fazla ekleyebilirsin. ve inherit edilen noktada da bu attribute çalışsın
    // diyebilirsin gibi.
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {


        public int Priority { get; set; } // hangi attribute önce çalışsın manasında. önce loglama sonra authorization gibi gibi

        public virtual void Intercept(IInvocation invocation)
        {

        }
    }
}
