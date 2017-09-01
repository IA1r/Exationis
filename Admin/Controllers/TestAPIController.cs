using Admin.RequestModels;
using Admin.RequestModels.TestAPI;
using BusinessModel.Managers;
using Core.Dto;
using Core.Helpers;
using Exationis.RequestModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Admin.Controllers
{
    public class TestAPIController : ApiController
    {
        private TestManager testManager;
        private UserManager userManager;

        public TestAPIController()
        {
            this.testManager = new TestManager();
            this.userManager = new UserManager();
        }

        [HttpPost]
        public HttpResponseMessage CreateTest(TestDto test)
        {
            this.testManager.CreateTest(test.DisciplineID, test);

            string redirect = this.Url.Link("Default", new
            {
                Action = "Test",
                id = this.testManager.GetTestID(test.DisciplineID, test.Name)
            });
            return Request.CreateResponse(HttpStatusCode.OK, new { testPage = redirect });
        }

        [HttpPost]
        public IHttpActionResult AddQuestion(TestContentRequestModel question)
        {
            AnswerDto[] answer = new AnswerDto[4];

            for (int i = 0; i < answer.Length; i++)
            {
                answer[i] = new AnswerDto();
                answer[i].Content = question.Answers[i];
                answer[i].Correct = question.Correct[i];
            }

            this.testManager.AddQuestion(new QuestionDto
            {
                TestID = question.TestID,
                Content = question.Question,
                Answer = answer,
                Points = question.Points
            });

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult EditQuestion(EditQuestionRequestModel newQuestion)
        {
            this.testManager.EditQuestion(new QuestionDto
            {
                ID = newQuestion.ID,
                TestID = newQuestion.TestID,
                Content = newQuestion.Content,
                Answer = newQuestion.Answer,
                Points = newQuestion.Points
            });

            return Ok();
        }

        [HttpGet]
        public HttpResponseMessage GetQuestions(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.testManager.GetContent(id, false));
        }

        [HttpGet]
        public HttpResponseMessage GetTest(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, this.testManager.GetTest(id));
            }
            catch (NullReferenceException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }

        }

        //[HttpGet]
        //public TestDto[] GetTests(int id)
        //{
        //    return this.testManager.GetTests(id);
        //}

        [HttpPost]
        public HttpResponseMessage IsTestExist(TestDto test)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, this.testManager.IsTestExist(test.DisciplineID, test.Name));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message);
            }

        }

        [HttpPost]
        public HttpResponseMessage GetStudents(AccessRequestModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.userManager.GetStudents(model.TestID, model.Group));
        }

        [HttpPost]
        public HttpResponseMessage SetAccess(AccessDto[] model)
        {
            this.userManager.SetAccess(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IHttpActionResult> ImageUpload(int id)
        {
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                byte[] fileArray = await file.ReadAsByteArrayAsync();
                MemoryStream stream = new MemoryStream(fileArray);

                try
                {
                    //await fs.WriteAsync(fileArray, 0, fileArray.Length);
                   string imageID = ImageHelper.UploadImage(stream, fileName);
                   await this.testManager.UploadImage(id, imageID);
                }
                catch (IOException ex)
                {
                    return InternalServerError(ex);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetImages(int id)
        {
            ImageDto[] images = await this.testManager.GetImages(id);

            return Ok(images);
        }

        [HttpPost]
        public HttpResponseMessage DeleteImage(string id)
        {
            ImageHelper.DeleteFile(id);
            this.testManager.DeleteImage(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public IHttpActionResult EditTest(TestTitleRequestModel test)
        {
            this.testManager.EditTest(new TestDto
            {
                ID = test.ID,
                Name = test.Name,
                Limitation = test.Limitation
            });

            return Ok();
        }
    }
}
