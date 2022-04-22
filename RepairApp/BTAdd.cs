using RepairApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairApp
{
    public class BTAdd
    {
        string er, def, rep, gen, desc, decode,username,iP;
        int userid;
        List<string> pos;
        FASEntities fas = new FASEntities();
        SMDCOMPONETSEntities smd = new SMDCOMPONETSEntities();
        public BTAdd(string Er, string Def, string Rep, string Gen,  string Desc, int UserID, string Decode, List<string> Pos,string IP)
        {
            er = Er ; def = Def; rep = Rep; gen = Gen; desc = Desc; userid = UserID; decode = Decode; pos = Pos; iP = IP;

            if (userid !=0)          
                username = fas.FAS_Users.FirstOrDefault(c => c.UserID == userid).UserName;
           
            
        }

        public string OTK_OK() {

            if (decode == "")
                return "1";
            if (decode == Environment.NewLine)
                return "1";

            //addRepaired();
            addLog(2,40);

            return "ok";
        }

        public string OTK_NOK() {

            if (desc == "" )
                return "1";

            if (decode == "")
                return "1";

            if (decode == Environment.NewLine)
                return "1";

            //if (pos.Count != 0) {
            //    var LOTID = GETLOTID(decode);
            //    foreach (var item in pos)
            //        //addRepaired(item, LOTID);
            //}
            //else
                //addRepaired();

            addLog(3,40);

            return "ok";
        }

        public string Repaired()
        {
            if (pos[0] == "" & rep != "H")            
                return "1";

            if (decode == "")
                return "1";

            if (decode == Environment.NewLine)
                return "1";

            foreach (var item in pos) {
                var LOTID = GETLOTID(decode);
                addRepaired(item, LOTID);
            }

            addLog(2);

            return "ok";
        }

        public string FalseFault()
        {
            if (decode == Environment.NewLine)
                return "1";

            rep = "Y";
            def = "3";
            var LOTID = GETLOTID(decode);
            addRepaired("", LOTID);
            addLog(2);
            return "ok";
        }

        public string UNITISNOTOK()
        {
            if (decode == Environment.NewLine)
                return "1";

            foreach (var item in pos) { 
                var LOTID = GETLOTID(decode);
                addRepaired(item, LOTID, false);
            }

            addLog(3);
            return "ok";
        }

        public string ScrapBT()
        {
            if (decode == Environment.NewLine)
                return "1";

            var LOTID = GETLOTID(decode);

            foreach (var item in pos)
                addRepaired(item, LOTID, false);

            addLog(4);
            return "ok";
        }

        void addRepaired(string position, int LOTID, bool uniok = true)
        {
            using (var fas = new FASEntities())
            {
                
                M_Repair_Table repa = new M_Repair_Table()
                {
                    Barcode = decode,
                    DefectCode = def,
                    RepairCode = rep,
                    Description = desc,
                    ErrorCode = er,
                    GeneratorCode = gen,
                    isUnitOK = uniok,
                    Position = position.ToUpper(),
                    RapairDate = DateTime.UtcNow.AddHours(2),
                    UserID = (short)userid,
                    Repairer = username,    
                    Repair_location = iP,
                    LOTID = LOTID
                };

                fas.M_Repair_Table.Add(repa);
                fas.SaveChanges();
            }
        }

        int GETLOTID(string Barcode)
        {
            var idlazer = smd.LazerBase.Where(c => c.Content == Barcode).Select(b => b.IDLaser).FirstOrDefault();
            if (idlazer == 0)
            {
                var lot = other();
                if (lot != 0)                
                    return lot;
                
            }

            var LOTID = Contract();
            if (LOTID != 0)         
                return LOTID;
         

            LOTID = VLV();
            if (LOTID != 0)         
                return LOTID;

            if (Barcode.Contains("BTA"))
                return 5156;
            if (Barcode.Contains("BTH"))
                return 5154;

            return 0;


            int VLV()
            {
                var serialnumber = fas.FAS_OperationLog.Where(c => c.PCBID == idlazer & c.SerialNumber != null).Select(c => c.SerialNumber).FirstOrDefault();
                if (serialnumber != null)                
                    if (serialnumber != 0)                    
                        return (int)fas.FAS_SerialNumbers.Where(c => c.SerialNumber == serialnumber).FirstOrDefault().LOTID;

                return fas.FAS_Disassembly.Where(c => c.PCBID == idlazer).Select(b => b.LOTID).FirstOrDefault();
               
                
            }

            int Contract()
            {
                var ctlotID = fas.Ct_OperLog.Where(c => c.PCBID != null & c.PCBID == idlazer).Select(c => c.LOTID).FirstOrDefault();
                if (ctlotID != null)               
                    if (ctlotID != 0)                 
                        return (int)ctlotID;
                return 0;
               
            }
            
            int other()
            {
                var snid = fas.Ct_FASSN_reg.Where(c => c.SN == Barcode).Select(c => c.ID).FirstOrDefault();
                if (snid != 0)
                {
                    var lot = fas.Ct_OperLog.Where(c => c.SNID == snid).Select(c => c.LOTID).FirstOrDefault();
                    if (lot != null)                   
                        return (int)lot;
                    return 0;                    
                }

             
                return 0;
                
            }
        }



        void addRepaired()
        {
            using (var fas = new FASEntities())
            {

                M_Repair_Table repa = new M_Repair_Table()
                {
                    Barcode = decode,
                    Description = desc,
                    GeneratorCode = gen,
                    isUnitOK = true,
                    RapairDate = DateTime.UtcNow.AddHours(2),
                    UserID = (short)userid,
                    Repairer = username,
                    Repair_location = iP,
                };

                fas.M_Repair_Table.Add(repa);
                fas.SaveChanges();
            }
        }

        void addLog(int testresult, int step = 4) 
        {            
                int? lotid;
                var idlazer = smd.LazerBase.Where(c => c.Content == decode).Select(c => c.IDLaser).FirstOrDefault();
                if (idlazer != 0)
                {
                    lotid = fas.Ct_OperLog.Where(c => c.PCBID == idlazer & c.LOTID != null).Select(c => c.LOTID).FirstOrDefault();
                if (lotid != null)
                    LoadGrid.SelectString($@"use fas  insert into [FAS].[dbo].[Ct_OperLog]  (PCBID,StepID,TestResultID,StepDate,Descriptions,LOTID) values 
                                        ('{idlazer}', '{step}','{testresult}',CURRENT_TIMESTAMP,'{username + " " + desc}' ,'{lotid}')");
                else
                    LoadGrid.SelectString($@"use fas  insert into [FAS].[dbo].[Ct_OperLog]  (PCBID,StepID,TestResultID,StepDate,Descriptions) values 
                                        ('{idlazer}', '{step}','{testresult}',CURRENT_TIMESTAMP,'{username + " " + desc}')");

            }

        }

        
    }
}