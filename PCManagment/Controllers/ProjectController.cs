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
    public class ProjectController : Controller
    {
        //public PartialViewResult Index(string SortOrder, string SortDirection, int? page, string pageSize, 
        //    string search)
        //{
        //    if (!string.IsNullOrEmpty(search))
        //        ViewBag.Filtered = "Y";
        //    else
        //        ViewBag.Filtered = "N";
        //    //List<Project> projects;
        //    var ravenDb = HttpContext.GetOwinContext().Get<IDocumentSession>();
        //    RavenQueryStatistics statsRef = new RavenQueryStatistics();

        //    if (page == null)
        //        page = 1;

        //    if (Request.Cookies["pageSize"] == null)
        //    {
        //        pageSize = GenericHelpers.IsNull(pageSize, "10");
        //        HttpCookie pageSizeCookie = new HttpCookie("pageSize");
        //        pageSizeCookie.Value = pageSize;
        //        Response.Cookies.Add(pageSizeCookie);
        //    }
        //    else
        //    {
        //        pageSize = GenericHelpers.IsNull(pageSize, Request.Cookies["pageSize"].Value);
        //        Response.Cookies["pageSize"].Value = pageSize;
        //    }

        //    foreach (var pSize in GenericHelpers.PageSizeList)
        //    {
        //        pSize.Selected = false;
        //        if (pSize.Text == pageSize.ToString())
        //            pSize.Selected = true;
        //    }
            
        //    int pageNumber = (page ?? 1);
        //    int skippedResult = Convert.ToInt32(pageSize) * (pageNumber - 1);

        //    var projects = GetHitList.GetPage<Project, Project>(ravenDb, skippedResult, Convert.ToInt32(pageSize), 
        //        "Id", SortOrder, SortDirection, search, out statsRef);

        //    ViewBag.TotalPage = Math.Round((double)(statsRef.TotalResults / Convert.ToInt32(pageSize))) + 1;
        //    ViewBag.ActualPage = pageNumber;
        //    ViewBag.TotalResult = statsRef.TotalResults;
        //    ViewBag.SortOrder = SortOrder;
        //    ViewBag.SortDirection = SortDirection;
        //    ViewBag.PageSizeList = GenericHelpers.PageSizeList;
        //    ViewBag.Search = search;
        //    ViewData["controller"] = "Project";
        //    return PartialView("Index", projects);
        //}

        // GET: Project/Create
        [Authorize]
        public ActionResult Create(string SortOrder, string SortDirection, int? page, string search)
        {
            var view = new Project();
            view.Date = DateTime.Now;
            ViewBag.ActualPage = page;
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortDirection = SortDirection;
            ViewBag.Search = search;
            return PartialView(view);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Project project, string SortOrder, string SortDirection, int? page, string search)
        {
            if (ModelState.IsValid)
            {
                TempData["IsValid"] = "";
                var ravenDB = HttpContext.GetOwinContext().Get<IDocumentSession>();
                project.Updated = DateTime.Now;
                ravenDB.Store(project);
                ravenDB.SaveChanges();
                return RedirectToAction("Index", "HitList", new
                {
                    SortOrder = SortOrder,
                    SortDirection = SortDirection,
                    page = page,
                    search = search,
                    Class = "Project"
                });
            }
            TempData["IsValid"] = "Ko";
            return PartialView(project);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = GenericHelpers.Base64Decode(id);
            var ravenDB = HttpContext.GetOwinContext().Get<IDocumentSession>();
            ravenDB.Load<Project>(id);
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(string SortOrder, string SortDirection, int? page, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = GenericHelpers.Base64Decode(id);
            var ravenDB = HttpContext.GetOwinContext().Get<IDocumentSession>();
            ravenDB.Delete(id);
            ravenDB.SaveChanges();
            return RedirectToAction("Index", "HitList", new {
                SortOrder = SortOrder,
                SortDirection = SortDirection,
                page = page,
                Class = "Project"
            });
        }

        //GET: Project/Search
        [Authorize]
        public ActionResult Search()
        {
            var view = new Project();
            return PartialView(view);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(Project project)
        {
            var json = new JavaScriptSerializer().Serialize(project);
            RouteValueDictionary routeValues = new RouteValueDictionary();
            routeValues.Add("search", json);
            routeValues.Add("Class", "Project");
            return RedirectToAction("index", "HitList", new RouteValueDictionary(routeValues));
        }
    }
}