using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Raven.Client;
using System.Web.Routing;
using System.Linq.Expressions;

namespace MvcPWy.Models
{
    public static class Polymer
    {
        public static MvcHtmlString PolymerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> polymerHelper, 
            Expression<Func<TModel, TProperty>> expression, string type)
        {
            return polymerHelper.PolymerTextBoxFor(expression, type, null);
        }

        public static MvcHtmlString PolymerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> polymerHelper,
            Expression<Func<TModel, TProperty>> expression, string type, 
            IDictionary<string, object> listHtmlAttributes)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, polymerHelper.ViewData);
            TagBuilder builder = new TagBuilder("paper-input");
            builder.MergeAttribute("name", data.PropertyName);
            builder.MergeAttribute("label", string.IsNullOrEmpty(data.DisplayName)
                ? data.PropertyName : data.DisplayName);
            builder.MergeAttribute("type", type);
            if (listHtmlAttributes != null)
                foreach (var attribute in listHtmlAttributes)
                    builder.MergeAttribute(attribute.Key, attribute.Value.ToString());
            if (data.Model != null)
            {
                builder.MergeAttribute("value", data.Model.ToString());
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString PolymerCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> polymerHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, polymerHelper.ViewData);
            TagBuilder builder = new TagBuilder("paper-checkbox");
            builder.MergeAttribute("name", data.PropertyName);
            builder.InnerHtml = data.DisplayName;
            builder.MergeAttribute("onclick", "toggleInputCheckBox('#" + data.PropertyName + "')");
            
            TagBuilder builderInput = new TagBuilder("input");
            builderInput.MergeAttribute("type", "hidden");
            builderInput.MergeAttribute("name", data.PropertyName);
            builderInput.MergeAttribute("id", data.PropertyName);
            builderInput.MergeAttribute("value", data.Model == null ? "false" : data.Model.ToString());

            builder.InnerHtml = data.GetDisplayName() + builderInput.ToString();

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString PolymerPaperActionIcon ( this AjaxHelper helper, string icon, string action,
            string controller, object routeValues, AjaxOptions options, 
            IDictionary<string, object> listHtmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            options.Url = urlHelper.Action(action, controller, routeValues);
            TagBuilder builder = new TagBuilder("paper-icon-button");
            builder.MergeAttribute("icon", icon);
            if (listHtmlAttributes != null)
                foreach (var attribute in listHtmlAttributes)
                    builder.MergeAttribute(attribute.Key, attribute.Value.ToString());
            builder.MergeAttributes((options ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes());
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString PolymerPaperTab(this AjaxHelper helper, string label, string action, string controller,
            object routeValues, AjaxOptions options, IDictionary<string, object> listHtmlAttributes)
        {
            options.Url = "/" + controller + "/" + action;
            TagBuilder builder = new TagBuilder("paper-tab");
            builder.InnerHtml = label;
            if (listHtmlAttributes != null)
                foreach (var attribute in listHtmlAttributes)
                    builder.MergeAttribute(attribute.Key, attribute.Value.ToString());
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(action, controller, routeValues);
            options.Url = url;
            builder.MergeAttributes((options ?? new AjaxOptions()).ToUnobtrusiveHtmlAttributes());
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString PolymerPaperTabGroup(this AjaxHelper helper, string action, 
            string controller, RouteValueDictionary routeValues, AjaxOptions options, 
            IDictionary<string, object> listHtmlAttributes, IEnumerable<SelectListItem> selectItems)
        {
            var builder = new TagBuilder("paper-tabs");
            if (listHtmlAttributes != null)
                foreach (var attribute in listHtmlAttributes)
                    builder.MergeAttribute(attribute.Key, attribute.Value.ToString());
            var sb = new StringBuilder();
            foreach (var item in selectItems)
            {
                RouteValueDictionary rv = new RouteValueDictionary();
                if (routeValues != null)
                    rv = routeValues;
                rv.Add("pageSize", item.Text);
                sb.Append(helper.PolymerPaperTab(item.Text, action, controller, rv, options, null));
                if (item.Selected)
                    builder.MergeAttribute("selected", item.Value);
            }
            builder.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}