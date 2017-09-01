using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class DisciplineDto
    {
        public int ID { get; set; }
        public int FacultyID { get; set; }
        public string Faculty { get; set; }
        public string Head { get; set; }
        public string Name { get; set; }
        public int Course { get; set; }

        public TestDto[] Tests { get; set; }
    }
}
