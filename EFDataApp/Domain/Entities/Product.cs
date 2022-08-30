using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public string ImagePath { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
