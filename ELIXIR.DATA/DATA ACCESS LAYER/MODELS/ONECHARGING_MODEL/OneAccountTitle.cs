using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.ONECHARGING_MODEL
{
    public class OneAccountTitle
    {
        public int Id { get; set; }
        public string code { get; set; }
        public string AccountTitleCode { get; set; }
        public string AccountTitleName { get; set; }
        public int? SyncId { get; set; }
        public string Delete { get; set; }
        public bool? IsActive { get; set; }
    }
}
