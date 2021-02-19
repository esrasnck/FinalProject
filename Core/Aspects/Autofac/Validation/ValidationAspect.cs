using Castle.DynamicProxy;
using Core.CorssCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType) // validator type ı ver bana diyor. [ValidationAspect(typeof(ProductValidator))] a gidiyor.
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType)) // eger bu IValidator değilse kız. exception at.
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil"); 
            }

            _validatorType = validatorType;
        }

        protected override void OnBefore(IInvocation invocation) // metotd interceptiondaki onbefore metodu bu
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);  //bu reflection: çalışma anında birşeyleri çalıştırabilmenizi sağlıyor. ör: newleme işini çalışma anında yaptırma. Product validator ın instance ını oluştur.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; // product validator'ın çalışma tipini bul.. bussinesdaki base. onun çalıştığı generic argümanlarından ( product) ilkini bul.[0]'dan
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); // onun parametrelerini bul. ilgili metodun paremetrelerine bak. entitylerine karşılık gelen(mesela product) 
            foreach (var entity in entities) // her birini tek tek gez. validation tool ile validate et
            {
                ValidationTool.Validate(validator, entity); // validationtool merkezi bir noktaya alındı.
            }
        }
    }
}
