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

            addRepaired();
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

            if (pos.Count != 0)
                foreach (var item in pos)
                    addRepaired(item);
            else
                addRepaired();

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

            foreach (var item in pos)           
                addRepaired(item);

            addLog(2);

            return "ok";
        }

        public string FalseFault()
        {
            if (decode == Environment.NewLine)
                return "1";

            rep = "Y";
            def = "3";
            addRepaired("");
            addLog(2);
            return "ok";
        }

        public string UNITISNOTOK()
        {
            if (decode == Environment.NewLine)
                return "1";

            foreach (var item in pos)
                addRepaired(item,false);

            addLog(3);
            return "ok";
        }

        public string ScrapBT()
        {
            if (decode == Environment.NewLine)
                return "1";

            foreach (var item in pos)
                addRepaired(item, false);

            addLog(4);
            return "ok";
        }

        void addRepaired(string position,bool uniok = true)
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
                };

                fas.M_Repair_Table.Add(repa);
                fas.SaveChanges();
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
                                        ('{idlazer}', '{step}','{testresult}',CURRENT_TIMESTAMP,'{username}' ,'{lotid}')");
                else
                    LoadGrid.SelectString($@"use fas  insert into [FAS].[dbo].[Ct_OperLog]  (PCBID,StepID,TestResultID,StepDate,Descriptions) values 
                                        ('{idlazer}', '{step}','{testresult}',CURRENT_TIMESTAMP,'{username}')");

            }

        }

        
    }
}