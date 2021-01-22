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

            var users = db.ProjectUsers.Where(x => x.ProjectId == id).Select(x => x.Users).ToList();

            ViewBag.projectUsers = new SelectList(users, "UserId", "UserName");
            ViewBag.projectId = id.ToString();
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

            // Add 5 default panels to the project.
            string[] panelNames = { "Todo", "In Progress", "Revision", "Check", "Done" };
            for (int i = 0; i < 5; i++)
            {
                Panels P = new Panels()
                {
                    ProjectId = _profilePage.projects.ProjectId,
                    PanelAuthorId = Convert.ToInt32(User.Identity.Name),
                    PanelName = panelNames[i]
                };
                db.Panels.Add(P);
                db.SaveChanges();
            }

            return RedirectToAction("Profil");
        }

        [Authorize]
        public ActionResult DeleteProject(int _projectId)
        {
            Projects p = db.Projects.FirstOrDefault(x => x.ProjectId == _projectId);
            db.Projects.Remove(p);
            db.SaveChanges();

            return RedirectToAction("Profil");
        }

        [Authorize]
        [HttpPost]
        public ActionResult JoinProject(ProfilePage _profilePage)
        {
            var p = db.Projects.FirstOrDefault(x => x.ProjectId == _profilePage.projects.ProjectId);
            
            // if project exists => add the user to the projectusers table
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

        [Authorize]
        [HttpPost]
        public ActionResult CreatePanel(ProjectPage _projectPage)
        {
            Panels newPanel = new Panels() { ProjectId = _projectPage.projects.ProjectId, PanelAuthorId = Convert.ToInt32(User.Identity.Name), PanelName = _projectPage.panels.PanelName };
            db.Panels.Add(newPanel);
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = _projectPage.projects.ProjectId });
        }

        [Authorize]
        public ActionResult DeletePanel(int _panelId)
        {
            int? projectId = db.Panels.FirstOrDefault(x => x.PanelId == _panelId).ProjectId;

            db.Panels.Remove(db.Panels.FirstOrDefault(x => x.PanelId == _panelId));
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = projectId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateTask(ProjectPage _projectPage)
        {
            Panels panel = db.Panels.FirstOrDefault(x => x.ProjectId == _projectPage.projects.ProjectId);

            if (panel == null)
                return RedirectToAction("Proje", new { id = _projectPage.projects.ProjectId });

            // Calculate estimated time
            var completedProjects = db.TaskCompletionTimes.FirstOrDefault(x => x.UserId == _projectPage.tasks.UserId);

            if (completedProjects != null)
            {
                _projectPage.tasks.TaskEstimatedDuration = (long)db.TaskCompletionTimes.Average(x => x.CompletionTime);
            }
            else
            {
                _projectPage.tasks.TaskEstimatedDuration = 24 * 60;
            }

            Tasks newTask = new Tasks()
            {
                PanelId = panel.PanelId,
                UserId = _projectPage.tasks.UserId,
                TaskStartDate = DateTime.Now,
                TaskDescription = _projectPage.tasks.TaskDescription,
                TaskNotes = _projectPage.tasks.TaskNotes,
                TaskEstimatedDuration = _projectPage.tasks.TaskEstimatedDuration
            };
            db.Tasks.Add(newTask);
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = _projectPage.projects.ProjectId });
        }

        [Authorize]
        public ActionResult DeleteTask(int _taskId)
        {
            int? panelId = db.Tasks.FirstOrDefault(x => x.TaskId == _taskId).PanelId;
            int? projectId = db.Panels.FirstOrDefault(x => x.PanelId == panelId).ProjectId;

            db.Tasks.Remove(db.Tasks.FirstOrDefault(x => x.TaskId == _taskId));
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = projectId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateTask(ProjectPage _projectPage)
        {
            var t = db.Tasks.FirstOrDefault(x => x.TaskId == _projectPage.tasks.TaskId);
            t.UserId = _projectPage.tasks.UserId;
            t.TaskDescription = String.IsNullOrEmpty(_projectPage.tasks.TaskDescription) ? t.TaskDescription : _projectPage.tasks.TaskDescription;
            t.TaskNotes = String.IsNullOrEmpty(_projectPage.tasks.TaskNotes) ? t.TaskNotes : _projectPage.tasks.TaskNotes;
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = _projectPage.projects.ProjectId });
        }

        [Authorize]
        public ActionResult FinishTask(int _projectId, int _taskId)
        {
            Tasks t = db.Tasks.FirstOrDefault(x => x.TaskId == _taskId);
            t.TaskFinishDate = DateTime.Now;
            db.SaveChanges();

            TaskCompletionTimes tct = new TaskCompletionTimes()
            {
                TaskId = t.TaskId,
                UserId = t.UserId,
                CompletionTime = (t.TaskFinishDate - t.TaskStartDate).Value.Minutes
            };
            db.TaskCompletionTimes.Add(tct);
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = _projectId });
        }

        [Authorize]
        public ActionResult UnfinishTask(int _projectId, int _taskId)
        {
            Tasks t = db.Tasks.FirstOrDefault(x => x.TaskId == _taskId);
            t.TaskFinishDate = null;
            db.SaveChanges();

            TaskCompletionTimes tct = db.TaskCompletionTimes.FirstOrDefault(x => x.TaskId == _taskId);
            db.TaskCompletionTimes.Remove(tct);
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = _projectId });
        }

        [Authorize]
        public ActionResult MoveTask(int _taskId, int _panelId)
        {
            int? projectId = db.Panels.FirstOrDefault(x => x.PanelId == _panelId).ProjectId;

            // move task to the new panel
            db.Tasks.FirstOrDefault(x => x.TaskId == _taskId).PanelId = _panelId;
            db.SaveChanges();

            return RedirectToAction("Proje", new { id = projectId });
        }
    }
}