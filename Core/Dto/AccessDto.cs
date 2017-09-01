using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class AccessDto
    {
        public int UserID { get; set; }
        public int TestID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Group { get; set; }
        public bool Access { get; set; }
    }
}
