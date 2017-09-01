using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.RequestModels
{
    public class DisciplineRequestModel
    {
        public string Name { get; set; }
        public int Faculty { get; set; }
        public int Course { get; set; }
    }
}