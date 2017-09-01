using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ShortResultDto
    {
        public int UserID { get; set; }
        public int TestID { get; set; }
        public string TestName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public double Points { get; set; }
        public double Evaluation { get; set; }
        public DateTime Date { get; set; }
    }
}
