using EFDataApp.Domain;
using EFDataApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Controllers
{
    public class OrdersController : Controller
    {
        private DataManager data;
        public OrdersController(DataManager data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            return View(data.Orders.GetOrders());
        }

        public IActionResult EditOrder(int id)
        {
            var products = data.Products.GetProducts();
            Order order = id == 0 ? new Order() : data.Orders.GetOrder(id);
            IDictionary<int, OrderLine> linesMap = order.Lines?.ToDictionary(l => l.ProductId) 
                ?? new Dictionary<int, OrderLine>();
            ViewBag.Lines = products.Select(p => linesMap.ContainsKey(p.Id) 
                ? linesMap[p.Id] : new OrderLine { Product = p, ProductId = p.Id, Quantity = 0});
            return View(order);
        }

        [HttpPost]
        public IActionResult AddOrUpdateOrder(Order order)
        {
            order.Lines = order.Lines.Where(l => l.Id > 0 || (l.Id == 0 && l.Quantity > 0)).ToArray();
            if(order.Id == 0)
            {
                data.Orders.AddOrder(order);
            }
            else
            {
                data.Orders.UpdateOrder(order);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteOrder(Order order)
        {
            data.Orders.DeleteOrder(order);
            return RedirectToAction(nameof(Index));
        }
    }
}
