using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CorssCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        // içerisimnde validate metodu olan interface aradık. bulduk. PRoductValidator'a miras gelen yer :) valida bu interfaceden geldi.
        // entity dto herşey ekleyebilirm. o yüzden object
        public static void Validate(IValidator validator, object entity )  // => sonra da değişen şeyler parametre olarak verilir :)
        {
            var context = new ValidationContext<object>(entity); // => product için doğrulama yapcam diyorum.
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

    }
}
