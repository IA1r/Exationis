using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exationis.RequestModels
{
    public class TestContentRequestModel
    {
        public int TestID { get; set; }
        public string Question { get; set; }
        public string[] Answers { get; set; }
        public bool[] Correct { get; set; }
        public double Points { get; set; }
    }
}