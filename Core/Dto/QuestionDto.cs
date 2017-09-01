using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class QuestionDto
    {
        public int ID { get; set; }
        public int TestID { get; set; }
        public string TestName { get; set; }
        public string Content { get; set; }
        public AnswerDto[] Answer { get; set; }
        public double Points { get; set; }
    }
}
