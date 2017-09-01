using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exationis.RequestModels
{
    public class SignInRequestModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}