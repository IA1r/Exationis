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
    public class CacheManager
    {
        public static FacultyDto[] Faculties
        {
            get
            {
                return (FacultyDto[])HttpContext.Current.Cache[CacheKey.Faculties];
            }

            set
            {
                HttpContext.Current.Cache[CacheKey.Faculties] = value;
            }
        }
    }
}
