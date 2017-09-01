using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exationis.RequestModels
{
    public class EditProfileRequestModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}