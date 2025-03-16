using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace anhemtoicodeweb.Controllers
{
    public class LoginController : Controller
    {
        private readonly Model1 database = new Model1();

        // GET: LoginUser
        public ActionResult Index(string returnUrl)
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Profile");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(User _user, string ReturnUrl)
        {
            var check = database.Users.Where(s => s.Name == _user.Name && s.Password == _user.Password).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Thông tin đăng nhập bị sai. Vui lòng kiểm tra";
                return View("Index");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["UserId"] = check.ID;
                Session["UserName"] = check.Name;
                Session["UserRole"] = check.Role;
                if (!ReturnUrl.IsNullOrWhiteSpace())
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult RegisterUser(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = database.Users.Where(s => s.ID == _user.ID || s.Name == _user.Name).FirstOrDefault();
                if (check == null)
                {
                    if (_user.ConfirmPassword != _user.Password)
                    {
                        ViewBag.ErrorRegister = "Password nhập lại không đúng.";
                        return View();
                    }
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.Users.Add(_user);
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
            return RedirectToAction("Index", "Home");
        }
    }
}