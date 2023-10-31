using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class HomeController : Controller
    {
        private Model1 database = new Model1();
        public ActionResult Index()
        {
            var productList = database.Products.OrderByDescending(x => x.NamePro);
            return View(productList);
        }
        public ActionResult ProductDetails()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

    }
}