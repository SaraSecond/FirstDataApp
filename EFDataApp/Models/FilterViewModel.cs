using EFDataApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class FilterOptions
    {
        public string SelectedManufacturer { get; set; }
        public string SelectedName { get; set; }
        public int? SelectedStore { get; set; }
        public int SelectedMaxPrice { get; set; }
        public int SelectedMinPrice { get; set; }
        public bool NotZero { get; set; }
    }
    public class FilterViewModel
    {
        public SelectList Products { get; set; }
        public SelectList Stores { get; set; }
        public FilterOptions Options { get; set; }
        

        public FilterViewModel()
        {

        }

        public FilterViewModel(FilterOptions options)
        {
            Options = options;
        }

        public FilterViewModel(List<Product> products, List<Store> stores, FilterOptions options)
        {
            products.Insert(0, new Product { Manufacturer = "Все"});
            products = products.GroupBy(x => x.Manufacturer).Select(x => x.First()).ToList();

            stores.Insert(0, new Store { Location = "Все", Id = 0 });
            Stores = new SelectList(stores, "Id", "Location", options.SelectedStore);

            Products = new SelectList(products, "Manufacturer", "Manufacturer", options.SelectedManufacturer);

            Options = options;  
        }

        //private static IQueryable<string> Search(IQueryable<string> query, string propertyName, string serchTerm)
        //{
        //    ParameterExpression parameter = Expression.Parameter(typeof(string), "x");
        //    Expression source = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
        //    MethodCallExpression body = Expression.Call(source, "Contains", Type.EmptyTypes, Expression.Constant(serchTerm, typeof(string)));
        //    Expression<Func<string, bool>> lambda = Expression.Lambda<Func<string, bool>>(body, parameter);
        //    return query.Where(lambda);
        //}


        //private static IQueryable<string> Order(IQueryable<string> query, string propertyName, bool desc)
        //{
        //    ParameterExpression parameter = Expression.Parameter(typeof(string), "x");
        //    Expression source = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
        //    var lambda = Expression.Lambda(typeof(Func<,>).MakeGenericType(typeof(string), source.Type), source, parameter);
        //    return typeof(Queryable)
        //        .GetMethods()
        //        .Single(method => method.Name == (desc ? "OrderByDescending" : "OrderBy") && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2)
        //        .MakeGenericMethod(typeof(string), source.Type)
        //        .Invoke(null, new object[] { query, lambda })
        //        as IQueryable<string>;
        //}

    }
}
