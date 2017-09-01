using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exationis.RequestModels
{
	public class EditQuestionRequestModel
	{
		public int ID { get; set; }
		public int TestID { get; set; }
		public string Content { get; set; }
		public AnswerDto[] Answer { get; set; }
	}
}