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
    
    public partial class CustomerContact
    {
        public int ContactID { get; set; }
        public short CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public int AddByEmplloyeeID { get; set; }
        public System.DateTime DeactiveDate { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
