using EFDataApp.Domain.Entities;
using EFDataApp.Domain.Repositories.Abstract;
using EFDataApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.EntityFramework
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly ApplicationContext context;
        public EFStoreRepository(ApplicationContext appContext)
        {
            context = appContext;
        }
        public void DeleteStore(int? id)
        {
            if(id != null)
            {
                Store store = context.Stores.Include(s => s.Products).FirstOrDefault(p => p.Id == id);
                if(store != null)
                {
                    context.Stores.Remove(store);
                    context.SaveChanges();
                }
            } 
        }

        public Store GetStore(int id)
        {
            return context.Stores.FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Store> GetStores()
        {
            return context.Stores.Include(x => x.Products);
        }

        public StorePage GetStores(StoreOptions options)
        {
            return new StorePage(context.Stores.Include(s => s.Products), options);
        }

        public void SaveStore(Store store)
        {
            if (store.Id == default)
            {
                context.Stores.Add(store);
            }
            else
            {
                context.Stores.Update(store);
            }
            context.SaveChanges();
        }
    }
}
