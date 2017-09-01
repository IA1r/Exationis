using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.ResponseModels
{
    public class ChartResponseModel
    {
        public ChartOneGroupDto[] GroupsChartData { get; set; }
        public ChartOneGroupDto GroupChartData { get; set; }
    }
}