using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly Model1 db = new Model1();

        public ActionResult Index()
        {
            var category = db.Categories.ToList();
            if (ControllerContext.IsChildAction)
            {
                return PartialView(category.ToList());
            }

            return RedirectToAction("Details", new { id = db.Categories.Where(e => e.Id == 1) });
        }

        public ActionResult Details(string id, int page = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            IEnumerable<Product> productList = category.Products.ToList();

            int maxPage = Math.Max(1, productList.Count() / 10);
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;

            var tuple = new Tuple<Category, IEnumerable<Product>>(category, productList.Skip((page - 1) * 10).Take(10));
            return View(tuple);
        }

        [Filters.RequireAdminRole]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireAdminRole]
        public ActionResult Create([Bind(Include = "CategoryId,Id,Name")] Category category)
        {

            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [Filters.RequireAdminRole]
        public ActionResult Edit(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireAdminRole]
        public ActionResult Edit([Bind(Include = "CategoryId,Id,Name")] Category category)
        {

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [Filters.RequireAdminRole]
        public ActionResult Delete(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Filters.RequireAdminRole]
        public ActionResult DeleteConfirmed(string id)
        {

            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
