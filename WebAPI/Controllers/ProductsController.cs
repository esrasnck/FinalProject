using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Entities.Dtos;
using System.Threading;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        // ınterface'in elinde somut referans yok. bunun için IoC yapısı gerek -- inversion of control => değişimin kontrolü
        public ProductsController(IProductService productService)  // gevşek bağımlılık. Loosely coupled. bir bağımlılık var. ama soyuta  
        {                                               //bağımlı.servis değişirse manager değişirsei herhangi bir problemle karşılaşmayacağız.
            _productService = productService;
        }

        // Bazı projelerde managerlar çoğul hale gelebilir. hiç bir zaman bir katman diğer katmanın somutunu interface olmayanlar dışında bağlantı kuramazsın. 

        [HttpGet]

        public IActionResult GetAll()
        {
            // dependency chain (bağımlılık zinciri. Iproduct servce productmanager' o da ProductDal'a bağımlı. bu bağımlılığın çözülmesi için constructor injection yapılması gerek
           // Thread.Sleep(1000);

            // User.ClaimRoles();  //=> using Core.Extensions; buradan gelecek

            IDataResult<List<Product>> result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // naming Convention => isimlendirme standartı.

        // çözümlemek, interface'e bağlı olan sınıfı newlemek demek.

        [HttpPost]
        public IActionResult Add(Product product)
        {
            IResult result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            IDataResult<Product> result = _productService.GetByID(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet]
        public IActionResult GetByCategory(int categoryId)
        {
            var result = _productService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public IActionResult TransactionTest(Product product)
        {
            IResult result = _productService.TransactionalOperation(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public IActionResult GetProductDetails()
        {
            IDataResult<List<ProductDetailDto>> result = _productService.GetProductDetails();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}

