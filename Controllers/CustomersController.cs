using anhemtoicodeweb.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class CustomersController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index()
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            var customers = db.Customers.ToList();
            if (ControllerContext.IsChildAction)
            {
                return PartialView(customers.ToList());
            }
            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            return View(customers.ToList());
        }

        public ActionResult Create()
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,AddressName,PasswordCus")] Customer customer)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,AddressName,PasswordCus")] Customer customer)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
