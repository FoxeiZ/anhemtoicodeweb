using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class OrderProesController : Controller
    {
        private Model1 db = new Model1();

        // GET: OrderProes
        public ActionResult Index()
        {
            var orderProes = db.OrderProes.Include(o => o.Customer);
            return View(orderProes.ToList());
        }

        // GET: OrderProes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        // GET: OrderProes/Create
        public ActionResult Create()
        {
            ViewBag.IDCus = new SelectList(db.Users, "ID", "Name");
            return View();
        }

        // POST: OrderProes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOrder,ID,PhoneNumber,AddressDelivery,TotalAmount,TotalMoney,TotalTax,TotalDiscount,State")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.OrderProes.Add(orderPro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCus = new SelectList(db.Users, "ID", "Name", orderPro.IDCus);
            return View(orderPro);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            OrderPro _order = db.OrderProes.Where(x => x.ID == id).FirstOrDefault();
            if (_order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.State = Enums.OrderState.GetAllStates().Select(x => new SelectListItem { Text = x, Value = x, Selected = (x == _order.State) }).ToList();
            return View(_order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOrder,ID,PhoneNumber,AddressDelivery,TotalAmount,TotalMoney,TotalTax,TotalDiscount,State")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderPro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CheckOrder", new { id = orderPro.ID });
            }

            ViewBag.State = Enums.OrderState.GetAllStates().Select(x => new SelectListItem { Text = x, Value = x, Selected = (x == orderPro.State) }).ToList();

            return View(orderPro);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderPro orderPro = db.OrderProes.Find(id);
            db.OrderDetails.Where(x => x.IDOrder == orderPro.ID).ForEach(x => db.OrderDetails.Remove(x));
            db.OrderProes.Remove(orderPro);
            db.SaveChanges();
            return RedirectToAction("Details", "Users", new { id = orderPro.IDCus });
        }

        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        [HttpPost, ActionName("CancelOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            OrderPro orderPro = db.OrderProes.Find(id);
            orderPro.State = "Đang hủy";
            db.Entry(orderPro).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CheckOrder", new { id = orderPro.ID });
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
