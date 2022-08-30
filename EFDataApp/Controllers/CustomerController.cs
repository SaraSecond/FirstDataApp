using EFDataApp.Domain;
using EFDataApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Controllers
{
    public class CustomerController : Controller
    {
        private DataManager data;
        public CustomerController(DataManager data)
        {
            this.data = data;
        }
        public IActionResult Index(ProductOptions options)
        {
            return View(data.Products.GetProducts(options));
        }
    }
}
