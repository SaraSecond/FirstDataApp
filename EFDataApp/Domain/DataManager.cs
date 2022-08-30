using EFDataApp.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain
{
    public class DataManager
    {
        public IProductRepository Products { get; set; }
        public IStoreRepository Stores { get; set; }
        public IOrdersRepository Orders { get; set; }

        public DataManager(IProductRepository products, IStoreRepository stores, IOrdersRepository orders)
        {
            Products = products;
            Stores = stores;
            Orders = orders;
        }
    }
}
