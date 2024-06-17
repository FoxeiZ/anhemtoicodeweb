using anhemtoicodeweb.Models;
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
        private Model1 db = new Model1();

        public PartialViewResult PartialProduct(Product product)
        {
            return PartialView(product);
        }

        // GET: Products
        public ActionResult Index(int page = 1)
        {
            if (ControllerContext.IsChildAction)
            {
                return PartialView(db.Products.ToList());
            }
            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            int maxPage = Math.Max(1, db.Products.Count() / 10);
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;
            return View("UserIndex", db.Products.OrderBy(x => x.NamePro).Skip((page - 1) * 15).Take(15).ToList());
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
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index");
            }
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "NameCate");
            return View(new Product());
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index");
            }

            if (product.ImagePro == null)
            {
                product.ImagePro = "~/Image/Product/CuonTuiRac.jpg";
            }

            if (product.InvQuantity == 0)
            {
                product.InvQuantity = 1000;
            }

            if (product.UploadImage != null)
            {
                product.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Image/Product/"), product.ImagePro.Split('/').Last()));
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "NameCate", product.IDCate);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "NameCate", product.IDCate);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index");
            }

            if (product.UploadImage != null)
            {
                product.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Image/Product/"), product.ImagePro.Split('/').Last()));
            }

            if (product.OldImagePro != null && product.ImagePro != product.OldImagePro)
            {
                var oldfile = Path.Combine(Server.MapPath("~/Image/Product/"), product.OldImagePro.Split('/').Last());
                var file = Path.Combine(Server.MapPath("~/Image/Product/"), product.ImagePro.Split('/').Last());
                System.IO.File.Move(oldfile, file);
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "NameCate", product.IDCate);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index");
            }

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
