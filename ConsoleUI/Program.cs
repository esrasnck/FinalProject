﻿using Business.Abstract;
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
            ProductManager productManager = new ProductManager(new EfProductDal());

            //GetAll(productManager);
            //GetByCatId(productManager);
            //GetByUnitPrice(productManager);
            // CategoryTest();

            foreach (var item in productManager.GetProductDetails())
            {
                Console.WriteLine($"{item.ProductName} Category {item.CategoryName}");
            }
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (Category item in categoryManager.GetAll())
            {
                Console.WriteLine(item.CategoryName);
            }
        }

        private static void GetByUnitPrice(ProductManager productManager)
        {
            foreach (Product product in productManager.GetByUnitPrice(50, 100))
            {
                Console.WriteLine(product.ProductName);
            }
        }

        private static void GetByCatId(ProductManager productManager)
        {
            foreach (Product product in productManager.GetAllByCategoryId(2))
            {
                Console.WriteLine(product.ProductName);
            }

            Console.WriteLine("--------------------------------------------------");
        }

        private static void GetAll(ProductManager productManager)
        {
            foreach (Product product in productManager.GetAll())
            {
                Console.WriteLine(product.ProductName);
            }
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
