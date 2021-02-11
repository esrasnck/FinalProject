﻿using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        // IResult dersen datayı döndüremez. IResult a data versen, SOLİD'in ilk prensibine ters. o yüzden, geri birşey döndürürsek, DataResult olusturcaz gibi
        IDataResult<List<Product>> GetAll();
        IDataResult<Product> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IResult Add(Product product);
        IDataResult<Product> GetByID(int productId);
    }
}
