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
    
    public partial class EmployeeHoursTotal
    {
        public short EmployeeHoursTotalID { get; set; }
        public short PayPeriodID { get; set; }
        public byte DomainMasterID { get; set; }
        public int EmployeeID { get; set; }
        public System.TimeSpan TotalDuration { get; set; }
    
        public virtual DomainMaster DomainMaster { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual PayPeriod PayPeriod { get; set; }
    }
}