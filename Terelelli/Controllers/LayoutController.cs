using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Terelelli.Models.Entity;

namespace Terelelli.Controllers
{
    public class LayoutController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                TaskBoardEntities7 db = new TaskBoardEntities7();
                Users loggedUser = db.Users.FirstOrDefault(x => x.UserId.ToString() == User.Identity.Name);

                TempData["Username"] = loggedUser.UserName;
            }

            return View();
        }
    }
}