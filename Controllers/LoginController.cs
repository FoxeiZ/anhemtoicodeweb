using anhemtoicodeweb.Context;
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
        readonly ProductManagementContext database = new ProductManagementContext();

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

        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAccount(User _user)
        {
            var check = database.Users.Where(s => s.UserName == _user.UserName && s.Password == _user.Password).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "SaiInfo";
                return View("Index");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["UserId"] = _user.UserId;
                Session["Password"] = _user.Password;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult RegisterUser() 
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RegisterUser(User _user)
        {
            if (ModelState.IsValid)
            {
                var check_ID = database.Users.Where(s => s.UserId == _user.UserId).FirstOrDefault();
                if (check_ID == null)
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.Users.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exist";
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