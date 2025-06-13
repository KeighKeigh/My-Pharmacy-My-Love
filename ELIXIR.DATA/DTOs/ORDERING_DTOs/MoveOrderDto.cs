using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.ORDERING_DTOs
{
    public class MoveOrderDto
    {

        public int Id { get; set; }
        public int OrderNo { get; set; }
        public string FarmCode { get; set; }
        public string FarmName { get; set; }
        public string FarmType { get; set; }

        public string Category { get; set; }
        public string Uom { get; set; }
        public string OrderDate { get; set; }
        public string DateNeeded { get; set; }
        public string PreparedDate { get; set; }
        public int BarcodeNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public decimal Quantity { get; set; }
        public string Expiration { get; set; }
        public bool IsActive { get; set; }

        public string PlateNumber { get; set; }

        public bool IsReject { get; set; }
        public bool IsApprove { get; set; }
        public bool IsPrepared { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectedDate { get; set; }
        public string Remarks { get; set; }

        public int OrderNoPKey { get; set; }
        public bool IsPrint { get; set; }
        public bool IsTransact { get; set; }

        public string DeliveryStatus { get; set; }

        public string BatchNo { get; set; }

        public string OrderRemarks { get; set; }

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
