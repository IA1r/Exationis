using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
	public class UserDto
	{
		public int ID { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public string Avatar { get; set; }
		public string Name { get; set; }
		public string SurName { get; set; }
		public string MiddleName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Group { get; set; }
		public string Role { get; set; }
		public int FacultyID { get; set; }
		public string Faculty { get; set; }
		public bool RememberMe { get; set; }
		public LanguageDto Language { get; set; }
		public string KeyWord { get; set; }

		public ShortResultDto[] Results { get; set; }
		//need!
		public bool Access { get; set; }

	}
}
