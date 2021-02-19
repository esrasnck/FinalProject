using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator: AbstractValidator<Product>
    {
        // dtolarda da olurmuş
        public ProductValidator() //=> kurallar buraya yazılırmış.
        {
            RuleFor(p => p.ProductName).NotEmpty();
            RuleFor(p => p.ProductName).MinimumLength(2);
            RuleFor(p => p.UnitPrice).NotEmpty();
            RuleFor(p => p.UnitPrice).GreaterThan(0);
            // içecek kategorisinin ürün fiyatı min. 10 tl olmalı mesela
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
            // ürünlerimin ismi a ile başlamalı gibi bir kural koymak istiyoruz gibi...
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı"); // => hata mesajı vermek istersek
            // startWithA kendi yazcağın metot.

        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
