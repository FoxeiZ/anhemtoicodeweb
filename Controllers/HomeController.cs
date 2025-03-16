using anhemtoicodeweb.Enums;
using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly Model1 database = new Model1();
        public ActionResult Index()
        {

            var userRole = Session["UserRole"].IfNotNull(o => (Role)o, Role.Empty);
            var userId = (int?)Session["UserId"];

            var productsQuery = database.Products.AsQueryable();
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

            IEnumerable<Product> productList = productsQuery.OrderByDescending(x => x.Name).ToList();
            IEnumerable<Category> categoriesList = database.Categories.OrderByDescending(x => x.Name).ToList();
            var tuple = new Tuple<IEnumerable<Product>, IEnumerable<Category>>(productList, categoriesList);
            return View(tuple);
        }

        private void SearchInCategory(string query, ref List<Product> searchQuery)
        {
            string norm_name;
            foreach (var item in database.Categories)
            {
                norm_name = Utils.NormalizeDiacriticalCharacters(item.Name);
                if (norm_name.Contains(query))
                {
                    foreach (var prod in item.Products)
                    {
                        if (!searchQuery.Contains(prod))
                        {
                            searchQuery.Add(prod);
                        }
                    }
                }
            }
        }

        public ActionResult Search(string query, int? page = 1)
        {
            ViewBag.CurrentPage = 0;
            ViewBag.MaxPage = 0;

            if (query == null || query.Length == 0)
            {
                return View();
            }
            query = Utils.NormalizeDiacriticalCharacters(query);
            List<Product> searchQuery = new List<Product>();

            string norm_name;
            foreach (var item in database.Products)
            {
                norm_name = Utils.NormalizeDiacriticalCharacters(item.Name);
                if (norm_name.Contains(query))
                {
                    searchQuery.Add(item);
                }
            }
            SearchInCategory(query, ref searchQuery);

            int maxPage = Math.Max(1, searchQuery.Count() / 10);
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;
            return View(searchQuery.Skip(((int)page - 1) * 10).Take(10));
        }

        public ActionResult ProductDetails()
        {
            return View();
        }
    }
}