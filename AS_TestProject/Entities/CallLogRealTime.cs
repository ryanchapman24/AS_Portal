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
    
    public partial class CallLogRealTime
    {
        public System.DateTime RecordDate { get; set; }
        public byte DomainMasterID { get; set; }
        public string AgentID { get; set; }
        public string LeadID { get; set; }
        public string Disposition { get; set; }
        public string CallHourOfDay { get; set; }
        public string CampaignName { get; set; }
        public string CallType { get; set; }
        public string Skill { get; set; }
        public string ListName { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZipCode { get; set; }
        public string CallID { get; set; }
        public string ProjectID { get; set; }
        public string PatientID { get; set; }
        public System.TimeSpan RecordTime { get; set; }
        public long CallLogRealTimeID { get; set; }
    }
}
