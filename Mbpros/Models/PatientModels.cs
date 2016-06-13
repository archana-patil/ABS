using Mbpros.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mbpros.Models
{
    public class PatientModel
    {
        public int PatientID { get; set; }
        public int draw { get; set; }
        
        [Required(ErrorMessage = "Please enter the office name")]
        public string OfficeName { get; set; }
        [Required(ErrorMessage = "Please enter the patient name")]
        public string PatientName { get; set; }
        [Required(ErrorMessage = "Please enter the patient street address name")]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "Please enter the patient city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please select the patient state")]
        public string StateCode { get; set; }
        [Required(ErrorMessage = "Please enter the patient zip code")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Please select date of birth")]
        [DataType(DataType.Date)]
        public string DateofBirth { get; set; }

        public string SSN { get; set; }
        [Required(ErrorMessage = "Please select patient sex")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Please enter the insurance company name")]
        public string InsuranceCompanyName { get; set; }
        public string InsuranceCompanyAddress { get; set; }
        

        [DataType(DataType.PhoneNumber)]
        public string InsuranceCompanyPhone { get; set; }
        public string EDIPayerNumber { get; set; }

        //[Range(0, Int32.MaxValue, ErrorMessage = "Invalid Number")]
        [Required(ErrorMessage = "Please enter the Policy ID #/Claim # ")]
        public string  PolicyID { get; set; }
        public string GroupNumber { get; set; }
        [Required(ErrorMessage = "Please select any one option")]
        public string IsPatInsured { get; set; }
        public bool IsInsured { get; set; }
        public string PrimaryInsuredName { get; set; }
        public string PrimaryInsuredDOB { get; set; }
        [Required(ErrorMessage = "Please select any one option")]
        public string IsConditionRelatedTo { get; set; }
        public string DateofAccident { get; set; }
        public string InsuranceFax { get; set; }
        public string AdjusterName { get; set; }

        //public string IsSecInsurance { get; set; }
        [Required(ErrorMessage = "Please select any one option")]
        public string IsSecondaryInsurance { get; set; }
        
        //[RequiredIf("IsSecInsurance", "YES", ErrorMessage = "Please enter the secondary insurance company name")]
        public string SecondaryInsuranceCompanyName { get; set; }
        public string SecondaryCompanyAddr { get; set; }
        public string SecondaryInsuranceCompanyPhone { get; set; }
        //[RequiredIf("IsSecInsurance", "YES", ErrorMessage = "Please enter the secondary insurance ID")]
        public string SecondaryInsuranceID { get; set; }
        public string SecondaryInsuranceGroupID { get; set; }
        //[RequiredIf("IsSecInsurance", "YES", ErrorMessage = "Please enter the secondary insured name")]
        public string SecondaryInsuredName { get; set; }
        //[RequiredIf("IsSecInsurance", "YES", ErrorMessage = "Please enter the secondary insured date of birth")]
        public string SecondaryInsuredDOB { get; set; }

        public string FacilityName { get; set; }
        public string AdmissionDate { get; set; }

        public string LastSeenByRefProvider { get; set; }
        public string ReferringProviderName { get; set; }
        //[RegularExpression(@"[0-9]+", ErrorMessage = "Please enter proper referring provider NPI.")]
        public string ReferringProviderNPI { get; set; }

        public string InitialTreatmentDate { get; set; }
        public string LastXRayDateORPARTCodes { get; set; }
        [Required(ErrorMessage = "Please enter diagnosis code")]
        public string DiagnosisCodes { get; set; }
        public string AdditionalInfo { get; set; }
        //public string InsVerification { get; set; }
        public bool Active { get; set; }
        //[Required(ErrorMessage = "Please enter the user email ID")]
        //[EmailAddress(ErrorMessage = "The User EmailID field is not a valid e-mail address.")]
        //public string UserEmailID { get; set; }
        //[Required(ErrorMessage = "Please enter the user name")]
        //public string UserName { get; set; }

        //public bool DispUserEmailID { get; set; }

        public int UserID { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public SelectList StateList { get; set; }
        public SelectList OptionList { get; set; }
        public int ArchiveOption { get; set; }
//[RegularExpression(@"^.{5,}$", ErrorMessage = "Minimum 4 characters required")]
//[StringLength(30, MinimumLength = 4)]


        public List<PatientMaster> PatientList { get; set; }

        public string CallFrom { get; set; }
        public string CaptchaSitekeychallenge { get; set; }
        public string CaptchaSitekeynoscript { get; set; }
        public string CaptchaSecretkey { get; set; }

        public int? FormId { get; set; }
        public string ServiceDate { get; set; }
        public string SubFrom { get; set; }
        public string SubTo { get; set; }

        [Required(ErrorMessage = "Please select any one option")]
        public string IsVerifyEligibility { get; set; }
    }

}