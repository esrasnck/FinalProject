using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IProductService productManager = new ProductManager(new EfProductDal());

            foreach (Product product in productManager.GetAll())
            {
                Console.WriteLine(product.ProductName);
            }

           
        }
    }
}
