using EFDataApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.Abstract
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrder(int key);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);

    }
}
