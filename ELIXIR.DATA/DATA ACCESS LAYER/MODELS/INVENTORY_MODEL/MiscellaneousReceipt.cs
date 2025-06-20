﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.INVENTORY_MODEL
{
    public class MiscellaneousReceipt : BaseEntity
    {

        public string Supplier { 
            get; 
            set; 
        }

        public string SupplierCode {
            get;
            set;
        }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalQuantity { 
            get;
            set;
        }
        public DateTime PreparedDate { 
            get; 
            set; 
        }
        public string  PreparedBy {
            get;
            set; 
        }

        public string Details
        {
            get;
            set;
        }

        public DateTime? TransactionDate { get; set; }


        public string Remarks { 
            get; 
            set;
        }
        public bool IsActive { 
            get;
            set;
        }

        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }
        public string OneChargingCode { get; set; }
        public string OneChargingName { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string DepartmentUnitCode { get; set; }
        public string DepartmentUnitName { get; set; }
        public string SubUnitCode { get; set; }
        public string SubUnitName { get; set; }
    }
}
