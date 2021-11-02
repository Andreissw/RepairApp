using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairApp.DataInfo
{
    public class RepairInfo
    {

        public string User { get; set; }

        public string Order { get; set; }
        public List<SelectListItem> NameUsers { get; set; }

        public List<SelectListItem> Orders { get; set; }
        public RepairInfo()
        {
            Orders = new List<SelectListItem>() { new SelectListItem {Text = "" } };
        }

        public List<RepairTables> RepairTables { get; set; }

        public List<AggregatPosition> aggregatPositions { get; set; }
         
        public List<AggregatPosRep> aggregatPosReps { get; set; }

        public List<AggregatPosDefRep> aggregatPosDefReps { get; set; }

        public List<AggregatPosDef> aggregatPosDefs { get; set; }
       
        

    }
}