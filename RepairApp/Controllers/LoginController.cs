using RepairApp.DataInfo;
using RepairApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RepairApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        Entities ent = new Entities();
        FASEntities fas = new FASEntities();
        SMDCOMPONETSEntities smd = new SMDCOMPONETSEntities();
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult WorkForm(LoginData login)
        {

            //login.MachineName = Environment.MachineName;
            login.MachineName = HttpContext.Request.UserHostAddress;
            login.ListErrors = new List<SelectListItem>() { new SelectListItem { Text = "" } };
            login.ListErrors.AddRange(fas.FAS_ErrorCode.Select(c => new SelectListItem { Text = c.ErrorCode }).Distinct().ToList());

            login.ListRepair = new List<SelectListItem>() { new SelectListItem { Text = "" } };
            login.ListRepair.AddRange(fas.FAS_RepairCode.Select(c => new SelectListItem { Text = c.NameCode }).Distinct().ToList());

            login.ListDefects = new List<SelectListItem>() { new SelectListItem { Text = "" } };
            login.ListDefects.AddRange(fas.FAS_DefectCode.Select(c => new SelectListItem { Text = c.NameCode }).Distinct().ToList());

            login.ListGenerator = new List<SelectListItem>() { new SelectListItem { Text = "" }, new SelectListItem { Text = "01" }, new SelectListItem { Text = "09" } };


            //login.MachineName = Dns.GetHostEntry(Request.UserHostAddress).HostName.ToString();     
            return View(login);
        }

        public ActionResult Info()
        {
            return View();
        }

        public ActionResult GetTableRepair(string NameUser, string NameOrder, string date_st, string date_end)
        {
            RepairInfo repairInfo = new RepairInfo();
            repairInfo.RepairTables = new List<RepairTables>();
            string IP = HttpContext.Request.UserHostAddress; 

            //NameUser = NameUser == "Все ремонтики" ? "" : NameUser; //Если Все ремонтики то имя пустое 

            #region
            //string Query = ""; //Переменная для конечного запроса 


            ////================================== Otherq общий запрос за период времени, не привязывается к модели
            //string otherQ = @"( select 
            //                (case when(select FullLOTCode from Contract_LOT where ID = (select TOP(1) LOTID from Ct_OperLog where PCBID =
            //               (select IDLaser from SMDCOMPONETS.dbo.LazerBase where ttt.Barcode = Content))) is not null then

            //               (select FullLOTCode from Contract_LOT where ID = (select TOP(1) LOTID from Ct_OperLog where PCBID =
            //               (select IDLaser from SMDCOMPONETS.dbo.LazerBase where ttt.Barcode = Content)))

            //                else

            //    (select FULL_LOT_Code from FAS_GS_LOTs

            //    where LOTID =  (select TOP(1)(select LOTID from FAS_SerialNumbers ss where ss.SerialNumber = stt.SerialNumber )
            //                from FAS_OperationLog stt where stt.PCBID =
            //                (select IDLaser from SMDCOMPONETS.dbo.LazerBase where ttt.Barcode = Content) order by stt.StateCodeDate desc))

            //                end) Заказ";


            ////=========================================================== Запрос конкретно по контракному заказу 
            //string contractQ = @" (select (select FullLOTCode from Contract_LOT where ID = (select TOP(1) LOTID from Ct_OperLog where PCBID =
            //               (select IDLaser from SMDCOMPONETS.dbo.LazerBase where ttt.Barcode = Content))) Заказ";


            ////========================================================== Запрос конкретно по ВЛВ заказку
            //string VLVq = @"  (select (select FULL_LOT_Code from FAS_GS_LOTs

            //    where LOTID = (select TOP(1)(select LOTID from FAS_SerialNumbers ss where ss.SerialNumber = stt.SerialNumber )
            //                from FAS_OperationLog stt where stt.PCBID =
            //                (select IDLaser from SMDCOMPONETS.dbo.LazerBase where ttt.Barcode = Content) order by stt.StateCodeDate desc)) Заказ";

            //string QQ = "";

            //string NameUserQ = $"where tttt.[Имя ремонтника] = '{NameUser}'";
            //string NameOrderQ = $"Заказ = '{NameOrder}'";
            #endregion

            if (date_st == " " || date_end == " ")
            {
                return PartialView(repairInfo);
            }

            if (DateTime.Parse(date_st) >= DateTime.Parse(date_end)) //Если дата не указана
            {
                return PartialView("Ошибка Даты");
            }

            var st = DateTime.Parse(date_st);
            var end = DateTime.Parse(date_end);

            var listBarcode = fas.M_Repair_Table.Where(c => c.RapairDate >= st & c.RapairDate <= end & c.Position != null).Select(c =>c.Barcode).Distinct().ToList();
            //var list = new List<FAS_RepairTemp>();

            //Parallel.ForEach<string>(listBarcode, x =>
            //{
            //    FAS_RepairTemp fAS_RepairTemp = new FAS_RepairTemp()
            //    { 
            //        Barcode = x.
            //    };

            //});
            fas.FAS_RepairTemp.RemoveRange(fas.FAS_RepairTemp.Where(c => c.IPADRESS == IP));
            fas.SaveChanges();

            if (listBarcode.Count > 999)
            {
                var part = listBarcode.Count / 999;
                int start = 0;
                int count = 999;

                for (int i = 0; i <= part; i++)
                {
                    if (i == part)
                    {
                        count = listBarcode.Count - start;
                    }

                    var L = listBarcode.GetRange(start, count);
                    var Templist = smd.LazerBase.Where(c => L.Where(b => b.Substring(7, 3) != "BTH").Contains(c.Content)).Select(c => new FAS_RepairTemp()
                    {
                        Barcode = c.Content,
                        PCBID = c.IDLaser,
                        IPADRESS = IP,

                    }).ToList();

                    fas.FAS_RepairTemp.AddRange(Templist);
                    fas.SaveChanges();

                    start += 999;
                    //ends += 999;

                }
            }
            else if (listBarcode.Count < 999)
            {
                var Templist = smd.LazerBase.Where(c => listBarcode.Where(b => b.Substring(7, 3) != "BTH").Contains(c.Content)).Select(c => new FAS_RepairTemp()
                {
                    Barcode = c.Content,
                    PCBID = (int)c.IDLaser,
                    IPADRESS = IP,

                }).ToList();

                fas.FAS_RepairTemp.AddRange(Templist);
                fas.SaveChanges();
            }

            var l = fas.FAS_RepairTemp.Select(b => b.PCBID).ToList();
            var B = fas.FAS_RepairTemp.Select(b => b.Barcode).ToList();
            var list = fas.FAS_RepairTemp.ToList();


            //var l = list.Select(b => b.ID).ToList();
            //var B = list.Select(b => b.Barcode).ToList();

            var listData = fas.Ct_OperLog.Where(c => l.Contains((int)c.PCBID) & c.ErrorCodeID != null).OrderByDescending(c=>c.StepDate).Select(c =>  new Merg()
            {
                PCBID = (int)c.PCBID,
                ErrorCode = fas.FAS_ErrorCode.Where(b=> b.ErrorCodeID == c.ErrorCodeID).Select(b=>b.ErrorCode + " " + b.Description).FirstOrDefault(),
                ModelName = fas.Contract_LOT.Where(b=>b.ID == c.LOTID).Select(b=>b.FullLOTCode).FirstOrDefault(),
                
            }).ToList();

            var listdata2 = fas.FAS_Disassembly.Where(c => l.Contains((int)c.PCBID)).OrderByDescending(c => c.DisassemblyDate).Select(c => new Merg()
            {
                PCBID = c.PCBID,
                ErrorCode = fas.FAS_ErrorCode.Where(b => b.ErrorCodeID == c.ErrorCodeID).Select(b => b.ErrorCode + " " + b.Description).FirstOrDefault(),
                ModelName = fas.FAS_GS_LOTs.Where(b => b.LOTID == c.LOTID).Select(b => b.FULL_LOT_Code).FirstOrDefault(),

            }).ToList();

            var BB = fas.Ct_FASSN_reg.Where(c => B.Contains(c.SN)).Select(c => c.ID).ToList();

            var listdata3 = fas.Ct_OperLog.Where(c => BB.Contains((int)c.PCBID)).OrderByDescending(c => c.StepDate).Select(c => new Merg()
            {
                PCBID = (int)c.PCBID,
                ErrorCode = fas.FAS_ErrorCode.Where(b => b.ErrorCodeID == c.ErrorCodeID).Select(b => b.ErrorCode + " " + b.Description).FirstOrDefault(),
                ModelName = fas.Contract_LOT.Where(b => b.ID == c.LOTID).Select(b => b.FullLOTCode).FirstOrDefault(),

            }).ToList();

            listData.AddRange(listdata2);
            listData.AddRange(listdata3);

           
            var Table = fas.M_Repair_Table.Where(c => listBarcode.Contains(c.Barcode) & c.Position != null & c.RapairDate >= st & c.RapairDate <= end).Select(c => new RepairTables
            {
                Barcode = c.Barcode,
                Date = (DateTime)c.RapairDate,
                NameUser = c.FAS_Users.UserName,
                DefectName = fas.FAS_DefectCode.Where(b => b.NameCode == c.DefectCode).Select(b => b.NameCode + " " + b.DescriptionCode).FirstOrDefault(),
                RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.RepairCode).Select(b => b.NameCode + " " + b.DescriptionCode).FirstOrDefault(),
                Position = c.Position,
                Status = c.isUnitOK,

            }).ToList();


            Parallel.ForEach<RepairTables>(Table, x =>
            {
                if (x.Barcode.Contains("BTH"))
                {
                    x.ModelName = "BarTon"; return;
                }

                x.ID = list.Where(c => c.Barcode == x.Barcode).Select(c => (int)c.PCBID).FirstOrDefault();
                x.ErrorCode = listData.Where(c => c.PCBID == x.ID).Select(c => c.ErrorCode).FirstOrDefault();
                x.ModelName = listData.Where(c => c.PCBID == x.ID).Select(c => c.ModelName).FirstOrDefault();
            });


            Table = GetRepairTables(Table,NameOrder,NameUser);
            repairInfo.RepairTables = Table.OrderByDescending(c => c.Date).ToList();

            repairInfo.aggregatPositions = Table.GroupBy(c => new { c.Status,c.ModelName ,c.Position }).Select(c => new AggregatPosition()
            {
                Modelname = c.Key.ModelName,
                UserName = c.Select(z => z.NameUser).FirstOrDefault(),
                Position = c.Key.Position,
                Status = c.Key.Status,
                count = c.Count(),

            }).OrderByDescending(c => c.count).ToList();

            repairInfo.aggregatPosReps = Table.GroupBy(c => new { c.Status,c.ModelName,c.Position, c.RepairCode }).Select(c => new AggregatPosRep()
            {
                ModelName = c.Key.ModelName,
                UserName = c.Select(z => z.NameUser).FirstOrDefault(),
                PositionName = c.Key.Position,
                RepairCode = c.Key.RepairCode,
                Status = c.Key.Status,
                count = c.Count(),

            }).OrderByDescending(c => c.count).ToList();

            repairInfo.aggregatPosDefs = repairInfo.RepairTables.GroupBy(c => new { c.Status, c.ModelName,c.Position, c.DefectName }).Select(c => new AggregatPosDef()
            {
                ModelName = c.Key.ModelName,
                UserName = c.Select(z => z.NameUser).FirstOrDefault(),
                PositionName = c.Key.Position,
                DefectCode = c.Key.DefectName,
                Status = c.Key.Status,
                count = c.Count(),

            }).OrderByDescending(c => c.count).ToList();

            repairInfo.aggregatPosDefReps = repairInfo.RepairTables.GroupBy(c => new { c.Status,c.ModelName,c.Position, c.DefectName, c.RepairCode }).Select(c => new AggregatPosDefRep()
            {
                ModelName = c.Key.ModelName,
                UserName = c.Select(z => z.NameUser).FirstOrDefault(),
                PositionName = c.Key.Position,
                DefectCode = c.Key.DefectName,
                RepairCode = c.Key.RepairCode,
                Status = c.Key.Status,
                count = c.Count(),

            }).OrderByDescending(c => c.count).ToList();


            if (Request.IsAjaxRequest())
            {
                return PartialView(repairInfo);
            }
            else
            {
                return View(repairInfo);
            }

            #region      



            //repairInfo.aggregatPosDefReps = repairInfo.RepairTables.GroupBy(c => new { c.Position, c.DefectName, c.RepairCode }).Select(c => new AggregatPosDefRep()
            //{
            //    UserName = c.Select(z => z.NameUser).FirstOrDefault(),
            //    PositionName = c.Key.Position,
            //    DefectCode = c.Key.DefectName,
            //    RepairCode = c.Key.RepairCode,
            //    count = c.Count(),

            //}).OrderByDescending(c => c.count).ToList();
            #endregion
        }

        List<RepairTables> GetRepairTables(List<RepairTables> repairTables, string modelname, string User)
        {
            if (modelname != "")
                repairTables = repairTables.Where(c => c.ModelName == modelname).ToList();
            if (User != "Все ремонтники")
                repairTables = repairTables.Where(c => c.NameUser == User).ToList();

            return repairTables;
        }


            public ActionResult GetDataRepair()
            {
                RepairInfo repairInfo = new RepairInfo();
                repairInfo.NameUsers = new List<SelectListItem>() { new SelectListItem { Text = "Все ремонтники" } };
                repairInfo.NameUsers.AddRange(fas.FAS_Users.Where(c => fas.M_Repair_Table.Where(b => b.UserID != null).Select(b => b.UserID).Distinct().Contains(c.UserID)).Select(c => new SelectListItem { Text = c.UserName }).ToList());

                var datasetOrder = LoadGrid.Loadgrid($@" use fas select * from (select distinct FullLOTCode as Orders, ID as LOTID from Contract_LOT
                                      union all 
                                     select distinct FULL_LOT_Code as Orders, LOTID as LOTID from FAS_GS_LOTs where LOTID not in (78,64,110) ) tt order by tt.LOTID desc");

                //var datasetOrder = LoadGrid.Loadgrid($@" use fas select * from (select distinct FullLOTCode as Orders, ID as LOTID from Contract_LOT  ) tt order by tt.LOTID desc");

                foreach (DataRow item in datasetOrder.Tables[0].Rows)
                {
                    repairInfo.Orders.Add(new SelectListItem { Text = item.ItemArray.FirstOrDefault().ToString() });
                }


                return PartialView(repairInfo);
            }

            public ActionResult GetListError(string Decode)
            {
                if (Decode == "")
                    return PartialView();


                var pcbid = smd.LazerBase.Where(c => c.Content == Decode).Select(c => c.IDLaser).FirstOrDefault();
                DataInfoPCB dataInfoPCB = new DataInfoPCB();
                if (pcbid != 0)
                {
                    dataInfoPCB.ListDissambles = fas.Ct_OperLog.OrderByDescending(c => c.StepDate).Where(c => c.PCBID == pcbid & c.ErrorCodeID != null)
                        .Select(c => new DissambleList
                        {
                            Name = fas.FAS_ErrorCode.Where(b => b.ErrorCodeID == c.ErrorCodeID).Select(b => b.ErrorCode + " - " + b.Description).FirstOrDefault(),
                            Date = c.StepDate
                        }).ToList();

                    var list2 = fas.FAS_Disassembly.OrderByDescending(c => c.DisassemblyDate).Where(c => c.PCBID == pcbid)
                        .Select(c => new DissambleList
                        {
                            Name = fas.FAS_ErrorCode.Where(b => b.ErrorCodeID == c.ErrorCodeID).Select(b => b.ErrorCode + " - " + b.Description).FirstOrDefault(),
                            Date = c.DisassemblyDate
                        }).ToList();

                    dataInfoPCB.ListDissambles.AddRange(list2);


                    dataInfoPCB.ListOperLogs = fas.Ct_OperLog.Where(c => c.PCBID == pcbid).Select(c => new ListOperLog {


                        NameStage = fas.Ct_StepScan.Where(b => b.ID == c.StepID).Select(b => b.StepName).FirstOrDefault(),
                        NameResult = fas.Ct_TestResult.Where(b => b.ID == c.TestResultID).Select(b => b.Result).FirstOrDefault(),
                        DateStage = c.StepDate,
                        UserStage = fas.FAS_Users.Where(b => b.UserID == c.StepByID).Select(b => b.UserName).FirstOrDefault(),
                        Description = c.Descriptions
                    }).ToList();




                    dataInfoPCB.AOILists = smd.AOIresult.Where(c => c.PCBnumber == Decode).Select(c => new AOIList
                    {
                        DateAOI = c.inspectionDate,
                        NameAOI = c.User_INSPect,
                        NameResutAOI = c.UserInspectionResult,
                        NamePosition = c.CIRNAME,

                    }).ToList();
                }

                dataInfoPCB.RepairLists = fas.M_Repair_Table.Where(c => c.Barcode == Decode).OrderByDescending(c => c.RapairDate).Select(c => new RepairList
                {
                    RepairDate = (DateTime)c.RapairDate,
                    RepairName = c.Repairer,
                    RepairResult = c.isUnitOK,
                    RapairError = c.ErrorCode,
                    RepaitPosition = c.Position,
                    RepairCode = fas.FAS_RepairCode.Where(b => b.NameCode == c.RepairCode).Select(b => b.NameCode + " " + b.DescriptionCode).FirstOrDefault(),
                    DefectCode = fas.FAS_DefectCode.Where(b => b.NameCode == c.DefectCode).Select(b => b.NameCode + " " + b.DescriptionCode).FirstOrDefault(),
                    GenerateCode = c.GeneratorCode,
                    Description = c.Description,
                }).ToList();

                if (Request.IsAjaxRequest())
                {
                    return PartialView(dataInfoPCB);
                }
                else
                {
                    return View(dataInfoPCB);
                }

            }

            public JsonResult CheckGroupUser(int UserID)
            {
                bool result = false;

                var group = fas.FAS_Users.Where(c => c.UserID == UserID).Select(c => c.UsersGroupID).FirstOrDefault();
                if (group == 4)
                {
                    result = true;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            public JsonResult AddBaseOTK(string BT, string er, string def, string rep, string gen, string Desc, int UserID, string Decode, List<string> pos, string IP)
            {
                var list = pos[0].Split(',').ToList();
                var ClassBT = new BTAdd(er, def, rep, gen, Desc, UserID, Decode, list, IP);

                string result = null;

                if (BT == "Подтвердить ремонт")
                    result = ClassBT.OTK_OK();
                if (BT == "Не подтвердить ремонт")
                    result = ClassBT.OTK_NOK();



                return Json(result, JsonRequestBehavior.AllowGet);
            }

            public JsonResult AddBase(string BT, string er, string def, string rep, string gen, string Desc, int UserID, string Decode, List<string> pos, string IP)
            {
                var list = pos[0].Split(',').ToList();
                var ClassBT = new BTAdd(er, def, rep, gen, Desc, UserID, Decode, list, IP);

                string result = null;

                if (BT == "Отремонтирован")
                    result = ClassBT.Repaired();
                if (BT == "Возврат без ремонта")
                    result = ClassBT.FalseFault();
                if (BT == "UNIT is not ok")
                    result = ClassBT.UNITISNOTOK();
                if (BT == "Брак на списание")
                    result = ClassBT.ScrapBT();


                return Json(result, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public ActionResult GetUserID(LoginData login)
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Не верно введены данные");
                    TempData["Er"] = "Не верный формат пароля";
                    return RedirectToAction("Login");
                }
                string username = CheckUser(login.RFID);
                if (string.IsNullOrWhiteSpace(username))
                {
                    ModelState.AddModelError("", "Не верно введены данные");
                    TempData["Er"] = "Не верный пароль";
                    return RedirectToAction("Login");
                }

                TempData["Er"] = null;
                login.UserName = username;
                login.UserID = fas.FAS_Users.Where(c => c.UserName == username).Select(c => c.UserID).FirstOrDefault();
                return RedirectToAction("WorkForm", login);
            }
            public JsonResult GetTIme()
            {
                return Json(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), JsonRequestBehavior.AllowGet);
            }



            string CheckUser(string RFID)
            {
                using (var fas = new FASEntities())
                {
                    var UserName = fas.FAS_Users.Where(c => c.RFID == RFID).Select(c => c.UserName).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(UserName))
                        return string.Empty;
                    return UserName;

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


            if (idlazer != 0)
            {
                LOTID = VLV();

                if (LOTID != 0)
                    return LOTID;
            }
            

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

        public JsonResult GetDecode(string decode)
            {
                if (decode.Length <= 5)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                string errorCode = "";

                var Decode = smd.LazerBase.Where(c => c.Content == decode).Select(c => c.Content).FirstOrDefault();

                if (Decode == null)
                {
                    Decode = fas.M_CadenaID.Where(c => c.TRID == decode).Select(c => c.TRID).FirstOrDefault();
                }

                if (Decode == null)
                {
                    Decode = fas.CT_TCC_SN_MAC.Where(c => c.series_bar == decode).Select(c => c.series_bar).FirstOrDefault();
                }

                if (Decode == null)
                {
                    Decode = ent.TRC_Unit.Where(c => c.Barcode == decode).Select(c => c.Barcode).FirstOrDefault();
                }

                if (Decode != null)
                {
                    var pcbid = smd.LazerBase.Where(c => c.Content == decode).Select(c => c.IDLaser).FirstOrDefault();
                    if (pcbid != 0)
                    {
                        var idEr = fas.Ct_OperLog.OrderByDescending(c => c.StepDate).Where(c => c.ErrorCodeID != null & c.PCBID == pcbid).Select(c => c.ErrorCodeID).FirstOrDefault();
                        if (idEr == null)
                            idEr = fas.FAS_Disassembly.OrderByDescending(c => c.DisassemblyDate).Where(c => c.PCBID == pcbid).Select(c => c.ErrorCodeID).FirstOrDefault();
                        errorCode = fas.FAS_ErrorCode.Where(c => c.ErrorCodeID == idEr).Select(c => c.ErrorCode).FirstOrDefault();
                    }

                }

         

                if (Decode == null)
                    Decode = "";

            string FULLLOTCODE = "";

                var LOTID = GETLOTID(decode);
                if (LOTID != 0)
                {
                    var result = fas.FAS_GS_LOTs.Select(c => new LOT{ LOTID = c.LOTID, FULLLOTCODE = c.FULL_LOT_Code }).ToList();
                    var result2 = fas.Contract_LOT.Select(c => new LOT{ LOTID = c.ID, FULLLOTCODE = c.FullLOTCode }).ToList();
                    
                    result.AddRange(result2);
                    result.AddRange(new List<LOT>() { new LOT() { LOTID = 5154, FULLLOTCODE = "BarTon TH-562" }, new LOT() { LOTID = 5156, FULLLOTCODE = "BarTon TA-561" } });
                    FULLLOTCODE = result.Where(c => c.LOTID == LOTID).Select(c=>c.FULLLOTCODE).FirstOrDefault();
                   
                }

                return new JsonResult { Data = new { Decode = Decode, errorcode = errorCode, ModelName = FULLLOTCODE }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            public JsonResult GetPosition(string filter)
            {
                var list = ent.TRC_RepairPosition.Where(c => c.PositionName.StartsWith(filter)).OrderBy(c => c.PositionName).Select(c => new PositinName {

                    Name = c.PositionName

                }).Distinct().ToList();

                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            public JsonResult CheckPosition(string content)
            {
                string ErrorCode = ent.TRC_RepairPosition.Where(c => c.PositionName == content).Select(c => c.PositionName).FirstOrDefault();
                return Json(ErrorCode, JsonRequestBehavior.AllowGet);
            }


            public JsonResult GetErrorCode(string filter)
            {
                var list = fas.FAS_ErrorCode.Where(c => c.ErrorCode.StartsWith(filter)).Select(c => new PositinName
                {
                    Name = c.ErrorCode

                }).Distinct().ToList();

                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            public JsonResult CheckError(string content)
            {
                string ErrorCode = fas.FAS_ErrorCode.Where(c => c.ErrorCode == content).Select(c => c.ErrorCode).FirstOrDefault();
                if (ErrorCode == null)
                    ErrorCode = "";

                return Json(ErrorCode, JsonRequestBehavior.AllowGet);
            }

            public JsonResult GetRepCode(string filter)
            {
                if (filter == "")
                {
                    return new JsonResult { Data = ent.TRC_RepairCode.Select(c => new PositinName
                    {
                        Name = c.RepairCodeName

                    }).Distinct().Take(10).ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var list = ent.TRC_RepairCode.Where(c => c.RepairCodeName.StartsWith(filter)).Select(c => new PositinName
                {
                    Name = c.RepairCodeName

                }).Distinct().Take(10).ToList();

                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            public JsonResult CheckRep(string content)
            {
                string RepCode = ent.TRC_RepairCode.Where(c => c.RepairCodeName == content).Select(c => c.RepairCodeName).FirstOrDefault();
                if (RepCode == null)
                    RepCode = "";

                return Json(RepCode, JsonRequestBehavior.AllowGet);
            }

            public JsonResult GerDefect(string filter)
            {
                var list = ent.TRC_DefectCode.Where(c => c.DefectCodeName.StartsWith(filter)).Select(c => new PositinName
                {
                    Name = c.DefectCodeName

                }).Distinct().Take(10).ToList();

                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            public JsonResult CheckDefect(string content)
            {
                string DefectCode = ent.TRC_DefectCode.Where(c => c.DefectCodeName == content).Select(c => c.DefectCodeName).FirstOrDefault();
                if (DefectCode == null)
                    DefectCode = "";

                return Json(DefectCode, JsonRequestBehavior.AllowGet);
            }
        }
    } 
