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
using Business.CCS;
using System.Linq;
using Core.Utilities.Business;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        // bir iş sınıfı, başka sınıfları newlemez

        IProductDal _productDal;
        //  ICategoryDal _categoryDal; Yanlış. Bir entity manager, kendisi hariç başka DaL'ı enjekte edemez. Ama servis enjekte ederiz. :D
        // herkes kendi metotlarını/ kurallarını yazar. bu yüzden bir ayrı servis yapıyoruz.

        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService; // bu helal. haram değil. Mesela sistemin içerisne e-devlet microservisi enjekte etmek istersek, bu şekilde yazacaz.
        }

        public IDataResult<List<Product>> GetAll()
        {
            // iş kodları => iş kodlarından geçiyorsa, benim veri erişimi çağrımam gerek. bu safha da dependency injection olaya giriyor.

            if (DateTime.Now.Hour == 22) // 22 de sistemi kapamak istiyoruz. ürün list istemiyoz
            {
                // boş ürün döndürmece
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetByID(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
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
                return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
            }
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
           
            // attribute dan ötürü gidecek :) ValidationTool.Validate(new ProductValidator(), product);// değişen iki şey bu. diğerleri aynı kalıyor.
         

            #region Sorular.
            // 1) bir kategoride en fazla 10 ürün olabilir...
            // 2) aynı isimde ürün eklenemez.
            // 3) Eğer mevcut kategori sayısı 15'i geçtiyse, sisteme yeni ürün eklenemez.
            #endregion


            // başka bir kural geldiyse, kural geldi deyip eklemek yeterli. 

            IResult result =  BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfProductNameExists(product.ProductName),CheckIfCategoryLimitExceded());

            // utilities deki business => businessRules kısmındaki static metodu çağırdık. Buradan bana tek bir result dönecek. bunu resulta atacaz. bu ya boştur. ya nulldır yada içi doludur. dolu ise error resulttır. 
             
            if (result!= null) // error result carsa bana error resutl ver demektir.
            {
                return result;
            }
            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);

            #region Çirkin kodlar. üstteki ile aynı. 
           /* if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success) // iç içe if ile daha kontrollü olurmuş. && dan daha çokomelliymiş.
            {
                if (CheckIfProductNameExists(product.ProductName).Success)
                {
                    _productDal.Add(product);

                    return new SuccessResult(Messages.ProductAdded);
                }
               
            };

            return new ErrorResult(); */
            #endregion
        }


        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success )
            {
                _productDal.Update(product);

                return new SuccessResult(Messages.ProductAdded);
            };

            return new ErrorResult();
         }


        // bu iş kodunu metot haline getirdik.
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        { 

            // getAll demek : select (*) from where categoryId=1 demek gibi.
            int result = _productDal.GetAll(c => c.CategoryId == categoryId).Count;

            if (result >= 0)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
               
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            bool result = _productDal.GetAll(c => c.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded() // category de bunu yazıyorsak, bu başlı başına bir servis.
        { 
            // product için kategori servisi nasıl yorumlanıyor? o yüzden buraya yazıyorum.

            IDataResult<List<Category>> result = _categoryService.GetAll();
            if (result.Data.Count> 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
