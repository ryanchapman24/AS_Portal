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
    
    public partial class ClientLead
    {
        public string LeadID { get; set; }
        public string ClientName { get; set; }
        public System.DateTime LeadAddDate { get; set; }
        public System.TimeSpan LeadAddTime { get; set; }
        public System.DateTime ArrivalTimestamp { get; set; }
        public System.DateTime PreserviceTimestamp { get; set; }
        public System.DateTime PostserviceTimestamp { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        public string LeadType { get; set; }
        public string CampaignName { get; set; }
    }
}
