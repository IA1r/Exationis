using BusinessModel.Managers;
using Core.Common;
using Core.Dto;
using Core.Helpers;
using Exationis.RequestModels;
using Exationis.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Exationis.Controllers
{
    public class TestAPIController : ApiController
    {
        private TestManager testManager;
        private DisciplineManager disciplineManager;
        private UserManager userManager;

        public TestAPIController()
        {
            this.testManager = new TestManager();
            this.disciplineManager = new DisciplineManager();
            this.userManager = new UserManager();
        }

        [HttpGet]
        public HttpResponseMessage GetQuestions(int id)
        {
            int resttime = 0;

            if (SessionManager.CurrentTime != DateTime.MinValue)
                resttime = Convert.ToInt32(TimeSpan.Parse(SessionManager.Limitation).TotalMilliseconds - DateTime.Now.Subtract(SessionManager.CurrentTime).TotalMilliseconds);
            else
            {
                SessionManager.CurrentTime = DateTime.Now;
                SessionManager.Limitation = this.testManager.GetTestLimitation(id);
            }
            if (SessionManager.Questions != null)
            {
                QuestionDto[] questions = SessionManager.Questions as QuestionDto[];
                if (questions[0].TestID == id)
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new TestingResponseModel { Questions = questions, Time = resttime });
                else
                    if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "At the moment you pass the " + questions[0].TestName + " test. Please complete it and be able to start a new one.");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "На даний момент ви проходите" + questions[0].TestName + "тест. Будь ласка, заповніть його і буду змога почати новий.");
            }
            else
                SessionManager.Questions = this.testManager.GetContent(id, true);

            return Request.CreateResponse(HttpStatusCode.OK,
                new TestingResponseModel { Questions = SessionManager.Questions, Time = Convert.ToInt32(TimeSpan.Parse(SessionManager.Limitation).TotalMilliseconds) });
        }

        [HttpGet]
        public HttpResponseMessage GetTest(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new TestingResponseModel { Test = this.testManager.GetTest(id) });
            }
            catch (NullReferenceException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPost]
        public void UpdateResult(QuestionDto[] qestions)
        {
            SessionManager.Questions = qestions;
        }

        [HttpPost]
        public HttpResponseMessage SendResult(QuestionDto[] result)
        {
            this.testManager.SendResult(result);
            this.userManager.RemoveAccess(SessionManager.LoggedInUser.ID, result[0].TestID);

            SessionManager.CurrentTime = DateTime.MinValue;
            SessionManager.Limitation = null;
            SessionManager.Questions = null;

            return Request.CreateResponse(HttpStatusCode.OK, "Your answers successfully sended to the server. Results are available in your profile");
        }


        [HttpGet]
        public HttpResponseMessage GetDiscipline(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, this.disciplineManager.GetDiscipline(id));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTests(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, this.testManager.GetTests(id));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetImages(int id)
        {
            ImageDto[] images = await this.testManager.GetImages(id);

            return Ok(images);
        }
    }
}
