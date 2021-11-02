using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairApp
{
    public class RepairList
    {
        public DateTime RepairDate { get; set; }

        public string RepairName { get; set; }

        public bool? RepairResult { get; set; }

        public string repOK { get; } = "OK";

        public string repNOK { get; } = "NOK";

        public string Lot { get; set; }

        public string RapairError { get; set; }

        public string RepaitPosition { get; set; }

        public string RepairCode { get; set; }

        public string DefectCode { get; set; }

        public string GenerateCode { get; set; }

        public string Description { get; set; }
    }
}