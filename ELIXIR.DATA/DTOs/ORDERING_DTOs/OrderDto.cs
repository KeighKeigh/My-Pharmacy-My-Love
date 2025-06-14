﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.ORDERING_DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderDate { get; set; }
        public int OrderNo { get; set; }
        public string DateNeeded { get; set; }
        public string Farm { get; set; }
        public string FarmCode { get; set; }
        public string FarmType { get; set; }
        public string Category { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Uom{ get; set; }
        public decimal QuantityOrder { get; set; }
        public string PreparedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrepared { get; set; }
        public decimal StockOnHand { get; set; }
        public decimal TotalOrders { get; set; }
        public decimal Difference { get; set; }
        public string CancelDate { get; set; }
        public string CancelBy { get; set; }
        public decimal PreparedQuantity { get; set; }
        public string PlateNumber { get; set; }
        public bool IsMove { get; set; }
        public bool IsReject { get; set; }
        public string Remarks { get; set; }
        public int OrderNoPKey { get; set; }
        public string DeliveryStatus { get; set; }

        public string OrderRemarks { get; set; }

        public int Days { get; set; }

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
