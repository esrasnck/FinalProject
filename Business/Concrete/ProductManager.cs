using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CorssCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        // bir iş sınıfı, başka sınıfları newlemez

        IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }


        [ValidationAspect(typeof(ProductValidator))] // add metodunu product validator'ı kurallara göre doğrula

        public IResult Add(Product product)
        {
            // business code : ehliyet örneği. ilk yardmdan 70 almış mı motordan 70 almış mı vs. gibi kurallar gibi
            // validation : ismin uzunluğu vs. tarzı şeyler... klasik :)

            #region Old style
            //if (product.UnitPrice<= 0)
            //{
            //    return new ErrorResult(Messages.UnitPriceInvalid); // bunlar validasyon kuralları aslında. Fluent validation is coming:)
            //}
            //if (product.ProductName.Length<2)
            //{
            //    return new ErrorResult(Messages.ProductNameInvalid);
            //}
            #endregion
            #region bütün projelerimde kullancağım bir tool haline getirmek için core katmanına geçiyoruz:) Bir kez yazıp/geçiceğiz
            //var context = new ValidationContext<Product>(product); // => product için doğrulama yapcam diyorum.
            //ProductValidator productValidator = new ProductValidator();
            //var result = productValidator.Validate(context);
            //if (!result.IsValid)
            //{
            //    throw new ValidationException(result.Errors);
            //}
            #endregion
            #region
            // attribute dan ötürü gidecek :) ValidationTool.Validate(new ProductValidator(), product);// değişen iki şey bu. diğerleri aynı kalıyor.
            #endregion

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            // iş kodları => iş kodlarından geçiyorsa, benim veri erişimi çağrımam gerek. bu safha da dependency injection olaya giriyor.

            if (DateTime.Now.Hour ==22) // 22 de sistemi kapamak istiyoruz. ürün list istemiyoz
            {
                // boş ürün döndürmece
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetByID(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=> p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {

            if (DateTime.Now.Hour == 22) // 22 de sistemi kapamak istiyoruz. ürün list istemiyoz
            {
                // boş ürün döndürmece
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            else
            {
            return new SuccessDataResult<List<ProductDetailDto>>( _productDal.GetProductDetails());
            }
        }
    }
}
