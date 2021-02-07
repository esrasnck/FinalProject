using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    // generic constraint = generic kısıt
    // class= referans tip olabilir demek miş
    // new(): newlenebilir olmalı. IEntity newlenemeyeceği için, IEntity'i koyamayız. ama onun referansını tuttuğu sınıfları koyabiliriz.
   public interface IEntityRepository<T> where T: class, IEntity, new()
    {
        // veritabanını ilgilendiren herhangi bir yapı yok !
        List<T> GetAll(Expression<Func<T,bool>> filter =null); // filtre vermeyebilirsin.
        T Get(Expression<Func<T, bool>> filter); // filtre zorunlu
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
