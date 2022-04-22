using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairApp.DataInfo
{
    public class RepairTables
    {
       
        public string Barcode { get; set; }              
        public DateTime Date { get; set; }
        public string NameUser { get; set; }
        public string DefectName { get; set; }
        public string Position { get; set; }
        public string ErrorCode { get; set; }
        public string RepairCode { get; set; }
        public string ModelName { get; set; }

        public bool? Status { get; set; }
        public int ID { get; set; }
       
    
    }

   
}