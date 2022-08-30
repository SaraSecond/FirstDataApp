using EFDataApp.Domain.Entities;
using EFDataApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.Abstract
{
    public interface IStoreRepository
    {
        IQueryable<Store> GetStores();
        public StorePage GetStores(StoreOptions options);
        Store GetStore(int id);
        void SaveStore(Store store);
        void DeleteStore(int? id);
    }
}
