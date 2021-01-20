using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Terelelli.Models.Entity;

namespace Terelelli.Controllers
{
    public class HomeController : Controller
    {
        TaskBoardEntities7 db = new TaskBoardEntities7();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User _user)
        {
            var u = db.Users.FirstOrDefault(x => x.UserMail == _user.username && x.UserPassword == _user.password);

            if (u != null)
            {
                // Login
                FormsAuthentication.SetAuthCookie(u.UserName, false);
                return RedirectToAction("Profil");
            }
            else
            {
                // Incorrect username or password
            }

            return View();
        }

        [Authorize]
        public ActionResult Profil()
        {
            return View();
        }

        public ActionResult Kayıt()
        {
            return View();
        }
        public ActionResult Proje()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}