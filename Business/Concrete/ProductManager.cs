using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
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

        public List<Product> GetAll()
        {
            // iş kodları => iş kodlarından geçiyorsa, benim veri erişimi çağrımam gerek. bu safha da dependency injection olaya giriyor.
            return _productDal.GetAll();
        }
    }
}
