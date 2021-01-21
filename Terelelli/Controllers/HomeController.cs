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
        public ActionResult Index(Users _user)
        {
            var u = db.Users.FirstOrDefault(x => x.UserMail == _user.UserMail && x.UserPassword == _user.UserPassword);

            if (u != null)
            {
                // Login
                FormsAuthentication.SetAuthCookie(u.UserId.ToString(), false);
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
        public ActionResult Register(Users _user)
        {
            if (String.IsNullOrEmpty(_user.UserMail) || String.IsNullOrEmpty(_user.UserName) || String.IsNullOrEmpty(_user.UserPassword))
            {
                ViewBag.Message = "Lütfen istenilen bilgileri giriniz.";
            }
            else
            {
                var u = db.Users.FirstOrDefault(x => x.UserMail == _user.UserMail);

                if (u != null)
                {
                    // Mail already exists in database.
                    ViewBag.Message = "Kullanıcı zaten mevcut.";
                }
                else
                {
                    Users newUser = new Users() { UserMail = _user.UserMail, UserName = _user.UserName, UserPassword = _user.UserPassword};
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult Proje(int? id)
        {
            if (id == null)
                return RedirectToAction("Profil");

            // if user does not exist in this project, return to profile screen.
            var p = db.ProjectUsers.FirstOrDefault(x => x.ProjectId == id && x.UserId.ToString() == User.Identity.Name);
            if (p == null)
                return RedirectToAction("Profil");

            ViewBag.text = id.ToString();
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

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateProject(ProfilePage _profilePage)
        {
            _profilePage.projects.ProjectAuthorId = db.Users.FirstOrDefault(x => x.UserId.ToString() == User.Identity.Name).UserId;
            _profilePage.projects.ProjectStartDate = DateTime.Now;
            db.Projects.Add(_profilePage.projects);
            db.SaveChanges();

            ProjectUsers PU = new ProjectUsers() { ProjectId = _profilePage.projects.ProjectId, UserId = Convert.ToInt32(User.Identity.Name) };
            db.ProjectUsers.Add(PU);
            db.SaveChanges();

            return RedirectToAction("Profil");
        }

        [Authorize]
        [HttpPost]
        public ActionResult JoinProject(ProfilePage _profilePage)
        {
            var p = db.Projects.FirstOrDefault(x => x.ProjectId == _profilePage.projects.ProjectId);
            
            // if project exists => add the user to the projectusers relationship table
            if (p != null)
            {
                // if user is not a nember of this project, add them
                var pu = db.ProjectUsers.FirstOrDefault(x => x.ProjectId == _profilePage.projects.ProjectId && x.UserId.ToString() == User.Identity.Name);

                if (pu == null)
                {
                    pu = new ProjectUsers() { ProjectId = _profilePage.projects.ProjectId, UserId = Convert.ToInt32(User.Identity.Name) };
                    db.ProjectUsers.Add(pu);
                    db.SaveChanges();
                }

                // then redirect the user to the project page.
                return RedirectToAction("Proje");
            }
            return RedirectToAction("Profil");
        }
    }
}