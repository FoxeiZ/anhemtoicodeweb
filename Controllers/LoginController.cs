using anhemtoicodeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class LoginController : Controller
    {
        Model1 database = new Model1();

        // GET: LoginUser
        public ActionResult Index()
        {
            var Id = Session["UserId"];
            if (Id != null)
            {
                return RedirectToAction("UserProfile");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(Customer _user)
        {
            var check = database.Customers.Where(s => s.NameCus == _user.NameCus && s.PasswordCus == _user.PasswordCus).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Thông tin đăng nhập bị sai. Vui lòng kiểm tra";
                return View("Index");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["UserId"] = _user.IDCus;
                Session["NameCus"] = _user.NameCus;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult RegisterUser() 
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RegisterUser(Customer _user)
        {
            if (ModelState.IsValid)
            {
                var check = database.Customers.Where(s => s.IDCus == _user.IDCus || s.NameCus == _user.NameCus).FirstOrDefault();
                if (check == null)
                {
                    if (_user.ConfirmPasswordCus != _user.PasswordCus)
                    {
                        ViewBag.ErrorRegister = "Password nhập lại không đúng.";
                        return View();
                    }
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.Customers.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "Người dùng này đã tồn tại trước đó.";
                    return View();
                }
            }
            return View();
        }

        public ActionResult LogoutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}