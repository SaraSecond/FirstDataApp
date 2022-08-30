using EFDataApp.Domain.Entities;
using EFDataApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.Abstract
{
    public interface IProductRepository
    {

        IQueryable<Product> GetProducts();
        ProductPage GetProducts(ProductOptions options);
        Product GetProduct(int? id);
        void SaveProduct(Product prod);
        void DeleteProduct(int? id);
        void ClearBase();
    }
}
