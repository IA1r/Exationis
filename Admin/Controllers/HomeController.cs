using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Disciplines()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Test(int id)
        {
            return View(id);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Courses()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Result()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult StudentsResults(int id)
        {
            return View(id);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Charts()
        {
            return View();
        }

    }
}
