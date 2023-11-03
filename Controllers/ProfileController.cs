using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
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
    }
}