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
            var u = db.Users.FirstOrDefault(x => x.UserMail == _user.email && x.UserPassword == _user.password);

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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User _user)
        {
            if (String.IsNullOrEmpty(_user.email) || String.IsNullOrEmpty(_user.name) || String.IsNullOrEmpty(_user.password))
            {
                ViewBag.Message = "Lütfen istenilen bilgileri giriniz.";
            }
            else
            {
                var u = db.Users.FirstOrDefault(x => x.UserMail == _user.email);

                if (u != null)
                {
                    // Mail already exists in database.
                    ViewBag.Message = "Kullanıcı zaten mevcut.";
                }
                else
                {
                    Users newUser = new Users() { UserMail = _user.email, UserName = _user.name, UserPassword = _user.password };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

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