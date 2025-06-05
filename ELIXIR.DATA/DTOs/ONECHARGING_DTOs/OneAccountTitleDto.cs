﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.ONECHARGING_DTOs
{
    public class OneAccountTitleDto
    {
        public string code { get; set; }
        public string accountTitleCode { get; set; }
        public string accountTitleName { get; set; }
        public int? syncId { get; set; }
        public string delete { get; set; }
        public bool? isActive { get; set; }
    }
}
