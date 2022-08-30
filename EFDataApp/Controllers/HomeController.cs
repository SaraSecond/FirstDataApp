using EFDataApp.Domain;
using EFDataApp.Domain.Entities;
using EFDataApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EFDataApp.Infrastructure.ExtensionsMethods;

namespace EFDataApp.Controllers
{
    public class HomeController : Controller
    { 
        private DataManager data;
        private readonly IWebHostEnvironment hostingEnvironment;
        public HomeController(DataManager data, IWebHostEnvironment hostEnvironment)
        {
            this.data = data;
            hostingEnvironment = hostEnvironment;


            //if (data.Products.GetProducts().Count() == 0)
            //{
            //    Store store1 = new Store { Location = "Odessa" };
            //    Store store2 = new Store { Location = "Kotovsk" };
            //    Store store3 = new Store { Location = "Red Okna" };

            //    Product prod1 = new Product { Name = "iPhone 11", Manufacturer = "Apple", Price = 18000, Count = 10, ImagePath = "/images/001.jpg", Store = store1 };
            //    Product prod2 = new Product { Name = "iPhone 12", Manufacturer = "Apple", Price = 30000, Count = 15, ImagePath = "/images/002.jpg", Store = store2 };
            //    Product prod3 = new Product { Name = "iPhone 13", Manufacturer = "Apple", Price = 45000, Count = 17, ImagePath = "/images/003.jpg", Store = store3 };
            //    Product prod4 = new Product { Name = "Samsung Galaxy 11X", Manufacturer = "Samsung", Price = 25000, Count = 9, ImagePath = "/images/004.jpg", Store = store3 };

            //    db.Stores.AddRange(store1, store2, store3);
            //    db.Products.AddRange(prod1, prod2, prod3, prod4);
            //    db.SaveChanges();
            //}


        }

        public IActionResult Index(ProductOptions options)
        {
            ViewBag.Cols = new string[] { "Id", "Name", "Manufacturer", "Price", "Count", "Store.Location" };
            return View(data.Products.GetProducts(options));
        }

        //public IActionResult OldIndex(IndexViewModel model, string SearchName)
        //{

        //    IQueryable<Product> products = data.Products.GetProducts().Include(x => x.Store);
        //    //IQueryable<Product> products = data.Products.Include(x => x.Store);

        //    int pageSize = 15;
        //    string name = SearchName ?? model.FilterViewModel?.Options.SelectedName;
        //    string manufacturer = model.FilterViewModel?.Options.SelectedManufacturer;
        //    int? store = model.FilterViewModel?.Options.SelectedStore;
        //    decimal maxPrice = 0;
        //    decimal minPrice = 0;
        //    if (products.Count() != 0)
        //    {
        //        maxPrice = model.FilterViewModel?.Options.SelectedMaxPrice ?? products.Max(p => p.Price);
        //        minPrice = model.FilterViewModel?.Options.SelectedMinPrice ?? products.Min(p => p.Price);
        //    }
        //    int page = model.PageViewModel?.PageNumber ?? 1;
        //    SortState sortOrder = model.SortViewModel?.Current ?? SortState.NameAsc;
        //    bool notZero = model.FilterViewModel?.Options.NotZero ?? false;
        //    bool tableView = model?.TableView ?? false;


        //    if (!String.IsNullOrEmpty(name))
        //    {
        //        //products = products.Where(p => EF.Functions.Like(p.Name!, $"%{name}%"));
        //        products = products.Where("Name", name);
        //    }
        //    if (!String.IsNullOrEmpty(manufacturer) && manufacturer != "Все")
        //    {
        //        products = products.Where("Manufacturer", manufacturer);
        //    }
        //    if (store != null && store != 0)
        //    {
        //        products = products.Where(p => p.StoreId == store);
        //    }

        //    //products = products.Where(p => p.Price <= maxPrice && p.Price >= minPrice);

        //    products = products.Where("Price", minPrice, maxPrice);

        //    products = sortOrder switch
        //    {
        //        SortState.NameDesc => products.OrderBy("Name", true),
        //        SortState.PriceAsc => products.OrderBy(s => s.Price),
        //        SortState.PriceDesc => products.OrderByDescending(s => s.Price),
        //        SortState.ManufacturerAsc => products.OrderBy(s => s.Manufacturer),
        //        SortState.ManufacturerDesc => products.OrderByDescending(s => s.Manufacturer),
        //        SortState.StoreAsc => products.OrderBy("Store.Location", false),
        //        SortState.StoreDesc => products.OrderByDescending(s => s.Store.Location),
        //        SortState.CountAsc => products.OrderBy(s => s.Count),
        //        SortState.CountDesc => products.OrderByDescending(s => s.Count),
        //        _ => products.OrderBy("Name", false),
        //    };

