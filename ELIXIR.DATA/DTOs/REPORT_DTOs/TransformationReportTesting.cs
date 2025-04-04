using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.REPORT_DTOs
{
    public class TransformationReportTesting
    {

        public int TransformId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public string Category { get; set; }
        public int Batch { get; set; }
        public int Version { get; set; }
        public string PlanningDate { get; set; }
        public string DateTransformed { get; set; }



    }
}
