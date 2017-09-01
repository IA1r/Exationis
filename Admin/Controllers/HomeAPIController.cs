using Admin.RequestModels;
using BusinessModel.Managers;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Admin.Controllers
{
    public class HomeAPIController : ApiController
    {
        private DisciplineManager disciplineManager;
        private FacultyManager facultyManager;

        public HomeAPIController()
        {
            disciplineManager = new DisciplineManager();
            this.facultyManager = new FacultyManager();
        }

        [HttpGet]
        public HttpResponseMessage GetDisciplines()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.disciplineManager.GetDisciplines());
        }

        [HttpPost]
        public HttpResponseMessage CreateDiscipline(DisciplineRequestModel discipline)
        {
            try
            {
                this.disciplineManager.CreateDiscipline(new DisciplineDto
                {
                    Name = discipline.Name,
                    FacultyID = discipline.Faculty,
                    Course = discipline.Course
                });

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message);
            }          
        }

        [HttpGet]
        public HttpResponseMessage GetFacultyList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.facultyManager.GetFacultyList());
        }
    }
}
