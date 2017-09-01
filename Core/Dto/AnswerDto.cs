using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class AnswerDto
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string Content { get; set; }
        public bool Correct { get; set; }


        //results field for students, where they determine the correct answer
        public bool Result { get; set; }
    }
}
