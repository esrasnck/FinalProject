using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;  // constructor injection ile bağımlılığımı dahil ediyorum !!!

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public IDataResult<List<Category>> GetAll()
        {
            // iş kodları.
            return new SuccessDataResult<List<Category>> (_categoryDal.GetAll());
        }

        public IDataResult<Category> GetById(int CategoryId)
        {
            return new SuccessDataResult<Category> (_categoryDal.Get(c => c.CategoryId == CategoryId));
        }
    }
}
