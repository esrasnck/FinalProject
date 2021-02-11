using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
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

        public IResult Add(Product product)
        {
            if (product.ProductName.Length<2)
            {
                return new ErrorResult(Messages.ProductNameInvalid);
            }
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
