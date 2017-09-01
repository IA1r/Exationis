using BusinessModel.Managers;
using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Controllers
{
    public class AccountController : Controller
    {
        private FacultyManager facultyManager;
        private UserManager userManager;

        public AccountController()
        {
            facultyManager = new FacultyManager();
            userManager = new UserManager();
        }

        public ActionResult Registration()
        {

            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            SessionManager.LoggedInUser = null;

            return RedirectToAction("Disciplines", "Home", null);
        }

    }
}
