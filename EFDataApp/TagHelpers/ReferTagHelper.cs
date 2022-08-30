using EFDataApp.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.TagHelpers
{
    // Тагхелпер для создания ссылок постраничной навигации и сортировки (сортировочная ссылка в названии сторбцов таблицы)
    // с помощью формы (все теги <input> скрыты(hidden), кроме кнопки, которая стилизирует ссылку)
    // Сохраняем данные фильтрации, номера страницы и/или вида сортировки при переходе на новую страницу или сортировке
    public class ReferTagHelper : TagHelper
    {
        public QueryOptions Options { get; set; }
        public int? Page { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Sort { get; set; }

        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("method", "get");

            string listContent = 
                  Tag.Input("CurrentPage", Page ?? Options.CurrentPage)
                + Tag.Input("SortPropertyName", Sort ?? Options.SortPropertyName)
                + Tag.Input("DecsendingOrder", Options.SortPropertyName == Sort ? !Options.DecsendingOrder : Options.DecsendingOrder)
                + Tag.Input("PageSize", Options.PageSize)
                + $@"<input type=""submit"" value=""{Name}"" class=""{Class}"" />";

            for (int i = 0; i < Options.Properties.Count(); i++)
            {
                listContent += Tag.Input("Properties", Options.Properties[i]);
            }

            // Добавляем доп. параметры фильтрации в ссылку с помощью нескольких объеденненных тегов input, что бы не терять данные фильтрации при переходе на другую страницу или сортировке
            // При создании обособленной фильтрации для новой сущности дописываем свой функционал в методе Tag.EntityForm()
            listContent += Tag.EntityForm(Options);

            output.Content.SetHtmlContent(listContent);
        } 
    }

    // Тагхелпер для установки доп. параметров для формы с помощью нескольких объеденненных тегов input, что бы не терять номер страницы и вид сортировки при фильтрации данных таблицы
    // Устанавливается внутри формы фильтрации для любой сущности, принимает объект параметров QueryOptions
    // и булевое значение AddCols для формы в случае фильтрации таблицы по отображаемым столбцам, если значение не будет установленно, то добавление в свойство Prоperties будет двойное: первое в самой форме, а второе уже в этом тагхелпере
    // Во избежание этого в тагхелпер добавлено условие, если значение AddCols установлено в true, то добавление параметров в свойство Prоperties в классе тагхелпера уже не будет
    public class PageForm : TagHelper
    {
        public QueryOptions Options { get; set; }
        public bool AddCols { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            string listContent = 
                  Tag.Input("CurrentPage", Options.CurrentPage)
                + Tag.Input("PageSize", Options.PageSize)
                + Tag.Input("SortPropertyName", Options.SortPropertyName)
                + Tag.Input("ecsendingOrder", Options.DecsendingOrder == true ? "True" : "False");
            if (!AddCols)
            {
                for (int i = 0; i < Options.Properties.Count(); i++)
                {
                    listContent += Tag.Input("Properties", Options.Properties[i]);
                }
            }
            

            output.Content.SetHtmlContent(listContent);
        }
    }

    // Тагхелпер для установки доп. параметров для формы с помощью нескольких объеденненных тегов input, что бы не терять данные фильтрации при дополнительной фильтрации (установка размера страницы и/или фильтрации таблицы по отображаемым столбцам)
    // Устанавливается внутри формы дополнительной фильтрации для любой сущности, принимает объект параметров QueryOptions
    public class EntityForm : TagHelper
    {
        public QueryOptions Options { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(Tag.EntityForm(Options));
        }
    }

    //Вспомогательный статический класс для работы с классом тагхелпера
    public static class Tag
    {
        //Сокращает многократно повторяющееся строковое значение тега <input>
        public static string Input<T>(string opName, T val)
        {
            return @$"<input type=""hidden"" name=""Options.{opName}"" value=""{val}""/>";
        }

        ////Методы для установки доп. параметров фильтрации сущностей для формы с помощью нескольких объеденненных тегов input
        //public static string ProdForm(ProductOptions options)
        //{
        //    string listContent =
        //          Input("MinCount", options.MinCount)
        //        + Input("MaxCount", options.MaxCount)
        //        + Input("Switch", options.Switch);

        //    for (int i = 0; i < options.SearchTerms.Count() && i < options.SearchPropertyNames.Count(); i++)
        //    {
        //        listContent += Input("SearchTerms", options.SearchTerms.Count() > 0 ? options.SearchTerms[i] : string.Empty)
        //                     + Input("SearchPropertyNames", options.SearchPropertyNames[i]);
        //    }
        //    return listContent;
        //}

        //public static string StoreForm(StoreOptions options)
        //{
        //    return Input("SearchTerm", options.SearchTerm) + Input("Switch", options.Switch);
        //}

        //Обобщённый метод для установки доп. параметров фильтрации сущностей для формы с помощью нескольких объеденненных тегов input
        public static string EntityForm(QueryOptions options)
        {
            if(options is ProductOptions prodOp)
            {
                string listContent =
                  Input("MinCount", prodOp.MinCount)
                + Input("MaxCount", prodOp.MaxCount)
                + Input("Switch", prodOp.Switch);

                for (int i = 0; i < prodOp.SearchTerms.Count() && i < prodOp.SearchPropertyNames.Count(); i++)
                {
                    listContent += Input("SearchTerms", prodOp.SearchTerms.Count() > 0 ? prodOp.SearchTerms[i] : string.Empty)
                                 + Input("SearchPropertyNames", prodOp.SearchPropertyNames[i]);
                }
                return listContent;
            }

            if(options is StoreOptions storeOp)
            {
                return Input("SearchTerm", storeOp.SearchTerm) + Input("Switch", storeOp.Switch);
            }

            return string.Empty;
        }
    }


}
