using EFDataApp.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.TagHelpers
{
    public class RefTagHelper : TagHelper
    {
        public IndexViewModel IndexModel { get; set; }
        public string SortOrder { get; set; }
        public string PagePosition { get; set; }
        public string TableView { get; set; }
        public string Name { get; set; } = "No Name";
        public string RefClass { get; set; } = "page-link";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SortState sortOrder;

            switch (SortOrder)
            {
                case "name": 
                    sortOrder = IndexModel.SortViewModel.NameSort; 
                    break;
                case "manufacturer": 
                    sortOrder = IndexModel.SortViewModel.ManufacturerSort;
                    break;
                case "price": 
                    sortOrder = IndexModel.SortViewModel.PriceSort;
                    break;
                case "store":
                    sortOrder = IndexModel.SortViewModel.StoreSotr;
                    break;
                case "count": 
                    sortOrder = IndexModel.SortViewModel.CountSort;
                    break;
                default: 
                    sortOrder = IndexModel.SortViewModel.Current;
                    break;
            }

            int pagePos;
            switch (PagePosition)
            {
                case "back":
                    pagePos = IndexModel.PageViewModel.PageNumber - 1;
                    break;
                case "first":
                    pagePos = 1;
                    break;
                case "next":
                    pagePos = IndexModel.PageViewModel.PageNumber + 1;
                    break;
                case "last":
                    pagePos = IndexModel.PageViewModel.TotalPage;
                    break;
                case "current":
                    pagePos = IndexModel.PageViewModel.PageNumber;
                    break;
                default:
                    pagePos = IndexModel.PageViewModel.PageNumber;
                    break;
            }


            output.TagName = "form";
            output.Attributes.SetAttribute("method", "get");

            string listContent = @$"<input type=""hidden"" name=""FilterViewModel.Options.SelectedName"" value=""{IndexModel.FilterViewModel.Options.SelectedName}""/>"
                + $@"<input type=""hidden"" name=""FilterViewModel.Options.SelectedManufacturer"" value=""{IndexModel.FilterViewModel.Options.SelectedManufacturer}"" />"
                + $@"<input type=""hidden"" name=""FilterViewModel.Options.SelectedStore"" value=""{IndexModel.FilterViewModel.Options.SelectedStore}"" />"
                + $@"<input type=""hidden"" name=""FilterViewModel.Options.SelectedMinPrice"" value=""{IndexModel.FilterViewModel.Options.SelectedMinPrice}"" />"
                + $@"<input type=""hidden"" name=""FilterViewModel.Options.SelectedMaxPrice"" value=""{IndexModel.FilterViewModel.Options.SelectedMaxPrice}"" />"
                + $@"<input type=""hidden"" name=""FilterViewModel.Options.NotZero"" value=""{IndexModel.FilterViewModel.Options.NotZero}"" />"
                + $@"<input type=""hidden"" name=""TableView"" value=""{TableView}"" />"
                + $@"<input type=""hidden"" name=""PageViewModel.PageNumber"" value=""{pagePos}"" />"
                + $@"<input type=""hidden"" name=""SortViewModel.Current"" value=""{sortOrder}""/>"
                + $@"<input type=""submit"" value=""{Name}"" class=""{RefClass}"" />";

            output.Content.SetHtmlContent(listContent);
        }
    }

    
}
