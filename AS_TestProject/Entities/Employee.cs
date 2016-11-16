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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Customers = new HashSet<Customer>();
            this.EmployeeFive9Agent = new HashSet<EmployeeFive9Agent>();
            this.EmployeeNotes = new HashSet<EmployeeNote>();
            this.EmployeeNoteTypes = new HashSet<EmployeeNoteType>();
            this.AgentDailyHours = new HashSet<AgentDailyHour>();
            this.EmployeeHoursTotals = new HashSet<EmployeeHoursTotal>();
            this.AgentDailyHours1 = new HashSet<AgentDailyHour>();
            this.CFRInsurances = new HashSet<CFRInsurance>();
            this.CFRInsurances1 = new HashSet<CFRInsurance>();
            this.CFRInsurances2 = new HashSet<CFRInsurance>();
            this.CFRMortgages = new HashSet<CFRMortgage>();
            this.CFRMortgages1 = new HashSet<CFRMortgage>();
            this.CFRPatientRecruitments = new HashSet<CFRPatientRecruitment>();
            this.CFRPatientRecruitments1 = new HashSet<CFRPatientRecruitment>();
            this.DisciplinaryActions = new HashSet<DisciplinaryAction>();
            this.DisciplinaryActions1 = new HashSet<DisciplinaryAction>();
        }
    
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public System.DateTime BirthDate { get; set; }
        public System.DateTime HireDate { get; set; }
        public System.DateTime TerminationDate { get; set; }
        public bool IsManager { get; set; }
        public int ManagerEmployeeID { get; set; }
        public int PositionID { get; set; }
        public byte SiteID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public int AddByEmployeeID { get; set; }
        public Nullable<System.DateTime> RehireDate { get; set; }
        public string FileNumber { get; set; }
        public string FullName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual Position Position { get; set; }
        public virtual Site Site { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeFive9Agent> EmployeeFive9Agent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeNote> EmployeeNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeNoteType> EmployeeNoteTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentDailyHour> AgentDailyHours { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeHoursTotal> EmployeeHoursTotals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentDailyHour> AgentDailyHours1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisciplinaryAction> DisciplinaryActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisciplinaryAction> DisciplinaryActions1 { get; set; }
    }
}
