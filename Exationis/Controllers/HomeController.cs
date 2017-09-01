using BusinessModel.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;
using Core.Common;

namespace Exationis.Controllers
{
    public class HomeController : Controller
    {
        private DisciplineManager disciplineManager;

        public HomeController()
        {
            this.disciplineManager = new DisciplineManager();
        }

        [HttpGet]
        public ActionResult Faculty()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Discipline(int id)
        {
            return View(id);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Test(int id)
        {
            return View(id);
        }
    }
}
