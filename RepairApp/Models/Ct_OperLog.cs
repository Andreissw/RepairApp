
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
    
public partial class Ct_OperLog
{

    public Nullable<int> PCBID { get; set; }

    public Nullable<int> LOTID { get; set; }

    public Nullable<short> StepID { get; set; }

    public byte TestResultID { get; set; }

    public System.DateTime StepDate { get; set; }

    public Nullable<short> StepByID { get; set; }

    public Nullable<byte> LineID { get; set; }

    public Nullable<int> ErrorCodeID { get; set; }

    public string Descriptions { get; set; }

    public Nullable<int> SNID { get; set; }

}

}
