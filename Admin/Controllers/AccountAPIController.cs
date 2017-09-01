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


namespace Admin.Controllers
{
    public class AccountAPIController : ApiController
    {
        private UserManager userManager;
        public AccountAPIController()
        {
            this.userManager = new UserManager();
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
                    FacultyID = user.FacultyID,
                    KeyWord = user.KeyWord
                }, "Admin");

                this.userManager.SignIn(new UserDto
                {
                    Login = user.Login,
                    Password = user.Password,
                    RememberMe = false
                }, true);
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
                }, true);

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

        //[HttpPost]
        //public void SignOut()
        //{
        //    this.userManager.SignOut();
        //}

        [HttpGet]
        public bool IsLogged()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }


        //[HttpPost]
        //public UserAccessDto[] GetUsersRequestAccessList()
        //{
        //    return this.userManager.GetUsersRequestAccessList();
        //}

        //[HttpPost]
        //public void ApproveAccess(UserAccessDto[] usersRequest)
        //{
        //    foreach (UserAccessDto userRequest in usersRequest)
        //    {
        //        (HttpContext.Current.Application["UserAccess_" + userRequest.UserID.ToString()] as UserAccessDto).TestAccess = userRequest.TestAccess;
        //    }
        //}


    }
}
