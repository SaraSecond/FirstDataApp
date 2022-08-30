using EFDataApp.Domain.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Controllers
{
    [Route("api/products")]
    public class ProductValuesController : ControllerBase
    {
        private IWebServiceRepository repository;
        public ProductValuesController(IWebServiceRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("id")]
        public object GetProduct(int id)
        {
            return repository.GetProducts(id) ?? NotFound();
        }
    }
}
