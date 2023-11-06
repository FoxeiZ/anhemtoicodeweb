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
            IEnumerable<Product> productList = database.Products.OrderByDescending(x => x.NamePro);
            IEnumerable<Category> categoriesList = database.Categories.OrderByDescending(x => x.NameCate);
            var tuple = new Tuple<IEnumerable<Product>, IEnumerable<Category>>(productList, categoriesList);
            return View(productList);
        }
        public ActionResult Search(string query)
        {
            if (query == null || query.Length == 0)
            {
                return View();
            }
            IEnumerable<Product> searchQuery = database.Products.Where(x => x.NamePro.Contains(query));
            return View(searchQuery);
        }
        public ActionResult ProductDetails()
        {
            return View();
        }
    }
}