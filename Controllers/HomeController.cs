﻿using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            IEnumerable<Product> productList = database.Products.OrderByDescending(x => x.NamePro).ToList();
            IEnumerable<Category> categoriesList = database.Categories.OrderByDescending(x => x.NameCate).ToList();
            var tuple = new Tuple<IEnumerable<Product>, IEnumerable<Category>>(productList, categoriesList);
            return View(tuple);
        }
        public ActionResult Search(string query)
        {
            if (query == null || query.Length == 0)
            {
                return View();
            }
            query = NormalizeDiacriticalCharacters(query);
            List<Product> searchQuery = new List<Product>();

            string norm_name;
            foreach (var item in database.Products)
            {
                norm_name = NormalizeDiacriticalCharacters(item.NamePro);
                if (norm_name.Contains(query))
                {
                    searchQuery.Add(item);
                }
            }
            return View(searchQuery);
        }

        private string NormalizeDiacriticalCharacters(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var normalised = value.Normalize(NormalizationForm.FormD).ToLower().ToCharArray();

            return new string(normalised.Where(c => (int)c <= 127).ToArray());
        }

        public ActionResult ProductDetails()
        {
            return View();
        }
    }
}