﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FASEntities : DbContext
    {
        public FASEntities()
            : base("name=FASEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FAS_Users> FAS_Users { get; set; }
        public virtual DbSet<FAS_ErrorCode> FAS_ErrorCode { get; set; }
        public virtual DbSet<CT_TCC_SN_MAC> CT_TCC_SN_MAC { get; set; }
        public virtual DbSet<M_CadenaID> M_CadenaID { get; set; }
        public virtual DbSet<M_Repair_Table> M_Repair_Table { get; set; }
        public virtual DbSet<Ct_OperLog> Ct_OperLog { get; set; }
        public virtual DbSet<FAS_Disassembly> FAS_Disassembly { get; set; }
        public virtual DbSet<Ct_StepResult> Ct_StepResult { get; set; }
        public virtual DbSet<Ct_StepScan> Ct_StepScan { get; set; }
        public virtual DbSet<Ct_TestResult> Ct_TestResult { get; set; }
        public virtual DbSet<FAS_UserGroup> FAS_UserGroup { get; set; }
        public virtual DbSet<FAS_DefectCode> FAS_DefectCode { get; set; }
        public virtual DbSet<FAS_RepairCode> FAS_RepairCode { get; set; }
    }
}
