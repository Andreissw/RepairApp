using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairApp
{
    public class LoginData
    {
        [MaxLength(10)]
        public string RFID { get; set; }

        public string MachineName { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Decode { get; set; }    

        public string ErrorCode { get; set; }

        public List<SelectListItem> ListErrors { get; set; }
        public List<SelectListItem> ListDefects { get; set; }
        public List<SelectListItem> ListRepair { get; set; }
        public List<SelectListItem> ListGenerator { get; set; }

        public string DefectCode { get; set; }

        public string ReapirCode { get; set; }

        public List<string> ListPosition { get; set; }

        public string GenerateCode { get; set; }

        public virtual string ListName { get; set; }

        public string CurrentError { get; set; }



       
    }
}