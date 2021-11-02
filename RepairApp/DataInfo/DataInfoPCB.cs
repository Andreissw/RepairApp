using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairApp
{
    public class DataInfoPCB
    {
       public List<DissambleList> ListDissambles { get; set; }

        public List<ListOperLog> ListOperLogs { get; set; }

        public List<RepairList> RepairLists { get; set; }

        public List<AOIList> AOILists { get; set; }
    }
}