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
    
    public partial class FAS_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FAS_Users()
        {
            this.M_Repair_Table = new HashSet<M_Repair_Table>();
        }
    
        public short UserID { get; set; }
        public string RFID { get; set; }
        public string UserName { get; set; }
        public bool IsActiv { get; set; }
        public byte UsersGroupID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_Repair_Table> M_Repair_Table { get; set; }
        public virtual FAS_UserGroup FAS_UserGroup { get; set; }
    }
}
