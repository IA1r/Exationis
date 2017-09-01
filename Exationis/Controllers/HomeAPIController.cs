using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessModel.Managers;
using Core.Common;

namespace Exationis.Controllers
{
    public class HomeAPIController : ApiController
    {
        private FacultyManager facultyManager;
        private DisciplineManager disciplineManager;
        public HomeAPIController()
        {
            this.facultyManager = new FacultyManager();
            this.disciplineManager = new DisciplineManager();
        }

        [HttpGet]
        public IHttpActionResult GetFaculties()
        {
            FacultyDto[] faculties;
            if (CacheManager.Faculties != null)
                faculties = CacheManager.Faculties;
            else faculties = this.facultyManager.GetFaculties();

            return Ok(faculties);
        }

        [HttpPost]
        public IHttpActionResult SearchDiscipline(string id)
        {
            //id - stores the key, which is searched.
            return Ok(this.disciplineManager.SearchDiscipline(id));  
        }
    }
}
