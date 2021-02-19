using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Utilities.Interceptors
{

    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        { // git classın attributelarını oku. bak onları onları listeye koy
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>
                (true).ToList();
            // ilgili metotlarını atributelarını oku onları listeye koy
            var methodAttributes = type.GetMethod(method.Name)
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);

          //  classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger))); // default loflama eklemece. altyapı hazırlamnsın. gelecek

            return classAttributes.OrderBy(x => x.Priority).ToArray();  // onların çalışma sırasını öncelik listesine koy
        }
    }
}
