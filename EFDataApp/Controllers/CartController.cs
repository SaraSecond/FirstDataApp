using EFDataApp.Domain;
using EFDataApp.Domain.Entities;
using EFDataApp.Infrastructure.ExtensionsMethods;
using EFDataApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Controllers
{
    public class CartController : Controller
    {
        private DataManager data;
        public CartController(DataManager data)
        {
            this.data = data;
        }
        public IActionResult Index(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(GetCart());
        }

        [HttpPost]
        public IActionResult AddToCart(Product prod, string returnUrl)
        {
            SaveCard(GetCart().AddItem(prod, 1));
            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId, string returnUrl)
        {
            SaveCard(GetCart().RemoveItem(productId));
            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        public IActionResult CreateOrder(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order, string[] quantity)
        {
            order.Lines = GetCart().Selections.Select(s => new OrderLine
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity
            }).ToArray();

            data.Orders.AddOrder(order);
            SaveCard(new Cart());

            return RedirectToAction(nameof(Completed));
        }

        public IActionResult Completed() => View();

        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        }

        private void SaveCard(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}
