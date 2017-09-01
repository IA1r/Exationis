using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.RequestModels
{
    public class StudentFilterRequestModel
    {
        public int TestID { get; set; }
        public string Group { get; set; }
    }
}