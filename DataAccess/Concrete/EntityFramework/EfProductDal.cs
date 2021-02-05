using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : IProductDal
    {

        public void Add(Product entity)
        {
            // IDisposable pattern implementation of c#

           using(NorthwindContext context= new NorthwindContext())
            {
                var addedEntity = context.Entry(entity);  // referenası yakalama
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
               
            };
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var deletedEntity = context.Entry(entity);  // referenası yakalama
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();

            };
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var updatedEntity = context.Entry(entity);  // referenası yakalama
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();

            };
        }
        public Product Get(Expression<Func<Product, bool>> filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context.Set<Product>().SingleOrDefault(filter);
            }
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            using (NorthwindContext context= new NorthwindContext())
            {
                #region if version
                if (filter == null)
                {
                    return context.Set<Product>().ToList();
                }
                else
                {
                    return context.Set<Product>().Where(filter).ToList();
                }  //todo: sonradan dön bak!!!
                #endregion

                /*return filter == null ? context.Set<Product>().ToList() : context.Set<Product>().Where(filter).ToList()*/;
            }
        }

    
    }
}
