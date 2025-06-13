using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.MISCELLANEOUS_DTOs
{
    public class MReceiptDto
    {

        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalQuantity { get; set; }
        public string PreparedDate { get; set; }
        public string AddedBy { get; set; }
        public string Remarks { get; set; }
        public string PreparedBy { get; set; }
        public bool IsActive { get; set; }
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
