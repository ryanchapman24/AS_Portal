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
    
    public partial class AgentDailyHour
    {
        public string AgentID { get; set; }
        public string DomainName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LoginTimestamp { get; set; }
        public string LogoutTimestamp { get; set; }
        public System.TimeSpan LoginDuration { get; set; }
    }
}
