using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.Abstract
{
    public interface IWebServiceRepository
    {
        object GetProducts(int id);
    }
}
