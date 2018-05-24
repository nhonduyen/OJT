using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJT.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string EMP_ID, string Password, string NewPassword, string PwConfirm)
        {
            if (!NewPassword.Equals(PwConfirm))
            {
                return RedirectToAction("ChangePassword", new { success = -1, message = "Password does not match" });
            }
            EMPLOYEE em = new EMPLOYEE();
            Password = em.Encode(Password);
            NewPassword = em.Encode(NewPassword);
            var result = em.ChangePassword(EMP_ID, Password, NewPassword);
            var message = result > 0 ? "Success" : "Fail";
            return RedirectToAction("ChangePassword", new { success = result, message = message });
        }
      
        [HttpPost]
        public ActionResult Signin(string username, string password)
        {
            EMPLOYEE em = new EMPLOYEE();
            password = em.Encode(password);
            bool login = em.Login(username, password);
            if (login)
                return RedirectToAction("Index");
            TempData["alert"] = "Login fail. Please check you ID and Password.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

    }
}
