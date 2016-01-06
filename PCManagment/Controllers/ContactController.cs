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
    public class ContactController : Controller
    {
        [Authorize]
        public ActionResult Create(string SortOrder, string SortDirection, int? page, string search)
        {
            var view = new Contact();
            ViewBag.ActualPage = page;
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortDirection = SortDirection;
            ViewBag.Search = search;
            return PartialView(view);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Contact contact, string SortOrder, string SortDirection, int? page, 
            string search)
        {
            if (ModelState.IsValid)
            {
                var ravenDB = HttpContext.GetOwinContext().Get<IDocumentSession>();
                contact.Updated = DateTime.Now;
                ravenDB.Store(contact);
                ravenDB.SaveChanges();
                TempData["IsValid"] = "";
                return RedirectToAction("Edit", new
                {
                    id = GenericHelpers.Base64Encode(contact.Id),
                    SortOrder = SortOrder,
                    SortDirection = SortDirection,
                    page = page,
                    search = search,
                    Class = "Contact"
                });
            }
            TempData["IsValid"] = "Ko";
            return PartialView(contact);
        }

        [HttpGet]
        [Authorize]
        public PartialViewResult Edit(string id, string SortOrder, string SortDirection, int? page,
            string search)
        {
            //if (id == null)
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            id = GenericHelpers.Base64Decode(id);
            var ravenDB = HttpContext.GetOwinContext().Get<IDocumentSession>();
            var contact = ravenDB.Load<Contact>(id);
            //if (contact == null)
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return PartialView(contact);
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = GenericHelpers.Base64Decode(id);
            var ravenDB = HttpContext.GetOwinContext().Get<IDocumentSession>();
            ravenDB.Load<Contact>(id);
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
            return RedirectToAction("Index", "HitList", new
            {
                SortOrder = SortOrder,
                SortDirection = SortDirection,
                page = page,
                Class = "Contact"
            });
        }
    }
}