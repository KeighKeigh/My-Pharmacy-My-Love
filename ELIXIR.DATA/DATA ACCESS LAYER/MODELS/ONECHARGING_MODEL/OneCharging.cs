﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.ONECHARGING_MODEL
{
    public class OneCharging
    {
        public int? Id { get; set; }
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
        public string deleted_at { get; set; }
        public DateTime? DateAdded { get; set; } = DateTime.Now;

        public bool? IsActive { get; set; }
    }
}
