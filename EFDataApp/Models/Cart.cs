using EFDataApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class Cart
    {
        private List<OrderLine> selections = new List<OrderLine>();

        public Cart AddItem(Product prod, int quantity)
        {
            OrderLine line = selections.Where(l => l.ProductId == prod.Id).FirstOrDefault();

            if(line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                selections.Add(new OrderLine
                {
                    ProductId = prod.Id,
                    Product = prod,
                    Quantity = quantity
                });
            }

            return this;
        }

        public Cart RemoveItem(int prodId)
        {
            selections.RemoveAll(l => l.ProductId == prodId);
            return this;
        }

        public void Clear() => selections.Clear();
        public IEnumerable<OrderLine> Selections { get => selections; }
    }
}
