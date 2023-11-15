using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class ProfileController : Controller
    {
        Model1 database = new Model1();
        public ActionResult Index()
        {
            int _userId = (int?)Session["UserId"] ?? -1;
            if (_userId < 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var _user = database.Customers.Find(_userId);
            return View(_user);
        }

        public PartialViewResult Products()
        {
            var products = new List<Product>();
            return PartialView(database.Products.ToList());
        }

        public ActionResult EditProfile()
        {
            int id = (int?)Session["UserId"] ?? -1;
            if (id < 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var user = database.Customers.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                database.Entry(customer).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}