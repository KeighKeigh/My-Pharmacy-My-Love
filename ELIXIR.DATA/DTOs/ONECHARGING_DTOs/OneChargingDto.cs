﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.ONECHARGING_DTOs
{
    public class OneChargingDto
    {
        public int? sync_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string company_code { get; set; }
        public string company_name { get; set; }
        public int? company_id { get; set; }
        public int? business_unit_id { get; set; }
        public string business_unit_code { get; set; }
        public string business_unit_name { get; set; }
        public string department_code { get; set; }
        public string department_name { get; set; }
        public string department_id { get; set; }
        public int? department_unit_id { get; set; }
        public string department_unit_code { get; set; }
        public string department_unit_name { get; set; }
        public int? sub_unit_id { get; set; }
        public string sub_unit_code { get; set; }
        public string sub_unit_name { get; set; }
        public string location_code { get; set; }
        public string location_name { get; set; }
        public int? location_id { get; set; }
        public DateTime? deleted_at { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? added_at { get; set; }
    }
}
