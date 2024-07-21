using anhemtoicodeweb.Models;
using anhemtoicodeweb.Models.ThongKe;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly Model1 database = new Model1();
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
        public ActionResult EditProfile(Customer customer, string PrevPassword)
        {
            if (ModelState.IsValid)
            {
                if (customer.PasswordCus != customer.ConfirmPasswordCus)
                {
                    TempData["Error"] = "Mật khẩu nhập lại không trùng";
                    return View();
                }

                if (customer.PasswordCus == "" || customer.PasswordCus == null)
                {
                    customer.PasswordCus = PrevPassword;
                }
                database.Entry(customer).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThongKe(int? month, int? year, bool? iframe)
        {
            var s = ThongKeNgay.UseDB(database);
            var ss = ThongKeThang.UseDB(database, month, year);
            var sss = ThongKeNam.UseDB(database, year);

            if (iframe.HasValue)
            {
                ViewBag.iframe = iframe.Value;
            }
            if (month.HasValue)
            {
                ViewBag.Month = month.Value;
            }

            if (year.HasValue)
            {
                ViewBag.Year = year.Value;
            }

            return View(new Tuple<ThongKeNgay, ThongKeThang, ThongKeNam>(s, ss, sss));
        }
    }
}