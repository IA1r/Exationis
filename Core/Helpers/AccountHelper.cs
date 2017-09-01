using Core.Common;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Core.Helpers
{
    public static class AccountHelper
    {
        public static bool IsRole(this HtmlHelper html, string role)
        {
            if (SessionManager.LoggedInUser != null)
                if (SessionManager.LoggedInUser.Role == role)
                    return true;
            return false;
        }

        public static bool IsRole(string role)
        {
            if (SessionManager.LoggedInUser != null)
                if (SessionManager.LoggedInUser.Role == role)
                    return true;
            return false;
        }

        public static bool IsApprove(int testID)
        {
            if ((HttpContext.Current.Application["UserAccess_" + SessionManager.LoggedInUser.ID.ToString()] as UserAccessDto).TestAccess[testID])
                return true;
            return false;
        }

        public static bool IsLogIn(this HtmlHelper html)
        {
            if (SessionManager.LoggedInUser != null)
                return true;
            return false;
        }
    }
}
