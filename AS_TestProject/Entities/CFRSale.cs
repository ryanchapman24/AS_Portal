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
    
    public partial class CFRSale
    {
        public int CFRSalesID { get; set; }
        public int EmployeeID { get; set; }
        public Nullable<byte> DomainMasterID { get; set; }
        public short C_Calls { get; set; }
        public Nullable<int> sTEQ1 { get; set; }
        public Nullable<int> sTEQ2 { get; set; }
        public Nullable<int> sTEQ3 { get; set; }
        public Nullable<int> sTEQ4 { get; set; }
        public Nullable<int> sPQ1 { get; set; }
        public Nullable<int> sPQ2 { get; set; }
        public Nullable<int> sPQ3 { get; set; }
        public Nullable<int> sPQ4 { get; set; }
        public Nullable<int> sCQ1 { get; set; }
        public Nullable<int> sCQ2 { get; set; }
        public Nullable<int> sAQ1 { get; set; }
        public Nullable<int> sAQ2 { get; set; }
        public Nullable<int> sAOIQ1 { get; set; }
        public Nullable<int> sAOIQ2 { get; set; }
        public Nullable<int> TelephoneEtiquetteRating { get; set; }
        public Nullable<int> ProfessionalismRating { get; set; }
        public Nullable<int> ComplianceRating { get; set; }
        public Nullable<int> AdheranceRating { get; set; }
        public Nullable<int> AccuracyOfInformationRating { get; set; }
        public string ConversionRateToday { get; set; }
        public string WeekToDate { get; set; }
        public string Comments { get; set; }
        public string Strengths { get; set; }
        public string ActionPlan { get; set; }
        public Nullable<int> ManagerID { get; set; }
        public System.DateTime DateOfFeedback { get; set; }
    
        public virtual DomainMaster DomainMaster { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual AnswerKey AnswerKey { get; set; }
        public virtual AnswerKey AnswerKey1 { get; set; }
        public virtual AnswerKey AnswerKey2 { get; set; }
        public virtual AnswerKey AnswerKey3 { get; set; }
        public virtual AnswerKey AnswerKey4 { get; set; }
        public virtual AnswerKey AnswerKey5 { get; set; }
        public virtual AnswerKey AnswerKey6 { get; set; }
        public virtual AnswerKey AnswerKey7 { get; set; }
        public virtual AnswerKey AnswerKey8 { get; set; }
        public virtual AnswerKey AnswerKey9 { get; set; }
        public virtual AnswerKey AnswerKey10 { get; set; }
        public virtual AnswerKey AnswerKey11 { get; set; }
        public virtual AnswerKey AnswerKey12 { get; set; }
        public virtual AnswerKey AnswerKey13 { get; set; }
        public virtual CFRPerformanceAnalysi CFRPerformanceAnalysi { get; set; }
        public virtual CFRPerformanceAnalysi CFRPerformanceAnalysi1 { get; set; }
        public virtual CFRPerformanceAnalysi CFRPerformanceAnalysi2 { get; set; }
        public virtual CFRPerformanceAnalysi CFRPerformanceAnalysi3 { get; set; }
        public virtual CFRPerformanceAnalysi CFRPerformanceAnalysi4 { get; set; }
    }
}
