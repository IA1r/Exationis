using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class FacultyDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Group { get; set; }
        public  DisciplineDto[] Disciplines { get; set; }

    }
}
