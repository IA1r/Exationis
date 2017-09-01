using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Exationis.RequestModels;
using Core.Dto;
using BusinessModel.Managers;
using System.Web;
using Core.Common;
using Exationis.ResponseModels;
using System.Threading.Tasks;
using System.IO;
using Core.Helpers;


namespace Exationis.Controllers
{
    public class AccountAPIController : ApiController
    {
        private UserManager userManager;
        private FacultyManager facultyManager;
        public AccountAPIController()
        {
            this.userManager = new UserManager();
            this.facultyManager = new FacultyManager();
        }

        [HttpGet]
        public HttpResponseMessage GetFacultyList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.facultyManager.GetFacultyList());
        }

        [HttpPost]
        public HttpResponseMessage Registration(RegisterRequestModel user)
        {
            try
            {
                this.userManager.Registration(new UserDto
                {
                    Login = user.Login,
                    Name = user.Name,
                    SurName = user.SurName,
                    MiddleName = user.MiddleName,
                    Email = user.Email,
                    Password = user.Password,
                    ConfirmPassword = user.ConfirmPassword,
                    Group = user.Group,
                    FacultyID = user.FacultyID
                }, "Exationis");

                this.userManager.SignIn(new UserDto
                {
                    Login = user.Login,
                    Password = user.Password,
                    RememberMe = false
                }, false);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage IsLoginExist(UserDto user)
        {
            if (!this.userManager.IsLoginExist(user.Login))
                return Request.CreateResponse(HttpStatusCode.OK, "");
            return Request.CreateErrorResponse(HttpStatusCode.Conflict, "This Login alreade used.");
        }

        [HttpPost]
        public HttpResponseMessage IsEmailExist(UserDto user)
        {
            if (!this.userManager.IsEmailExist(user.Email))
                return Request.CreateResponse(HttpStatusCode.OK, "");
            return Request.CreateErrorResponse(HttpStatusCode.Conflict, "This Email alreade used.");
        }

        [HttpPost]
        public HttpResponseMessage SignIn(SignInRequestModel user)
        {
            try
            {
                this.userManager.SignIn(new UserDto
                {
                    Login = user.Login,
                    Password = user.Password,
                    RememberMe = user.RememberMe
                }, false);

                if (!string.IsNullOrEmpty(this.Request.Headers.Referrer.Query) && this.Request.Headers.Referrer.Query.Contains("ReturnUrl"))
                {
                    string query = this.Request.Headers.Referrer.Query;
                    string returnUrl = query.Replace("%2f", "/");
                    returnUrl = returnUrl.Substring(returnUrl.IndexOf('/'));

                    return Request.CreateResponse(HttpStatusCode.OK, returnUrl);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserPrfile(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, this.userManager.GetUserProfile(id));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> AvatarUpload(int id)
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
                    string avatarID = ImageHelper.UploadImage(stream, fileName);
                    await this.userManager.AvatarUpload(id, avatarID);
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

        [HttpPost]
        public HttpResponseMessage EditUserProfile(EditProfileRequestModel request)
        {
            try
            {
                this.userManager.EditUserProfile(new UserDto
                {
                    ID = request.ID,
                    Name = request.Name,
                    SurName = request.SurName,
                    Login = request.Login,
                    Email = request.Email,
                    Password = request.Password,
                    ConfirmPassword = request.ConfirmPassword
                });
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLanguages()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.userManager.GetLanguages());
        }

        [HttpPost]
        public HttpResponseMessage ChangeLanguage(int id)
        {
            this.userManager.ChangeLanguage(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
