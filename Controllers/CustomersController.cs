using anhemtoicodeweb.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Model1 db = new Model1();

        [Filters.RequireAdminRole]
        public ActionResult Index()
        {
            var customers = db.Users.Where(x => x.RoleEnumId == (int)Enums.Role.Customer).ToList();
            if (ControllerContext.IsChildAction)
            {
                return PartialView(customers.ToList());
            }
            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            return View(customers.ToList());
        }

        [Filters.RequireAdminRole]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Users.Where(x => x.ID == id).FirstOrDefault();
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(customer);
        }


        [Filters.RequireAdminRole]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireAdminRole]
        public ActionResult Create([Bind(Include = "ID,Name,Role,Phone,Email,AddressName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = Enums.Role.Customer;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }


        [Filters.RequireAdminRole]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User customer = db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.RequireAdminRole]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,Email,AddressName,Password")] User customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [Filters.RequireAdminRole]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User customer = db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Filters.RequireAdminRole]
        public ActionResult DeleteConfirmed(int id)
        {
            User customer = db.Users.Find(id);
            db.Users.Remove(customer);
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
