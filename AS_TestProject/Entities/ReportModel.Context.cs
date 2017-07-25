﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ReportEntities : DbContext
    {
        public ReportEntities()
            : base("name=ReportEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerContact> CustomerContacts { get; set; }
        public virtual DbSet<DomainMaster> DomainMasters { get; set; }
        public virtual DbSet<DomainType> DomainTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeFive9Agent> EmployeeFive9Agent { get; set; }
        public virtual DbSet<EmployeeNote> EmployeeNotes { get; set; }
        public virtual DbSet<EmployeeNoteType> EmployeeNoteTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<ProblemMaster> ProblemMasters { get; set; }
        public virtual DbSet<CampaignGroup> CampaignGroups { get; set; }
        public virtual DbSet<DailyPortfolioInventory> DailyPortfolioInventories { get; set; }
        public virtual DbSet<AgentDailyHour> AgentDailyHours { get; set; }
        public virtual DbSet<AgentTimeAdjustmentReason> AgentTimeAdjustmentReasons { get; set; }
        public virtual DbSet<EmployeeHoursTotal> EmployeeHoursTotals { get; set; }
        public virtual DbSet<PayPeriod> PayPeriods { get; set; }
        public virtual DbSet<CallLogMaster> CallLogMasters { get; set; }
        public virtual DbSet<EverquoteHourlyCampaignMetric> EverquoteHourlyCampaignMetrics { get; set; }
        public virtual DbSet<EverquoteRollingCampaignMetric> EverquoteRollingCampaignMetrics { get; set; }
        public virtual DbSet<PreciseLeadsHourlyCampaignMetric> PreciseLeadsHourlyCampaignMetrics { get; set; }
        public virtual DbSet<PreciseLeadsRollingCampaignMetric> PreciseLeadsRollingCampaignMetrics { get; set; }
        public virtual DbSet<FreedomHourlyCallMetric> FreedomHourlyCallMetrics { get; set; }
        public virtual DbSet<ClientLead> ClientLeads { get; set; }
        public virtual DbSet<EverQuoteHourlyCallLogMaster> EverQuoteHourlyCallLogMasters { get; set; }
        public virtual DbSet<FreedomDailyPortfolioInventory> FreedomDailyPortfolioInventories { get; set; }
        public virtual DbSet<FreedomHourlyCallMetrics1> FreedomHourlyCallMetrics1 { get; set; }
        public virtual DbSet<PreciseLeadsHourlyCallLogMaster> PreciseLeadsHourlyCallLogMasters { get; set; }
        public virtual DbSet<AgentDailyHours1> AgentDailyHours1 { get; set; }
        public virtual DbSet<CallLogRealTime> CallLogRealTimes { get; set; }
        public virtual DbSet<AnswerKey> AnswerKeys { get; set; }
        public virtual DbSet<CFRInsurance> CFRInsurances { get; set; }
        public virtual DbSet<CFRMortgage> CFRMortgages { get; set; }
        public virtual DbSet<CFRPatientRecruitment> CFRPatientRecruitments { get; set; }
        public virtual DbSet<CFRPerformanceAnalysi> CFRPerformanceAnalysis { get; set; }
        public virtual DbSet<DisciplinaryAction> DisciplinaryActions { get; set; }
        public virtual DbSet<AgentDailyHoursToDomainMaster> AgentDailyHoursToDomainMasters { get; set; }
        public virtual DbSet<CFRSale> CFRSales { get; set; }
        public virtual DbSet<CFRAcurian> CFRAcurians { get; set; }
    
        public virtual ObjectResult<uspInsertProblemMaster_Result> uspInsertProblemMaster(string userID, string processName, Nullable<int> errorNumber, Nullable<int> lineNumber, string exceptionMessage, string exceptionSource, Nullable<int> exceptionSeverity, Nullable<int> exceptionState)
        {
            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));
    
            var processNameParameter = processName != null ?
                new ObjectParameter("ProcessName", processName) :
                new ObjectParameter("ProcessName", typeof(string));
    
            var errorNumberParameter = errorNumber.HasValue ?
                new ObjectParameter("ErrorNumber", errorNumber) :
                new ObjectParameter("ErrorNumber", typeof(int));
    
            var lineNumberParameter = lineNumber.HasValue ?
                new ObjectParameter("LineNumber", lineNumber) :
                new ObjectParameter("LineNumber", typeof(int));
    
            var exceptionMessageParameter = exceptionMessage != null ?
                new ObjectParameter("ExceptionMessage", exceptionMessage) :
                new ObjectParameter("ExceptionMessage", typeof(string));
    
            var exceptionSourceParameter = exceptionSource != null ?
                new ObjectParameter("ExceptionSource", exceptionSource) :
                new ObjectParameter("ExceptionSource", typeof(string));
    
            var exceptionSeverityParameter = exceptionSeverity.HasValue ?
                new ObjectParameter("ExceptionSeverity", exceptionSeverity) :
                new ObjectParameter("ExceptionSeverity", typeof(int));
    
            var exceptionStateParameter = exceptionState.HasValue ?
                new ObjectParameter("ExceptionState", exceptionState) :
                new ObjectParameter("ExceptionState", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<uspInsertProblemMaster_Result>("uspInsertProblemMaster", userIDParameter, processNameParameter, errorNumberParameter, lineNumberParameter, exceptionMessageParameter, exceptionSourceParameter, exceptionSeverityParameter, exceptionStateParameter);
        }
    
        public virtual int uspAgentDailyHours()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspAgentDailyHours");
        }
    
        public virtual int uspAssignPayPeriod()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspAssignPayPeriod");
        }
    
        public virtual int uspEmployeeHoursCalculation()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspEmployeeHoursCalculation");
        }
    
        public virtual int uspOverTimeHoursCalculation()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspOverTimeHoursCalculation");
        }
    
        public virtual int uspInsertEverquoteHourlyCallMetrics()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspInsertEverquoteHourlyCallMetrics");
        }
    
        public virtual int uspInsertPreciseLeadsHourlyCallLogMetrics()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspInsertPreciseLeadsHourlyCallLogMetrics");
        }
    
        public virtual int uspDailyPortfolioInventoryReport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspDailyPortfolioInventoryReport");
        }
    
        public virtual ObjectResult<uspInsertClientLeads_Result> uspInsertClientLeads(string recordType, string leadID, string clientName, Nullable<System.DateTime> arrivalTimestamp, Nullable<System.DateTime> preserviceTimestamp, Nullable<System.DateTime> postserviceTimestamp, string phoneNumber, string firstName, string lastName, string addressLine1, string addressLine2, string zipCode, string city, string state, string emailAddress, string leadType, string campaignName, string disposition, string callType, string agentID, string callHourOfDay, string skill, string listName, string customerCity, string customerState, string customerZipCode, string callID, string projectID, string patientID)
        {
            var recordTypeParameter = recordType != null ?
                new ObjectParameter("RecordType", recordType) :
                new ObjectParameter("RecordType", typeof(string));
    
            var leadIDParameter = leadID != null ?
                new ObjectParameter("LeadID", leadID) :
                new ObjectParameter("LeadID", typeof(string));
    
            var clientNameParameter = clientName != null ?
                new ObjectParameter("ClientName", clientName) :
                new ObjectParameter("ClientName", typeof(string));
    
            var arrivalTimestampParameter = arrivalTimestamp.HasValue ?
                new ObjectParameter("ArrivalTimestamp", arrivalTimestamp) :
                new ObjectParameter("ArrivalTimestamp", typeof(System.DateTime));
    
            var preserviceTimestampParameter = preserviceTimestamp.HasValue ?
                new ObjectParameter("PreserviceTimestamp", preserviceTimestamp) :
                new ObjectParameter("PreserviceTimestamp", typeof(System.DateTime));
    
            var postserviceTimestampParameter = postserviceTimestamp.HasValue ?
                new ObjectParameter("PostserviceTimestamp", postserviceTimestamp) :
                new ObjectParameter("PostserviceTimestamp", typeof(System.DateTime));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("PhoneNumber", phoneNumber) :
                new ObjectParameter("PhoneNumber", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var addressLine1Parameter = addressLine1 != null ?
                new ObjectParameter("AddressLine1", addressLine1) :
                new ObjectParameter("AddressLine1", typeof(string));
    
            var addressLine2Parameter = addressLine2 != null ?
                new ObjectParameter("AddressLine2", addressLine2) :
                new ObjectParameter("AddressLine2", typeof(string));
    
            var zipCodeParameter = zipCode != null ?
                new ObjectParameter("ZipCode", zipCode) :
                new ObjectParameter("ZipCode", typeof(string));
    
            var cityParameter = city != null ?
                new ObjectParameter("City", city) :
                new ObjectParameter("City", typeof(string));
    
            var stateParameter = state != null ?
                new ObjectParameter("State", state) :
                new ObjectParameter("State", typeof(string));
    
            var emailAddressParameter = emailAddress != null ?
                new ObjectParameter("EmailAddress", emailAddress) :
                new ObjectParameter("EmailAddress", typeof(string));
    
            var leadTypeParameter = leadType != null ?
                new ObjectParameter("LeadType", leadType) :
                new ObjectParameter("LeadType", typeof(string));
    
            var campaignNameParameter = campaignName != null ?
                new ObjectParameter("CampaignName", campaignName) :
                new ObjectParameter("CampaignName", typeof(string));
    
            var dispositionParameter = disposition != null ?
                new ObjectParameter("Disposition", disposition) :
                new ObjectParameter("Disposition", typeof(string));
    
            var callTypeParameter = callType != null ?
                new ObjectParameter("CallType", callType) :
                new ObjectParameter("CallType", typeof(string));
    
            var agentIDParameter = agentID != null ?
                new ObjectParameter("AgentID", agentID) :
                new ObjectParameter("AgentID", typeof(string));
    
            var callHourOfDayParameter = callHourOfDay != null ?
                new ObjectParameter("CallHourOfDay", callHourOfDay) :
                new ObjectParameter("CallHourOfDay", typeof(string));
    
            var skillParameter = skill != null ?
                new ObjectParameter("Skill", skill) :
                new ObjectParameter("Skill", typeof(string));
    
            var listNameParameter = listName != null ?
                new ObjectParameter("ListName", listName) :
                new ObjectParameter("ListName", typeof(string));
    
            var customerCityParameter = customerCity != null ?
                new ObjectParameter("CustomerCity", customerCity) :
                new ObjectParameter("CustomerCity", typeof(string));
    
            var customerStateParameter = customerState != null ?
                new ObjectParameter("CustomerState", customerState) :
                new ObjectParameter("CustomerState", typeof(string));
    
            var customerZipCodeParameter = customerZipCode != null ?
                new ObjectParameter("CustomerZipCode", customerZipCode) :
                new ObjectParameter("CustomerZipCode", typeof(string));
    
            var callIDParameter = callID != null ?
                new ObjectParameter("CallID", callID) :
                new ObjectParameter("CallID", typeof(string));
    
            var projectIDParameter = projectID != null ?
                new ObjectParameter("ProjectID", projectID) :
                new ObjectParameter("ProjectID", typeof(string));
    
            var patientIDParameter = patientID != null ?
                new ObjectParameter("PatientID", patientID) :
                new ObjectParameter("PatientID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<uspInsertClientLeads_Result>("uspInsertClientLeads", recordTypeParameter, leadIDParameter, clientNameParameter, arrivalTimestampParameter, preserviceTimestampParameter, postserviceTimestampParameter, phoneNumberParameter, firstNameParameter, lastNameParameter, addressLine1Parameter, addressLine2Parameter, zipCodeParameter, cityParameter, stateParameter, emailAddressParameter, leadTypeParameter, campaignNameParameter, dispositionParameter, callTypeParameter, agentIDParameter, callHourOfDayParameter, skillParameter, listNameParameter, customerCityParameter, customerStateParameter, customerZipCodeParameter, callIDParameter, projectIDParameter, patientIDParameter);
        }
    
        public virtual int AddEmployee()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddEmployee");
        }
    
        public virtual int FileNumber()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FileNumber");
        }
    
        public virtual int uspImportedEmployees()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspImportedEmployees");
        }
    
        public virtual int uspNewEmployeeData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspNewEmployeeData");
        }
    
        public virtual int uspHoursCalculation(Nullable<int> employeeID, Nullable<int> payPeriodID)
        {
            var employeeIDParameter = employeeID.HasValue ?
                new ObjectParameter("EmployeeID", employeeID) :
                new ObjectParameter("EmployeeID", typeof(int));
    
            var payPeriodIDParameter = payPeriodID.HasValue ?
                new ObjectParameter("PayPeriodID", payPeriodID) :
                new ObjectParameter("PayPeriodID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspHoursCalculation", employeeIDParameter, payPeriodIDParameter);
        }
    
        public virtual int uspOverTimeHoursCalculation1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspOverTimeHoursCalculation1");
        }
    }
}
