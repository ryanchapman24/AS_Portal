//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AS_TestProject.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class DomainMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DomainMaster()
        {
            this.AgentDailyHours = new HashSet<AgentDailyHour>();
            this.CallLogMasters = new HashSet<CallLogMaster>();
            this.EmployeeHoursTotals = new HashSet<EmployeeHoursTotal>();
            this.CFRInsurances = new HashSet<CFRInsurance>();
            this.CFRMortgages = new HashSet<CFRMortgage>();
            this.CFRPatientRecruitments = new HashSet<CFRPatientRecruitment>();
            this.AgentDailyHoursToDomainMasters = new HashSet<AgentDailyHoursToDomainMaster>();
            this.CFRSales = new HashSet<CFRSale>();
        }
    
        public byte DomainMasterID { get; set; }
        public string DomainName { get; set; }
        public short CustomerID { get; set; }
        public byte DomainTypeID { get; set; }
        public string CostCode { get; set; }
        public string FileMask { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DeactiveDate { get; set; }
        public System.DateTime AddDate { get; set; }
        public System.TimeSpan AddTime { get; set; }
        public int AddByEmployeeID { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual DomainType DomainType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentDailyHour> AgentDailyHours { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallLogMaster> CallLogMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeHoursTotal> EmployeeHoursTotals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentDailyHoursToDomainMaster> AgentDailyHoursToDomainMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRSale> CFRSales { get; set; }
    }
}
