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

        public ActionResult Details(int id, int page = 1)
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

            int maxPage = Math.Max(1, products.Count() / 10);
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;

            var tuple = new Tuple<User, IEnumerable<Product>>(seller, products.Skip((page - 1) * 10).Take(10));
            return View(tuple);
        }
    }
}