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
    
    public partial class FreedomHourlyCallMetric
    {
        public int DailyRecordID { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.TimeSpan RecordTime { get; set; }
        public string ListName { get; set; }
        public string CampaignName { get; set; }
        public string Disposition { get; set; }
        public string CallType { get; set; }
        public byte HourOfDay { get; set; }
        public string CallState { get; set; }
    }
}
