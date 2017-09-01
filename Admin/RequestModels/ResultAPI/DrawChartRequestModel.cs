using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.RequestModels.ResultAPI
{
    public class DrawChartRequestModel
    {
        public int TestID { get; set; }
        public string Group { get; set; }
        public string[] Groups { get; set; }  
        public int OneOrMany { get; set; }
    }
}