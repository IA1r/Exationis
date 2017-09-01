using BusinessModel.Managers;
using Core.Common;
using Exationis.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Exationis.Controllers
{
    public class AccountController : Controller
    {

        [Authorize]
        [HttpGet]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            SessionManager.LoggedInUser = null;
            SessionManager.CurrentTime = DateTime.MinValue;
            SessionManager.Limitation = null;
            SessionManager.Questions = null;
            return RedirectToAction("Faculty", "Home", null);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserProfile(int id)
        {
            return View(id);
        }
    }
}
