using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exationis.RequestModels
{
    public class TestRequestModel
    {
        [Required]
        public string Name { get; set; }
        //(([0-1][0-9]\:[0-5][0-9]\:[0-5][0-9])|([2][0-3]\:[0-5][0-9]\:[0-5][0-9]))
        [RegularExpression(@"(([0-1][0-9])|([2][0-3]))(\:[0-5][0-9]\:[0-5][0-9])", ErrorMessage = "Ivalid Limitation")]
        public string Limitation { get; set; }
        [RegularExpression(@"^[1-9][0-9]?$|^100$", ErrorMessage = "Ivalid Evaluation")]
        public string Evaluation { get; set; }
        public string DisciplineID { get; set; }
    }
}