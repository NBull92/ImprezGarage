//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class MaintenanceCheckType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MaintenanceCheckType()
        {
            this.MaintenanceChecks = new HashSet<MaintenanceCheck>();
            this.MaintenanceCheckOptions = new HashSet<MaintenanceCheckOption>();
        }
    
        public int Id { get; set; }
        public string Type { get; set; }
        public Nullable<int> OccurenceMonthly { get; set; }
        public Nullable<int> OccurenceMiles { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaintenanceCheck> MaintenanceChecks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaintenanceCheckOption> MaintenanceCheckOptions { get; set; }
    }
}
