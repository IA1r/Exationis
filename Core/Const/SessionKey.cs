using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Const
{
	public static class SessionKey
	{
		public const string LoggedInUser = "LOGGED_IN_USER";
        public const string Questions = "QUESTIONS";
        public const string CurrentTime = "CURRENT_TIME";
        public const string Limitation = "LIMITATION";
	}

    public static class CacheKey
    {
        public const string Faculties = "FACULTIES";
    }
}
