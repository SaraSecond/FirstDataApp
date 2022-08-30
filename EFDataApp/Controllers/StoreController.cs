using EFDataApp.Domain;
using EFDataApp.Domain.Entities;
using EFDataApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Controllers
{
    public class StoreController : Controller
    {
        private DataManager data;
        public StoreController(DataManager data)
        {
            this.data = data;
        }

        public IActionResult Index(StoreOptions options)
        {
            ViewBag.Cols = new string[] {"Id", "Location", "Products.Count" };
            return View(data.Stores.GetStores(options));
        }

        public IActionResult Store(string name, string selectNotZero, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Store> stores = data.Stores.GetStores().Include(s => s.Products);

            //string name = model.FilterViewModel?.SelectedName;
            //string selectNotZero = model.FilterViewModel?.SelectedManufacturer;
            //SortState sortOrder = model.SortViewModel?.Current ?? SortState.NameAsc;


            if (!String.IsNullOrEmpty(name))
            {
                stores = data.Stores.GetStores().Where(s => EF.Functions.Like(s.Location!, $"%{name}%"));
            }
            if (!String.IsNullOrEmpty(selectNotZero))
            {
                stores = selectNotZero switch
                {
                    "С товарами" => stores.Where(s => s.Products.Count != 0),
                    "Пустые" => stores.Where(s => s.Products.Count == 0),
                    _ => stores
                };
            }

            stores = sortOrder switch
            {
                SortState.NameDesc => stores.OrderByDescending(s => s.Location),
                SortState.CountAsc => stores.OrderBy(s => s.Products.Count),
                SortState.CountDesc => stores.OrderByDescending(s => s.Products.Count),
                _ => stores.OrderBy(s => s.Location),
            };

            FilterOptions options = new FilterOptions
            {
                SelectedName = name,
                SelectedManufacturer = selectNotZero
            };


            IndexViewModel viewModel = new IndexViewModel
            {
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(options),
                Stores = stores
                .Include(s => s.Products)
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            data.Stores.SaveStore(store);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Store store = await data.Stores.GetStores().Include(s => s.Products).FirstOrDefaultAsync(p => p.Id == id);
                if (store != null)
                    return View(store);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Store store = await data.Stores.GetStores().Include(s => s.Products).FirstOrDefaultAsync(p => p.Id == id);
                if (store != null && store.Products.Count == 0)
                {
                    data.Stores.DeleteStore(id);
                    return RedirectToAction("Store");
                }
                return View(store);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Store store = await data.Stores.GetStores().FirstOrDefaultAsync(p => p.Id == id);
                if (store != null)
                    return View(store);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Store store)
        {
            data.Stores.SaveStore(store);
            return RedirectToAction("Store");
        }
    }

    
}
