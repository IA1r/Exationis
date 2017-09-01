using Admin.RequestModels.ResultAPI;
using BusinessModel.Managers;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Admin.ResponseModels;

namespace Admin.Controllers
{
    public class ResultAPIController : ApiController
    {
        private DisciplineManager disciplineManager;
        private TestManager testManager;
        private UserManager userManager;

        public ResultAPIController()
        {
            this.disciplineManager = new DisciplineManager();
            this.testManager = new TestManager();
            this.userManager = new UserManager();
        }

        [HttpGet]
        public HttpResponseMessage GetActualDisciplines()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.disciplineManager.GetActualDisciplines());
        }

        [HttpGet]
        public HttpResponseMessage GetTests(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, this.testManager.GetActualTests(id));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }        
        }

        [HttpGet]
        public HttpResponseMessage GetShortResult(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.testManager.GetShortResult(id));
        }

        [HttpGet]
        public HttpResponseMessage GetFullResult(int testID, int userID)
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.testManager.GetFullResult(testID, userID));
        }

        [HttpGet]
        public HttpResponseMessage GetDisciplines()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.disciplineManager.GetDisciplines());
        }

        [HttpGet]
        public HttpResponseMessage GetGroups()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.userManager.GetGroups());
        }

        [HttpPost]
        public IHttpActionResult DrawCharts(DrawChartRequestModel model)
        {
            ChartResponseModel response = new ChartResponseModel();
            if (model.OneOrMany == 1)
                response.GroupChartData = this.testManager.GetChartData(model.TestID, model.Group);
            if (model.OneOrMany == 2)
                response.GroupsChartData = this.testManager.GetChartData(model.TestID, model.Groups);

            return Ok(response);
        }
    }
}
