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
    
    public partial class DailyPortfolioInventory
    {
        public System.DateTime ReportDate { get; set; }
        public string ListName { get; set; }
        public string State { get; set; }
        public int InitialCount { get; set; }
        public int CompletedCount { get; set; }
        public int InProgressCount { get; set; }
    }
}