        //    if (notZero == true)
        //    {
        //        products = products.Where(p => p.Count != 0);
        //    }

        //    FilterOptions options = new FilterOptions()
        //    {
        //        SelectedManufacturer = manufacturer,
        //        SelectedName = name,
        //        SelectedStore = store,
        //        SelectedMaxPrice = (int)maxPrice,
        //        SelectedMinPrice = (int)minPrice,
        //        NotZero = notZero
        //    };

        //    int count = products.Count();
        //    List<Product> items = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    IndexViewModel viewModel = new IndexViewModel
        //    {
        //        PageViewModel = new PageViewModel(count, page, pageSize),
        //        SortViewModel = new SortViewModel(sortOrder),
        //        FilterViewModel = new FilterViewModel(data.Products.GetProducts().ToList(), data.Stores.GetStores().ToList(), options),
        //        //FilterViewModel = new FilterViewModel(data.Products.ToList(), data.Stores.ToList(), store, manufacturer, name, (int)maxPrice, (int)minPrice, notZero),
        //        Products = items,
        //        TableView = tableView
        //    };

        //    return View(viewModel);
        //}

        public IActionResult Create()
        {

            ViewBag.Stores = data.Stores.GetStores().ToList();
            //ViewBag.Stores = data.Stores.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product prod, IFormFile imagePath)
        {
            if (imagePath != null)
            {
                prod.ImagePath = "/images/" + imagePath.FileName;

                using (FileStream stream = new FileStream(hostingEnvironment.WebRootPath + prod.ImagePath, FileMode.Create))
                {
                    imagePath.CopyTo(stream);
                }
            }
            else
            {
                prod.ImagePath = "/images/DefaultImg.jpg";
            }

            data.Products.SaveProduct(prod);
            //data.Products.Add(prod);
            //data.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (id != null)
            {
                Product prod = data.Products.GetProduct(id);
                if (prod != null)
                    return View(prod);
            }
            //if(!String.IsNullOrEmpty(name))
            //{
            //    IQueryable<Product> products = data.Products.GetProducts().Where(p => EF.Functions.Like(p.Name!, $"%{name}%"));
            //    //IQueryable<Product> products = data.Products.Where(p => EF.Functions.Like(p.Name!, $"%{name}%"));
            //    if (products.Count() == 1)
            //    {
            //        Product prod = products.First();
            //        if (prod != null)
            //            return View(prod);
            //    }
            //    if(products.Count() > 1)
            //    {
            //        return RedirectToAction("Index", new { model = new IndexViewModel { }, searchName = name});
            //    }
                
            //}
            return NotFound();
        }

        

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Stores = data.Stores.GetStores().ToList();
            if (id != null)
            {
                Product prod = data.Products.GetProduct(id);
                //Product prod = data.Products.Include(x => x.Store).FirstOrDefault(p => p.Id == id);
                if (prod != null)
                    return View(prod);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product prod, IFormFile imagePath)
        {
            if (imagePath != null)
            {
                if (prod.ImagePath != "/images/DefaultImg.jpg")
                {
                    FileInfo img = new FileInfo(hostingEnvironment.WebRootPath + prod.ImagePath);
                    if (img.Exists)
                    {
                        img.Delete();
                    }
                }
                prod.ImagePath = "/images/" + imagePath.FileName;
                using (FileStream stream = new FileStream(hostingEnvironment.WebRootPath + prod.ImagePath, FileMode.Create))
                {
                    imagePath.CopyTo(stream);
                }
            }
            
            data.Products.SaveProduct(prod);
            //data.Products.Update(prod);
            //data.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Product user = data.Products.GetProduct(id);
                //Product user = data.Products.FirstOrDefault(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Product prod = data.Products.GetProduct(id);
                //Product prod = data.Products.FirstOrDefault(p => p.Id == id);
                if (prod != null)
                {
                    if(prod.ImagePath != "/images/DefaultImg.jpg")
                    {
                        FileInfo img = new FileInfo(hostingEnvironment.WebRootPath + prod.ImagePath);
                        if (img.Exists)
                        {
                            img.Delete();
                        }
                    }

                    data.Products.DeleteProduct(prod.Id);
                    //data.Products.Remove(prod);
                    //data.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        public IActionResult CreateSeedData(int count)
        {
            Random random = new Random();
            for(int i = 0; i <= count; i++)
            {
                Product prod = new Product()
                {
                    Name = $"Product_{i}",
                    Manufacturer = $"Manufacturer_{i % 10}",
                    Price = random.Next(2, 200) * 500,
                    Count = random.Next(0, 200),
                    Store = data.Stores.GetStore((i % 3) + 1),
                    ImagePath = "/images/DefaultImg.jpg"
                };
                data.Products.SaveProduct(prod);
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAllDate()
        {
            data.Products.ClearBase();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
