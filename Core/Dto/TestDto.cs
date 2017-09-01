using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class TestDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Limitation { get; set; }
        public double Evaluation { get; set; }
        public int DisciplineID { get; set; }
        public double Points { get; set; }
        public bool EditAccess { get; set; }

        public bool Access { get; set; }
    }
}
