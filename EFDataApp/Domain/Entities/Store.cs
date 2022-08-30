using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Entities
{
    public class Store
    {
        
        public int Id { get; set; }
        public string Location { get; set; }
        public List<Product> Products { get; set; } = new();

        //public Store()
        //{
        //    Products = new List<Product>();
        //}
    }
}
