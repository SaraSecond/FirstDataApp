using EFDataApp.Domain.Entities;
using EFDataApp.Infrastructure.ExtensionsMethods;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class ProductOptions : QueryOptions
    {
        public string[] SearchPropertyNames { get; set; } = { "Name", "Manufacturer", "Store.Location" };
        public string[] SearchTerms { get; set; } = Array.Empty<string>();


        public int? MinCount { get; set; }
        public int? MaxCount { get; set; }


        public bool Switch { get; set; }
    }


    public class ProductPage : PageList<Product>
    {
        public IQueryable<Store> Stores { get; set; }
        public ProductOptions Options { get; set; }
        public ProductPage(IQueryable<Product> query, IQueryable<Store> stores, ProductOptions options) : base(options)
        {
            Options = options;
            Stores = stores;
            Options.ControllerName = "Home";
            Options.Properties = options?.Properties ?? new string[] { "Name", "Manufacturer", "Price", "Count", "Store.Location" }; // Второе условное выражение устанавливает массив названий стобцов отображаемых по умолчанию
            Options.MinCount = options?.MinCount ?? (int)query.Min(p => p.Price);
            Options.MaxCount = options?.MaxCount ?? (int)query.Max(p => p.Price);

            if (options != null) // Тело фильтрации
            {
                if (options.SearchPropertyNames.Count() != 0 && options.SearchTerms.Count() != 0)
                {
                    for (int i = 0; i < options.SearchPropertyNames.Count() && i < options.SearchTerms.Count(); i++)
                    {
                        if (options.SearchTerms[i] != null)
                        {
                            query = query.Where(options.SearchPropertyNames[i], options.SearchTerms[i].Trim());
                        }
                    }
                }
  
                query = query.Where(p => p.Price >= Options.MinCount && p.Price <= Options.MaxCount);
                    
                if (options.Switch)
                {
                    query = query.Where(p => p.Count != 0);
                }
            }
            AddRangePage(query); 
        }
    }
}
