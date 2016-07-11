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
    
    public partial class EverquoteRollingCampaignMetric
    {
        public int RollingCampaignID { get; set; }
        public System.DateTime ReportDate { get; set; }
        public System.TimeSpan ReportTime { get; set; }
        public string CampaignID { get; set; }
        public int TotalAnswers { get; set; }
        public int TotalContacts { get; set; }
        public decimal ContactToTransfer { get; set; }
        public int TotalLeads { get; set; }
        public short TotalTransfers { get; set; }
        public short TotalDials { get; set; }
        public short CampaignDials { get; set; }
        public decimal TransfersPerDial { get; set; }
        public byte TotalAgents { get; set; }
        public decimal PercentOfTotalCalls { get; set; }
        public decimal TPHHours { get; set; }
        public decimal TransfersPerHour { get; set; }
        public string CallHourOfDay { get; set; }
        public string LeadSource { get; set; }
        public decimal CostPerTransfer { get; set; }
    }
}
