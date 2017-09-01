using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ChartOneGroupDto
    {
        public double TestEvaluation { get; set; }
        public string Group { get; set; }
        public double Avarage { get; set; }
        public double Percent { get; set; }
        public StudentChartData[] StudentsData { get; set; }
    }
}
