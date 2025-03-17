using anhemtoicodeweb.Enums;
using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class SellerController : Controller
    {
        private readonly Model1 db = new Model1();

        public ActionResult Details(int? id, int page = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var seller = db.Users.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }

            var products = db.Users.Find(id).Products.ToList();
            if (products.Count() == 0)
            {
                return HttpNotFound();
            }

            (ViewBag.MaxPage, ViewBag.CurrentPage) = Utils.PaginatorCalc(products, 10, page);
            var tuple = new Tuple<User, IEnumerable<Product>>(seller, products.Skip((page - 1) * 10).Take(10));
            return View(tuple);
        }

        [Filters.RequireAdminRole]
        public ActionResult Pending(int page = 1)
        {
            var sellerPendingQuery = db.Users.AsQueryable().Where(s => s.RoleEnumId == (int)Role.SellerPending);
            (ViewBag.MaxPage, ViewBag.CurrentPage) = Utils.PaginatorCalc(sellerPendingQuery, 10, page);
            return View(sellerPendingQuery);
        }
    }
}