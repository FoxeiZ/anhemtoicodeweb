using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


public class EditDiscountModel
{
    public int DiscountID { get; set; }
    public string NameDiscount { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
}

namespace anhemtoicodeweb.Controllers
{

    public class DiscountsController : Controller
    {
        private Model1 db = new Model1();

        private protected void GenerateViewBag(dynamic viewBag, Discount discount = null)
        {
            var unit = new Dictionary<String, String>()
            {
                { "fixed", "đ"},
                { "percent", "%"},
            };
            viewBag.Unit = unit.Select(
                x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key,
                    Selected = discount.IfNotNull(d => x.Key == d.TypeDiscount)
                }).ToList();

            var type = new Dictionary<String, String>()
            {
                {"product", "Giảm giá theo mặt hàng" },
                {"code", "Giảm giá theo code cho sẵn" },
                {"other", "Khác" }
            };
            viewBag.TypeDiscount = type.Select(
                x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key,
                    Selected = discount.IfNotNull(d => x.Key == d.TypeDiscount)
                }).ToList();

        }

        // GET: Discounts
        public ActionResult Index()
        {
            return View(db.Discounts.ToList());
        }

        // GET: Discounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // GET: Discounts/Create
        public ActionResult Create()
        {
            GenerateViewBag(ViewBag);
            return View(new Discount());
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiscountID,NameDiscount,TypeDiscount,CodeDiscount,ValueDiscount,Unit,StartDate,EndDate,Description")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                discount.Owner = (string)Session["Name"];
                db.Discounts.Add(discount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // GET: Discounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }

            GenerateViewBag(ViewBag, discount);
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiscountID,NameDiscount,EndDate,Description")] EditDiscountModel discount)
        {
            if (!ModelState.IsValid)
            {
                //GenerateViewBag(ViewBag, discount);
                return View(discount);
            }

            var origin_discount = db.Discounts.Find(discount.DiscountID);
            db.Entry(origin_discount).CurrentValues.SetValues(discount);
            db.Entry(origin_discount).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Discounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discount discount = db.Discounts.Find(id);
            db.Discounts.Remove(discount);
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
