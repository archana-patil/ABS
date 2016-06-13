using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mbpros.Models
{
    public class BillingModel
    {
        public int BillingID { get; set; }
        public int PatientID { get; set; }
        public string CreatedDate { get; set; }

        [Required(ErrorMessage = "Please enter the office name")]
        public string OfficeName { get; set; }

        [Required(ErrorMessage = "Please enter the patient name")]
        public string PatientName1 { get; set; }
        [Required(ErrorMessage = "Please enter date of service")]
        public string ServiceDate1 { get; set; }
        public string CopayType1 { get; set; }
        public string ChequeNo1 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid1 { get; set; }
        [Required(ErrorMessage = "Please enter procedure codes")]
        public string ProcedureCodes1 { get; set; }
        //[Required(ErrorMessage = "Please enter new DX codes")]
        public string NewDXCodes1 { get; set; }
        public int SrNo1 { get; set; }

        public string PatientName2 { get; set; }
        public string ServiceDate2 { get; set; }
        public string CopayType2 { get; set; }
        public string ChequeNo2 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid2 { get; set; }
        public string ProcedureCodes2 { get; set; }
        public string NewDXCodes2 { get; set; }
        public int SrNo2 { get; set; }

        public string PatientName3 { get; set; }
        public string ServiceDate3 { get; set; }
        public string CopayType3 { get; set; }
        public string ChequeNo3 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid3 { get; set; }
        public string ProcedureCodes3 { get; set; }
        public string NewDXCodes3 { get; set; }
        public int SrNo3 { get; set; }

        public string PatientName4 { get; set; }
        public string ServiceDate4 { get; set; }
        public string CopayType4 { get; set; }
        public string ChequeNo4 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid4 { get; set; }
        public string ProcedureCodes4 { get; set; }
        public string NewDXCodes4 { get; set; }
        public int SrNo4 { get; set; }

        public string PatientName5 { get; set; }
        public string ServiceDate5 { get; set; }
        public string CopayType5 { get; set; }
        public string ChequeNo5 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid5 { get; set; }
        public string ProcedureCodes5 { get; set; }
        public string NewDXCodes5 { get; set; }
        public int SrNo5 { get; set; }

        public string PatientName6 { get; set; }
        public string ServiceDate6 { get; set; }
        public string CopayType6 { get; set; }
        public string ChequeNo6 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid6 { get; set; }
        public string ProcedureCodes6 { get; set; }
        public string NewDXCodes6 { get; set; }
        public int SrNo6 { get; set; }

        public string PatientName7 { get; set; }
        public string ServiceDate7 { get; set; }
        public string CopayType7 { get; set; }
        public string ChequeNo7 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid7 { get; set; }
        public string ProcedureCodes7 { get; set; }
        public string NewDXCodes7 { get; set; }
        public int SrNo7 { get; set; }

        public string PatientName8 { get; set; }
        public string ServiceDate8 { get; set; }
        public string CopayType8 { get; set; }
        public string ChequeNo8 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid8 { get; set; }
        public string ProcedureCodes8 { get; set; }
        public string NewDXCodes8 { get; set; }
        public int SrNo8 { get; set; }

        public string PatientName9 { get; set; }
        public string ServiceDate9 { get; set; }
        public string CopayType9 { get; set; }
        public string ChequeNo9 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid9 { get; set; }
        public string ProcedureCodes9 { get; set; }
        public string NewDXCodes9 { get; set; }
        public int SrNo9 { get; set; }

        public string PatientName10 { get; set; }
        public string ServiceDate10 { get; set; }
        public string CopayType10 { get; set; }
        public string ChequeNo10 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid10 { get; set; }
        public string ProcedureCodes10 { get; set; }
        public string NewDXCodes10 { get; set; }
        public int SrNo10 { get; set; }

        public string PatientName11 { get; set; }
        public string ServiceDate11 { get; set; }
        public string CopayType11 { get; set; }
        public string ChequeNo11 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid11 { get; set; }
        public string ProcedureCodes11 { get; set; }
        public string NewDXCodes11 { get; set; }
        public int SrNo11 { get; set; }

        public string PatientName12 { get; set; }
        public string ServiceDate12 { get; set; }
        public string CopayType12 { get; set; }
        public string ChequeNo12 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid12 { get; set; }
        public string ProcedureCodes12 { get; set; }
        public string NewDXCodes12 { get; set; }
        public int SrNo12 { get; set; }

        public string PatientName13 { get; set; }
        public string ServiceDate13 { get; set; }
        public string CopayType13 { get; set; }
        public string ChequeNo13 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid13 { get; set; }
        public string ProcedureCodes13 { get; set; }
        public string NewDXCodes13 { get; set; }
        public int SrNo13 { get; set; }

        public string PatientName14 { get; set; }
        public string ServiceDate14 { get; set; }
        public string CopayType14 { get; set; }
        public string ChequeNo14 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        public double? CopayPaid14 { get; set; }
        public string ProcedureCodes14 { get; set; }
        public string NewDXCodes14 { get; set; }
        public int SrNo14 { get; set; }

        public string PatientName15 { get; set; }
        public string ServiceDate15 { get; set; }
        public string CopayType15 { get; set; }
        public string ChequeNo15 { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "CopayPaid can't have more than 2 decimal places")]
        [Range(typeof(double), "0.01", "9999999.99", ErrorMessage = "CopayPaid must be a number between {1} and {2}.")]
        [Display(Name = "CopayPaid")]
        public double? CopayPaid15 { get; set; }
        public string ProcedureCodes15 { get; set; }
        public string NewDXCodes15 { get; set; }
        public int SrNo15 { get; set; }
        public string AdditionalComments { get; set; }

        public List<DAL.PatientBilling> BillingList { get; set; }

        public string CallFrom { get; set; }
        public string CaptchaSitekeychallenge { get; set; }
        public string CaptchaSitekeynoscript { get; set; }
        public string CaptchaSecretkey { get; set; }

        public SelectList OptionList { get; set; }

        public int ArchiveOption { get; set; }
        public string PatientName { get; set; }
        public int? FormId { get; set; }
        public string ServiceDate { get; set; }
        public string SubFrom { get; set; }
        public string SubTo { get; set; }
        

    }
}