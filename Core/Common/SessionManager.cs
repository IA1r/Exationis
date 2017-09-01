using Core.Const;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Common
{
    public static class SessionManager
    {
        public static UserDto LoggedInUser
        {
            get
            {
                return (UserDto)HttpContext.Current.Session[SessionKey.LoggedInUser];
            }

            set
            {
                HttpContext.Current.Session[SessionKey.LoggedInUser] = value;
            }
        }

        public static QuestionDto[] Questions
        {
            get
            {
                return (QuestionDto[])HttpContext.Current.Session[SessionKey.Questions];
            }
            set
            {
                HttpContext.Current.Session[SessionKey.Questions] = value;
            }
        }

        public static DateTime CurrentTime
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.CurrentTime] != null)
                    return (DateTime)HttpContext.Current.Session[SessionKey.CurrentTime];

                return DateTime.MinValue;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.CurrentTime] = value;
            }
        }

        public static string Limitation
        {
            get
            {
                return (string)HttpContext.Current.Session[SessionKey.Limitation];
            }
            set
            {
                HttpContext.Current.Session[SessionKey.Limitation] = value;
            }
        }

        public static int Expire
        {
            set
            {
                HttpContext.Current.Session.Timeout = value;
            }
        }

    }
}
