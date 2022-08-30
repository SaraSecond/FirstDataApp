using EFDataApp.Domain.Entities;
using EFDataApp.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.EntityFramework
{
    public class EFOrdersRepository : IOrdersRepository
    {
        private readonly ApplicationContext context;
        public EFOrdersRepository(ApplicationContext appContext)
        {
            context = appContext;
        }
        public IEnumerable<Order> GetOrders() => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);   

        public void AddOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public Order GetOrder(int key)
        {
            return context.Orders.Include(o => o.Lines).FirstOrDefault(o => o.Id == key);
        }

        public void UpdateOrder(Order order)
        {
            context.Orders.Update(order);
            context.SaveChanges();
        }
    }
}
