using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.REPORT_DTOs
{
    public class MoveOrderReport
    {
        public int MoveOrderId { get; set; }
        public int OrderNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string FarmType { get; set; }
        public string FarmCode { get; set; }
        public string FarmName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Uom { get; set; }
        public string Category { get; set; }
        public decimal Quantity { get; set; }
        public string ExpirationDate { get; set; }
        public string TransactionType { get; set; }
        public string MoveOrderBy { get; set; }
        public string MoveOrderDate { get; set; }
        public string PreparedDate { get; set; }
        public string TransactedDate { get; set; }
        public string DateNeeded { get; set; }
        public string TransactedBy { get; set; }
        public string BatchNo { get; set; }
        public string DeliveryDate { get; set; }

        public string OrderRemarks { get; set; }
        public string Remarks { get; set; }


        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string DepartmentUnitCode { get; set; }
        public string DepartmentUnitName { get; set; }
        public string SubUnitCode { get; set; }
        public string SubUnitName { get; set; }
    }
}
