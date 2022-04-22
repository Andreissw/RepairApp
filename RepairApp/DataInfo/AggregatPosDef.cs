using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairApp.DataInfo
{
    public class AggregatPosDef
    {
        public string UserName { get; set; }
        public int count { get; set; }
        public string PositionName { get; set; }
        public string DefectCode { get; set; }
        public string ModelName { get; set; }
        public bool? Status { get; set; }

    }
}