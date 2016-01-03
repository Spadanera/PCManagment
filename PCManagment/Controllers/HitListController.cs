using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MvcPWy.Models;
using Raven.Client;
using PagedList;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace MvcPWy.Controllers
{
    public class HitListController : Controller
    {
        // GET: HitList
        public PartialViewResult Index(string SortOrder, string SortDirection, int? page, string pageSize,
            string search, string Class)
        {
            if (!string.IsNullOrEmpty(search))
                ViewBag.Filtered = "Y";
            else
                ViewBag.Filtered = "N";
            //List<Project> projects;
            var ravenDb = HttpContext.GetOwinContext().Get<IDocumentSession>();
            RavenQueryStatistics statsRef = new RavenQueryStatistics();

            if (page == null)
                page = 1;

            if (Request.Cookies["pageSize"] == null)
            {
                pageSize = GenericHelpers.IsNull(pageSize, "10");
                HttpCookie pageSizeCookie = new HttpCookie("pageSize");
                pageSizeCookie.Value = pageSize;
                Response.Cookies.Add(pageSizeCookie);
            }
            else
            {
                pageSize = GenericHelpers.IsNull(pageSize, Request.Cookies["pageSize"].Value);
                Response.Cookies["pageSize"].Value = pageSize;
            }

            foreach (var pSize in GenericHelpers.PageSizeList)
            {
                pSize.Selected = false;
                if (pSize.Text == pageSize.ToString())
                    pSize.Selected = true;
            }

            int pageNumber = (page ?? 1);
            int skippedResult = Convert.ToInt32(pageSize) * (pageNumber - 1);
            object results;
            switch (Class)
            {
                case "Project":
                    results = GetHitList.GetPage<Project, Project>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
                case "Contact":
                    results = GetHitList.GetPage<Contact, Contact>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
                case "User":
                    results = GetHitList.GetPage<ApplicationUser, ApplicationUser>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
                case "Distributor":
                    results = GetHitList.GetPage<Distributor, Distributor>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
                case "Detail":
                    results = GetHitList.GetPage<ProjectDetail, ProjectDetail>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
                case "Warning":
                    results = GetHitList.GetPage<Warning, Warning>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
                default:
                    results = GetHitList.GetPage<Project, Project>(ravenDb, skippedResult, Convert.ToInt32(pageSize),
                        "Id", SortOrder, SortDirection, search, out statsRef);
                    break;
            }

            ViewBag.TotalPage = Math.Round((double)(statsRef.TotalResults / Convert.ToInt32(pageSize))) + 1;
            ViewBag.ActualPage = pageNumber;
            ViewBag.TotalResult = statsRef.TotalResults;
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortDirection = SortDirection;
            ViewBag.PageSizeList = GenericHelpers.PageSizeList;
            ViewBag.Search = search;
            ViewData["controller"] = Class;
            return PartialView("Index", results);
        }
    }
}