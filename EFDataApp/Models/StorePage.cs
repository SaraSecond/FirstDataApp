using EFDataApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class StoreOptions : QueryOptions
    {
        public string SearchTerm { get; set; }
        public bool? Switch { get; set; } = null;
    }
    public class StorePage : PageList<Store>
    {
        public IQueryable<Store> Stores { get; set; }
        public StoreOptions Options { get; set; }

        public StorePage(IQueryable<Store> stores, StoreOptions options) : base(options)
        {
            Options = options;
            Options.ControllerName = "Store";
            Options.Properties = options?.Properties ?? new string[] { "Id", "Location", "Products.Count" };

            if(options != null)
            {
                if(!String.IsNullOrEmpty(options.SearchTerm))
                {
                    stores = stores.Where(s => s.Location.Contains(options.SearchTerm));
                }

                stores = options.Switch switch
                {
                    null => stores.Where(s => s.Products.Count >= 0),
                    true => stores.Where(s => s.Products.Count != 0),
                    false => stores.Where(s => s.Products.Count == 0)
                };
            }
            AddRangePage(stores); 
        }
    }
}
