using EFDataApp.Domain.Entities;
using EFDataApp.Domain.Repositories.Abstract;
using EFDataApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Domain.Repositories.EntityFramework
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationContext context;
        private readonly IWebHostEnvironment hostingEnvironment;
        public EFProductRepository(ApplicationContext appContext, IWebHostEnvironment hostingEnvironment)
        {
            context = appContext;
            this.hostingEnvironment = hostingEnvironment;
        }

        public void DeleteProduct(int? id)
        {
            if(id != null)
            {
                Product prod = context.Products.Include(s => s.Store).FirstOrDefault(p => p.Id == id);
                if (prod != null)
                {
                    context.Products.Remove(prod);
                    context.SaveChanges();
                }  
            }
            
        }

        public Product GetProduct(int? id)
        {
            return context.Products.Include(x => x.Store).FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Product> GetProducts()
        {
            return context.Products.Include(x => x.Store);
        }

        public ProductPage GetProducts(ProductOptions options)
        {
            return new ProductPage(context.Products.Include(s => s.Store), context.Stores, options);
        }

        public void SaveProduct(Product prod)
        {
            if (prod.Id == default)
            {
                context.Products.Add(prod);
            }
            else
            {
                context.Products.Update(prod);
            } 
            context.SaveChanges();
        }

        public void ClearBase()
        {
            foreach(Product prod in context.Products)
            {
                if (prod.ImagePath != "/images/DefaultImg.jpg")
                {
                    FileInfo img = new FileInfo(hostingEnvironment.WebRootPath + prod.ImagePath);
                    if (img.Exists)
                    {
                        img.Delete();
                    }
                }
            }
            
            context.Database.EnsureDeleted();
        }
    }
}
