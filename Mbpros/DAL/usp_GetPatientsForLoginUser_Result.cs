//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mbpros.DAL
{
    using System;
    
    public partial class usp_GetPatientsForLoginUser_Result
    {
        public int PatientID { get; set; }
        public string OfficeName { get; set; }
        public string PatientName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string SSN { get; set; }
        public string Sex { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string InsuranceCompanyAddress { get; set; }
        public string InsuranceCompanyPhone { get; set; }
        public string EDIPayerNumber { get; set; }
        public string PolicyID { get; set; }
        public string GroupNumber { get; set; }
        public Nullable<bool> IsInsured { get; set; }
        public string PrimaryInsuredName { get; set; }
        public Nullable<System.DateTime> PrimaryInsuredDOB { get; set; }
        public string ConditionRelatedTo { get; set; }
        public Nullable<System.DateTime> DateofAccident { get; set; }
        public string InsuranceFax { get; set; }
        public string AdjusterName { get; set; }
        public Nullable<bool> IsSecondaryInsurance { get; set; }
        public string SecondaryInsuranceCompanyName { get; set; }
        public string SecondaryInsuranceCompanyAddr { get; set; }
        public string SecondaryInsuranceCompanyPhone { get; set; }
        public string SecondaryInsuranceID { get; set; }
        public string SecondaryInsuranceGroupID { get; set; }
        public string SecondaryInsuredName { get; set; }
        public Nullable<System.DateTime> SecondaryInsuredDOB { get; set; }
        public string FacilityName { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public Nullable<System.DateTime> LastSeenByRefProvider { get; set; }
        public string ReferringProviderName { get; set; }
        public string ReferringProviderNPI { get; set; }
        public Nullable<System.DateTime> InitialTreatmentDate { get; set; }
        public string LastXRayDateORPARTCodes { get; set; }
        public string DiagnosisCodes { get; set; }
        public string AdditionalInfo { get; set; }
        public bool Active { get; set; }
        public bool IsArchived { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
