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
    
    public partial class CFRPerformanceAnalysi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CFRPerformanceAnalysi()
        {
            this.CFRInsurances = new HashSet<CFRInsurance>();
            this.CFRInsurances1 = new HashSet<CFRInsurance>();
            this.CFRInsurances2 = new HashSet<CFRInsurance>();
            this.CFRInsurances3 = new HashSet<CFRInsurance>();
            this.CFRInsurances4 = new HashSet<CFRInsurance>();
            this.CFRMortgages = new HashSet<CFRMortgage>();
            this.CFRMortgages1 = new HashSet<CFRMortgage>();
            this.CFRMortgages2 = new HashSet<CFRMortgage>();
            this.CFRMortgages3 = new HashSet<CFRMortgage>();
            this.CFRMortgages4 = new HashSet<CFRMortgage>();
            this.CFRPatientRecruitments = new HashSet<CFRPatientRecruitment>();
            this.CFRPatientRecruitments1 = new HashSet<CFRPatientRecruitment>();
            this.CFRPatientRecruitments2 = new HashSet<CFRPatientRecruitment>();
            this.CFRPatientRecruitments3 = new HashSet<CFRPatientRecruitment>();
            this.CFRPatientRecruitments4 = new HashSet<CFRPatientRecruitment>();
        }
    
        public int CFRPerformanceAnalysisID { get; set; }
        public string PerformanceRating { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRInsurance> CFRInsurances4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRMortgage> CFRMortgages4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CFRPatientRecruitment> CFRPatientRecruitments4 { get; set; }
    }
}
