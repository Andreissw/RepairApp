//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RepairApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class M_CadenaID
    {
        public int ID { get; set; }
        public string TRID { get; set; }
        public bool IsUsed { get; set; }
        public Nullable<int> LOTID { get; set; }
        public Nullable<bool> IsPacked { get; set; }
        public string Liter { get; set; }
        public Nullable<int> PalletNumber { get; set; }
        public Nullable<int> BoxNumber { get; set; }
        public Nullable<short> UnitNumber { get; set; }
        public Nullable<System.DateTime> PackDate { get; set; }
        public Nullable<System.DateTime> LabelDate { get; set; }
        public Nullable<short> PrintStationID { get; set; }
        public Nullable<short> Weight { get; set; }
        public Nullable<bool> IsWeight { get; set; }
        public Nullable<short> PrintBy { get; set; }
        public Nullable<bool> IsReprinted { get; set; }
        public Nullable<System.DateTime> OldDateLabel { get; set; }
        public string LangPO { get; set; }
        public Nullable<bool> Verify { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> DateVerify { get; set; }
    }
}
