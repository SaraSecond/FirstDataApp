using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public bool Shipped { get; set; }
        public IEnumerable<OrderLine> Lines { get; set; }
    }
}
