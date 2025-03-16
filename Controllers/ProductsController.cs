using anhemtoicodeweb.Enums;
using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Model1 db = new Model1();

        public PartialViewResult PartialProduct(Product product)
        {
            return PartialView(product);
        }

        public PartialViewResult PartialOrdersProduct(OrderDetail product)
        {
            return PartialView(product);
        }

        // GET: Products
        public ActionResult Index(int page = 1, int? fromSeller = null)
        {
            if (ControllerContext.IsChildAction)
            {
                return PartialView(db.Products.ToList());
            }

            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

            var userRole = Session["UserRole"].IfNotNull(o => (Role)o, Role.Empty);
            var userId = (int?)Session["UserId"];

            var productsQuery = db.Products.AsQueryable();

            // Filter products based on user role
            if (userRole == Role.Admin)
            {
                // Admin sees all products
                // No additional filters needed
            }
            else if (userRole == Role.Seller)
            {
                // Seller sees their own products (both shown and hidden) plus other sellers' shown products
                productsQuery = productsQuery.Where(p =>
                    p.StateEnumId == (int)ProductState.Shown ||
                    (p.SellerId == userId && p.StateEnumId != (int)ProductState.Deleted));
            }
            else
            {
                // Regular users only see shown products
                productsQuery = productsQuery.Where(p => p.StateEnumId == (int)ProductState.Shown);
            }

            // Additional filter if viewing specific seller's products
            if (fromSeller.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.SellerId == fromSeller.Value);
            }

            int maxPage = Math.Max(1, (int)Math.Ceiling((double)productsQuery.Count() / 10));
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;

            return View("UserIndex", productsQuery.OrderBy(x => x.Name).Skip((page - 1) * 15).Take(15).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var userRole = Session["UserRole"].IfNotNull(o => (Role)o, Role.Empty);
            var userId = (int?)Session["UserId"];

            // Admin can see all products
            // Sellers can see their own products
            // Regular users can only see products with "Shown" state
            if (userRole == Role.Admin ||
                (userRole == Role.Seller && product.SellerId == userId &&
                product.State != ProductState.Deleted) || product.State == ProductState.Shown)
            {
                return View(product);
            }

            // Unauthorized access or hidden/deleted product
            return RedirectToAction("Index");
        }

        // GET: Products/Create
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View(new Product());
        }

        private void ProcessProduct(Product product)
        {
            product.UploadImage?.SaveAs(Path.Combine(Server.MapPath("~/Image/Product/"), product.ImageLocation.Split('/').Last()));
            if (product.ImageLocation == null)
            {
                product.ImageLocation = "~/Image/Product/banphim.png";
            }
            if (product.OldImageLocation != null && product.ImageLocation != product.OldImageLocation)
            {
                var oldfile = Path.Combine(Server.MapPath("~/Image/Product/"), product.OldImageLocation.Split('/').Last());
                var file = Path.Combine(Server.MapPath("~/Image/Product/"), product.ImageLocation.Split('/').Last());
                System.IO.File.Move(oldfile, file);
            }

            product.FinalPrice = product.Price + (product.Price * product.Tax) - (product.Price * product.Discount);
            product.SellerId = (int)Session["UserId"];
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Create(Product product)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            //    return View(db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault());
            //}

            //if (product.ImageLocation == null)
            //{
            //    product.ImageLocation = "~/Image/Product/CuonTuiRac.jpg";
            //}

            //if (product.UploadImage != null)
            //{
            //    product.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Image/Product/"), product.ImageLocation.Split('/').Last()));
            //}

            //product.FinalPrice = product.Price + (product.Price * product.Tax) - (product.Price * product.Discount);

            //if (ModelState.IsValid)
            //{
            //    db.Products.Add(product);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            //return View(product);
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
                return View(db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault());
            }
            ProcessProduct(product);
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = product.ProductID });
        }

        // GET: Products/Edit/5
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
                return View(db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault());
            }
            ProcessProduct(product);
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = product.ProductID });
        }

        // GET: Products/Delete/5
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            //db.Products.Remove(product);
            product.State = ProductState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Products/Restore/5
        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        [ActionName("Restore")]
        public ActionResult RestoreConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireLoginWithRole(Role.Admin)]
        public ActionResult Restore(int id)
        {
            Product product = db.Products.Find(id);
            //db.Products.Remove(product);
            product.State = ProductState.Shown;
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
        }

        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Hide(int id)
        {
            var product = db.Products.Find(id);
            product.State = ProductState.Hidden;
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
        }

        [Filters.RequireLoginWithRole(Role.Seller, Role.Admin)]
        public ActionResult Show(int id)
        {
            var product = db.Products.Find(id);
            product.State = ProductState.Shown;
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
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
