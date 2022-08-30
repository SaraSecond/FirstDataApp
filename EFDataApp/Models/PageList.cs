using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataApp.Infrastructure.ExtensionsMethods;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFDataApp.Models
{
    public class QueryOptions
    {
        public string ControllerName { get; set; }

        public string[] Properties { get; set; }

        public string SortPropertyName { get; set; } = "Id"; // Устанавливаем значение по умолчанию для свойства сортировки по умолчанию
        public bool DecsendingOrder { get; set; }


        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        

        public QueryOptions Options { get; set; }


        public PageList(QueryOptions options)
        {
            Options = options;
            CurrentPage = options.CurrentPage;
            PageSize = options.PageSize;     
        }

        // Методу передаётся очередь отфильтрованных или неотфильтрованных данных, а в теле самого метола уже создаетя range на основе номера и вида сортировки
        // Далее этот окончательный range для отобрадения в представлении передаётся уже методу класса наследкика AddRange для заполнения данными наследуемого класса PageList
        public void AddRangePage(IQueryable<T> query)
        {
            if (Options != null)
            {
                if (!String.IsNullOrEmpty(Options.SortPropertyName))
                {
                    query = query.OrderBy(Options.SortPropertyName, Options.DecsendingOrder);
                }
            }
            TotalPages = (int)Math.Ceiling(query.Count() / (decimal)PageSize);
            if(CurrentPage > TotalPages && TotalPages != 0)
            {
                CurrentPage = TotalPages;
            }
            AddRange(query.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
        }

        public bool HasPreviousPage { get { return (CurrentPage > 1); } }
        public bool HasNextPage { get { return (CurrentPage < TotalPages); } }
    }
}
