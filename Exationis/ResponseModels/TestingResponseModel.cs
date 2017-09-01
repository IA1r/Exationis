using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exationis.ResponseModels
{
    public class TestingResponseModel
    {
        public TestDto Test { get; set; }
        public QuestionDto[] Questions { get; set; }
        public int Time { get; set; }
    }
}