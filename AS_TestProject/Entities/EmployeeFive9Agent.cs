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
    
    public partial class EmployeeFive9Agent
    {
        public short EmployeeFive9AgentID { get; set; }
        public int EmployeeID { get; set; }
        public string AgentID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public int AddByEmplloyeeID { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
