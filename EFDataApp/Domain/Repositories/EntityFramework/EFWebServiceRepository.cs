using EFDataApp.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.EntityFramework
{
    public class EFWebServiceRepository : IWebServiceRepository

    {
        private readonly ApplicationContext context;
        public EFWebServiceRepository(ApplicationContext appContext)
        {
            context = appContext;
        }
        public object GetProducts(int id)
        {
            return context.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
