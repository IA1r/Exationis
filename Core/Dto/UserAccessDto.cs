using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class UserAccessDto
    {
        public int UserID { get; set; }
        public int DisciplineID { get; set; }
        public int RequestAccessTestID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Group { get; set; }
        public Dictionary<int, bool> TestAccess { get; set; }
    }
}
