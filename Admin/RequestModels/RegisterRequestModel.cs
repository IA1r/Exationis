using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exationis.RequestModels
{
	public class RegisterRequestModel
	{
		[Required]
		public string Login { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string SurName { get; set; }
		public string MiddleName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Group { get; set; }
        public int FacultyID { get; set; }
        public string KeyWord { get; set; }

	}
}