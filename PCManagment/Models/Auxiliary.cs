using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Raven.Client;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Linq.Expressions;
using RavenDB.AspNet.Identity;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RavenDB.AspNet.Identity;

namespace MvcPWy.Models
{
    public class CustomPropertyInfo
    {
        public PropertyInfo Property { get; set; }
        public HitList Attribute { get; set; }

        public CustomPropertyInfo(PropertyInfo prop, HitList hitlist)
        {
            this.Property = prop;
            this.Attribute = hitlist;
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source,
            string propertyName, string direction = "ASC")
        {
            if (direction == "ASC")
                return source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            else
                return source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
        }

        public static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(type, "x");
            MemberExpression propertyAccess = Expression.Property(parameter, propertyName);
            return Expression.Lambda(propertyAccess, parameter);
        }
    }

    public static class CustomHtmlHelper
    {

        public static MvcHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl,
            string altText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            var link = helper.ActionLink("[replace]", actionName, controllerName, routeValues, ajaxOptions);
            return MvcHtmlString.Create(link.ToString().Replace("[replace]", builder.ToString(TagRenderMode.SelfClosing)));
        }

        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName,
            string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }

        public static MvcHtmlString DropDownList(this AjaxHelper html, string action, string controller,
            RouteValueDictionary routeValues, AjaxOptions options, IEnumerable<SelectListItem> selectItems,
            IDictionary<string, object> listHtmlAttributes)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // Wrap it in a form
            var formBuilder = new TagBuilder("form");


            //  build the <select> tag
            var listBuilder = new TagBuilder("select");
            if (listHtmlAttributes != null && listHtmlAttributes.Count > 0)
                listBuilder.MergeAttributes(listHtmlAttributes);
            StringBuilder optionHTML = new StringBuilder();
            foreach (SelectListItem item in selectItems)
            {
                var optionBuilder = new TagBuilder("option");
                optionBuilder.MergeAttribute("value", item.Value);
                optionBuilder.InnerHtml = item.Text;
                if (item.Selected)
                {
                    optionBuilder.MergeAttribute("selected", "selected");
                }

                //optionBuilder.Attributes["onchange"] = "($this.form).attr('action', '" + url.Action(action).Replace("___", item.Value) + "');$(this.form).submit();";
                optionHTML.Append(optionBuilder.ToString());
            }
            listBuilder.InnerHtml = optionHTML.ToString();
            listBuilder.Attributes["onchange"] = "$(this.form).attr('action', '/" + controller + "/" + action + "?pageSize=' + $(this).first('option:selected').val());$(this).first('option:selected').val();$(this.form).submit();";
            formBuilder.InnerHtml = listBuilder.ToString();

            foreach (var ajaxOption in options.ToUnobtrusiveHtmlAttributes())
                formBuilder.MergeAttribute(ajaxOption.Key, ajaxOption.Value.ToString());
            string formHtml = formBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(formHtml);
        }

        public static MvcHtmlString DisplayFor(this HtmlHelper helper, object value)
        {
            if (value != null)
            {
                switch (value.GetType().Name)
                {
                    case "Boolean":
                        TagBuilder checkBuilder = new TagBuilder("paper-checkbox");
                        checkBuilder.MergeAttribute("disabled", "true");
                        if ((bool)value)
                            checkBuilder.MergeAttribute("checked", "true");
                        return MvcHtmlString.Create(checkBuilder.ToString());
                    case "DateTime":
                        var result = ((DateTime)value).ToString("dd/MM/yyyy");
                        return MvcHtmlString.Create(result == "01/01/0001" ? "" : result);
                    default:
                        return MvcHtmlString.Create(value.ToString());
                }
            }
            else
                return MvcHtmlString.Create("");
        }
    }

    public static class GenericHelpers
    {
        public static IEnumerable<CustomPropertyInfo> GetHitListProperty(IEnumerable<MvcPWy.Models.IIndex> Model)
        {
            var props = from p in Model.First().GetType().GetProperties()
                let attr = p.GetCustomAttributes(typeof(HitList), true)
                where attr.Length == 1
                select new CustomPropertyInfo(p,attr.First() as HitList);
            return props;
        }

        public static string GetDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return property.Name;
            return (atts[0] as DisplayAttribute).Name;
        }

        public static string IsNull(string value, string isNullValue)
        {
            if (value == null)
                return isNullValue;
            else
                return value;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static SelectListItem[] PageSizeList = new[] {
                new SelectListItem { Text = "10", Value = "0", Selected = false},
                new SelectListItem { Text = "20", Value = "1", Selected = false},
                new SelectListItem { Text = "50", Value = "2", Selected = false},
                new SelectListItem { Text = "100", Value = "3", Selected = false}
            };
    }

    public static class GetHitList
    {
        public static List<TModel> GetPage<TModel, TSearch>(IDocumentSession ravenDb, int skippedResult, int pageSize,
            string defaultOrder, string SortOrder, string SortDirection, string search,
            out RavenQueryStatistics statsRef)
        {
            string where = "Id != @0 ";
            List<string> paramiters = new List<string>();
            paramiters.Add(null);

            if (!string.IsNullOrEmpty(search))
            {
                TSearch searchItem = new JavaScriptSerializer().Deserialize<TSearch>(search);
                int i = 1;
                foreach (var property in searchItem.GetType().GetProperties())
                {
                    if (property.GetValue(searchItem) != null && property.PropertyType.Name == "String")
                    {
                        where += " AND " + property.Name + " != null AND " + property.Name + ".StartsWith(@"
                            + i.ToString() + ") ";
                        paramiters.Add(property.GetValue(searchItem).ToString().ToLower());
                        i++;
                    }
                }
            }

            return ravenDb.Query<TModel>().Statistics(out statsRef).
                Where(where, paramiters.ToArray<string>()).
                OrderBy(GenericHelpers.IsNull(SortOrder, defaultOrder) + " "
                + GenericHelpers.IsNull(SortDirection, "DESC")).
                Skip(skippedResult).Take(pageSize).ToList();
        }
    }

    public class IndexData
    {
        public string SortOrder { get; set; }
        public string SortDirection { get; set; }
        public int ActualPage { get; set; }
        public string PageSize { get; set; }
        public string Search { get; set; }
        public int TotalPage { get; set; }
        public double TotalResult { get; set; }

        public IndexData(string defaultSortOrder)
        {
            this.SortOrder = defaultSortOrder;
            this.SortDirection = "ASC";
            this.ActualPage = 1;
        }

        public IndexData ChangePage(IndexData data, int page)
        {
            data.ActualPage = page;
            return data;
        }
    }

    public static class StandardAjaxOptions
    {
        public static AjaxOptions AjaxOptions(bool loading)
        {
            return AjaxOptions(null, null, true, null, null, InsertionMode.Replace);
        }

        public static AjaxOptions AjaxOptions()
        {
            return AjaxOptions(null, null, false, null, null, InsertionMode.Replace);
        }

        public static AjaxOptions AjaxOptions(string httpMethod)
        {
            return AjaxOptions(httpMethod, null, false, null, null, InsertionMode.Replace);
        }

        public static AjaxOptions AjaxOptions(string httpMethod, string updateTargetId)
        {
            return AjaxOptions(httpMethod, updateTargetId, false, null, null, InsertionMode.Replace);
        }

        public static AjaxOptions AjaxOptions(string httpMethod, string updateTargetId, bool loading)
        {
            return AjaxOptions(httpMethod, updateTargetId, loading, null, null, InsertionMode.Replace);
        }

        public static AjaxOptions AjaxOptions(string httpMethod, string updateTargetId, bool loading, string onSuccess)
        {
            return AjaxOptions(httpMethod, updateTargetId, loading, onSuccess, null, InsertionMode.Replace);
        }

        public static AjaxOptions AjaxOptions(string httpMethod, string updateTargetId, bool loading,
            string onSuccess, string onFailure, InsertionMode inseretionMode)
        {
            var options = new AjaxOptions();
            options.InsertionMode = inseretionMode;
            options.OnFailure = string.IsNullOrEmpty(onFailure) ? "ShowToast('An error occurs', 'Ko')" : onFailure;
            if (!string.IsNullOrEmpty(onSuccess))
                options.OnSuccess = onSuccess;
            options.UpdateTargetId = string.IsNullOrEmpty(updateTargetId) ? "content" : updateTargetId;
            if (loading)
                options.LoadingElementId = "loader";
            options.HttpMethod = string.IsNullOrEmpty(httpMethod) ? "GET" : httpMethod;
            return options;
        }
    }
}
