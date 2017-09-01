using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.RequestModels
{
    public class ImageRequestModels
    {
        public int QuestionID { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}