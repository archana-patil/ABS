using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mbpros.Models;
using Mbpros.Common;
using Mbpros.DAL;
using System.Text;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Mbpros.Controllers
{
    [SessionExpire]
    public class PatientController : Controller
    {  
        public ActionResult AddPatient(int patientID = 0, string callFrom = "")
        {

            MbprosEntities en = new MbprosEntities();
            var lstconfigkeys = en.Configs.ToList();

            PatientModel patientModel = new PatientModel();
            patientModel.CaptchaSitekeychallenge = "https://www.google.com/recaptcha/api/challenge?k=" + lstconfigkeys.Where(x => x.key == "CaptchaSitekey").FirstOrDefault().value;//ConfigurationManager.AppSettings["CaptchaSitekey"].ToString().Trim();
            patientModel.CaptchaSitekeynoscript = "https://www.google.com/recaptcha/api/noscript?k=" + lstconfigkeys.Where(x => x.key == "CaptchaSitekey").FirstOrDefault().value;//ConfigurationManager.AppSettings["CaptchaSitekey"].ToString().Trim();
            patientModel.CaptchaSecretkey = lstconfigkeys.Where(x => x.key == "CaptchaSecretkey").FirstOrDefault().value; //ConfigurationManager.AppSettings["CaptchaSecretkey"].ToString().Trim();

            patientModel.CallFrom = callFrom;
            patientModel.StateList = new SelectList(CommonFunctions.GetAllDropdownList("STATES"), "ID", "NAME", 0);
            int UserID = Session["USERID"] == null ? 0 : Convert.ToInt32(Session["USERID"]);
            if (patientID > 0)
            {
                MbprosEntities mbprosEntities = new MbprosEntities();
                var patientDetails = mbprosEntities.PatientMasters.Find(patientID);
                if (patientDetails != null)
                {
                    patientModel.PatientID = patientID;
                    if (patientDetails.CreatedDate != null)
                    {
                        patientModel.CreatedDate = Convert.ToDateTime(patientDetails.CreatedDate).ToString("MM/dd/yyyy");
                    }
                    patientModel.OfficeName = patientDetails.OfficeName;
                    patientModel.Active = patientDetails.Active;
                    patientModel.AdditionalInfo = patientDetails.AdditionalInfo;
                    patientModel.AdjusterName = patientDetails.AdjusterName;
                    if (patientDetails.AdmissionDate != null) patientModel.AdmissionDate = Convert.ToDateTime(patientDetails.AdmissionDate).ToString("MM/dd/yyyy");
                    patientModel.City = patientDetails.City;
                    if (patientDetails.DateofAccident != null) patientModel.DateofAccident = Convert.ToDateTime(patientDetails.DateofAccident).ToString("MM/dd/yyyy");
                    if (patientDetails.DateofBirth != null) patientModel.DateofBirth = Convert.ToDateTime(patientDetails.DateofBirth).ToString("MM/dd/yyyy");
                    patientModel.DiagnosisCodes = patientDetails.DiagnosisCodes;
                    patientModel.EDIPayerNumber = patientDetails.EDIPayerNumber;
                    patientModel.FacilityName = patientDetails.FacilityName;
                    patientModel.GroupNumber = patientDetails.GroupNumber;
                    if (patientDetails.InitialTreatmentDate != null) patientModel.InitialTreatmentDate = Convert.ToDateTime(patientDetails.InitialTreatmentDate).ToString("MM/dd/yyyy");
                    patientModel.InsuranceCompanyAddress = patientDetails.InsuranceCompanyAddress;
                    patientModel.InsuranceCompanyName = patientDetails.InsuranceCompanyName;
                    patientModel.InsuranceCompanyPhone = patientDetails.InsuranceCompanyPhone;
                    patientModel.InsuranceFax = patientDetails.InsuranceFax;
                    patientModel.IsConditionRelatedTo = patientDetails.ConditionRelatedTo;
                    patientModel.IsInsured = patientDetails.IsInsured ?? false;
                    //patientModel.IsPatInsured= patientDetails.ins;
                    if (patientModel.IsInsured == false)
                    {
                        patientModel.IsPatInsured = "NO";
                    }
                    else if (patientModel.IsInsured == true)
                    {
                        patientModel.IsPatInsured = "YES";
                    }

                    patientModel.IsSecondaryInsurance = patientDetails.IsSecondaryInsurance.ToString();
                    patientModel.IsVerifyEligibility = patientDetails.IsVerifyEligibility.ToString();
                    if (patientDetails.LastSeenByRefProvider != null) patientModel.LastSeenByRefProvider = Convert.ToDateTime(patientDetails.LastSeenByRefProvider).ToString("MM/dd/yyyy");
                    patientModel.LastXRayDateORPARTCodes = patientDetails.LastXRayDateORPARTCodes;
                    patientModel.PatientName = patientDetails.PatientName;
                    patientModel.PolicyID = patientDetails.PolicyID;
                    if (patientDetails.PrimaryInsuredDOB != null) patientModel.PrimaryInsuredDOB = Convert.ToDateTime(patientDetails.PrimaryInsuredDOB).ToString("MM/dd/yyyy");
                    patientModel.PrimaryInsuredName = patientDetails.PrimaryInsuredName;
                    patientModel.ReferringProviderName = patientDetails.ReferringProviderName;
                    patientModel.ReferringProviderNPI = patientDetails.ReferringProviderNPI;
                    patientModel.SecondaryCompanyAddr = patientDetails.SecondaryInsuranceCompanyAddr;
                    patientModel.SecondaryInsuranceCompanyName = patientDetails.SecondaryInsuranceCompanyName;
                    patientModel.SecondaryInsuranceCompanyPhone = patientDetails.SecondaryInsuranceCompanyPhone;
                    patientModel.SecondaryInsuranceGroupID = patientDetails.SecondaryInsuranceGroupID;
                    patientModel.SecondaryInsuranceID = patientDetails.SecondaryInsuranceID;
                    if (patientDetails.SecondaryInsuredDOB != null) patientModel.SecondaryInsuredDOB = Convert.ToDateTime(patientDetails.SecondaryInsuredDOB).ToString("MM/dd/yyyy");
                    patientModel.SecondaryInsuredName = patientDetails.SecondaryInsuredName;
                    patientModel.Sex = patientDetails.Sex;
                    patientModel.SSN = patientDetails.SSN;
                    patientModel.StateCode = patientDetails.StateCode;
                    patientModel.StreetAddress = patientDetails.StreetAddress;
                    patientModel.ZipCode = patientDetails.ZipCode;
                    //patientModel.InsVerification = patientDetails.InsuranceVerification;
                    var statecode = mbprosEntities.States.Where(x => x.StateCode == patientDetails.StateCode).FirstOrDefault();
                    if (statecode != null)
                        patientModel.StateList = new SelectList(CommonFunctions.GetAllDropdownList("STATES"), "ID", "NAME", statecode.StateID);
                }
            }
            else
            {
                if (UserID > 0) patientModel.OfficeName = CommonFunctions.GetUserOffice(UserID);
            }
            patientModel.UserID = UserID;
            return View(patientModel);
        }

        [HttpPost]
        public ActionResult AddPatient(PatientModel patientModel)
        {
            patientModel.Sex = Request.Form["hdnSex"];
            patientModel.IsPatInsured = Request.Form["hdnIsInsured"];
            patientModel.IsConditionRelatedTo = Request.Form["hdnIsConditionRelatedTo"];
            patientModel.IsSecondaryInsurance = Request.Form["hdnIsSecInsurance"];
            patientModel.IsVerifyEligibility = Request.Form["hdnIsVerify"];
            //        var errors = ModelState
            //.Where(x => x.Value.Errors.Count > 0)
            //.Select(x => new { x.Key, x.Value.Errors })
            //.ToArray();

            if (ModelState.IsValid)
            {
                var UserId = Convert.ToString(Session["USERID"]) == "" ? 0 : Convert.ToInt32(Session["USERID"]);
                // check captcha code is valid or not
                bool isCaptchaCodeValid = false;
                string CaptchaMessage = "";
                if (UserId > 0)
                {
                    isCaptchaCodeValid = true;
                }
                else
                {
                    isCaptchaCodeValid = CommonFunctions.GetCaptchaResponse(Request.Form["recaptcha_challenge_field"], Request.Form["recaptcha_response_field"], patientModel.CaptchaSecretkey.ToString(), out CaptchaMessage);
                }
                if (isCaptchaCodeValid)
                {
                    int patientID = 0;
                    using (MbprosEntities mbprosEntities = new MbprosEntities())
                    {
                        PatientMaster patientMaster = new PatientMaster();
                        if (patientModel.PatientID != 0)
                        {
                            patientMaster = mbprosEntities.PatientMasters.Find(patientModel.PatientID);
                            patientModel.Sex = Request.Form["hdnSex"];
                            patientModel.IsPatInsured = Request.Form["hdnIsInsured"];
                            patientModel.IsConditionRelatedTo = Request.Form["hdnIsConditionRelatedTo"];
                            patientModel.IsSecondaryInsurance = Request.Form["hdnIsSecInsurance"];
                            patientModel.IsVerifyEligibility = Request.Form["hdnIsVerify"];
                        }
                        patientMaster.OfficeName = patientModel.OfficeName;
                        patientMaster.PatientName = patientModel.PatientName;
                        patientMaster.StreetAddress = patientModel.StreetAddress;
                        patientMaster.City = patientModel.City;
                        patientMaster.StateCode = patientModel.StateCode;
                        patientMaster.ZipCode = patientModel.ZipCode;
                        if (!string.IsNullOrEmpty(patientModel.DateofBirth)) patientMaster.DateofBirth = DateTime.ParseExact(patientModel.DateofBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                        patientMaster.SSN = patientModel.SSN;
                        patientMaster.Sex = patientModel.Sex == "" ? "M" : patientModel.Sex;
                        patientMaster.InsuranceCompanyName = patientModel.InsuranceCompanyName;
                        patientMaster.InsuranceCompanyAddress = patientModel.InsuranceCompanyAddress;
                        patientMaster.InsuranceCompanyPhone = patientModel.InsuranceCompanyPhone;

                        patientMaster.EDIPayerNumber = patientModel.EDIPayerNumber;



                        patientMaster.PolicyID = patientModel.PolicyID;
                        patientMaster.GroupNumber = patientModel.GroupNumber;

                        if (patientModel.IsPatInsured == "NO")
                        {
                            patientMaster.PrimaryInsuredName = patientModel.PrimaryInsuredName;
                            if (!string.IsNullOrEmpty(patientModel.PrimaryInsuredDOB)) patientMaster.PrimaryInsuredDOB = DateTime.ParseExact(patientModel.PrimaryInsuredDOB, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                            patientMaster.IsInsured = false;
                        }
                        else if (patientModel.IsPatInsured == "YES")
                        {
                            patientMaster.IsInsured = true;
                        }

                        patientMaster.ConditionRelatedTo = patientModel.IsConditionRelatedTo;

                        if (!string.IsNullOrEmpty(patientModel.DateofAccident)) patientMaster.DateofAccident = DateTime.ParseExact(patientModel.DateofAccident, "MM/dd/yyyy", CultureInfo.InvariantCulture); 
                        patientMaster.InsuranceFax = patientModel.InsuranceFax;
                        patientMaster.AdjusterName = patientModel.AdjusterName;

                        if (patientModel.IsSecondaryInsurance == "True")// "Y")// IsSecInsurance == "YES")
                        {
                            patientMaster.SecondaryInsuranceCompanyName = patientModel.SecondaryInsuranceCompanyName;
                            patientMaster.SecondaryInsuranceCompanyAddr = patientModel.SecondaryCompanyAddr;
                            patientMaster.SecondaryInsuranceCompanyPhone = patientModel.SecondaryInsuranceCompanyPhone;
                            patientMaster.SecondaryInsuranceID = patientModel.SecondaryInsuranceID;
                            patientMaster.SecondaryInsuranceGroupID = patientModel.SecondaryInsuranceGroupID;
                            patientMaster.SecondaryInsuredName = patientModel.SecondaryInsuredName;

                            if (!string.IsNullOrEmpty(patientModel.SecondaryInsuredDOB)) patientMaster.SecondaryInsuredDOB = DateTime.ParseExact(patientModel.SecondaryInsuredDOB, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            patientMaster.IsSecondaryInsurance = true;
                        }
                        else
                        {
                            patientMaster.IsSecondaryInsurance = false;
                        }

                        patientMaster.IsVerifyEligibility = patientModel.IsVerifyEligibility == "True" ? true : false;
                        patientMaster.FacilityName = patientModel.FacilityName;
                        if (!string.IsNullOrEmpty(patientModel.AdmissionDate)) patientMaster.AdmissionDate = DateTime.ParseExact(patientModel.AdmissionDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(patientModel.LastSeenByRefProvider)) patientMaster.LastSeenByRefProvider = DateTime.ParseExact(patientModel.LastSeenByRefProvider, "MM/dd/yyyy", CultureInfo.InvariantCulture); 
                        patientMaster.ReferringProviderName = patientModel.ReferringProviderName;
                        patientMaster.ReferringProviderNPI = patientModel.ReferringProviderNPI;
                        if (!string.IsNullOrEmpty(patientModel.InitialTreatmentDate)) patientMaster.InitialTreatmentDate = DateTime.ParseExact(patientModel.InitialTreatmentDate, "MM/dd/yyyy", CultureInfo.InvariantCulture); 
                        patientMaster.LastXRayDateORPARTCodes = patientModel.LastXRayDateORPARTCodes;
                        patientMaster.DiagnosisCodes = patientModel.DiagnosisCodes;
                        patientMaster.AdditionalInfo = patientModel.AdditionalInfo;
                        //patientMaster.InsuranceVerification = patientModel.InsVerification;
                        patientMaster.Active = true;
                        //patientMaster.UserEmailID = patientModel.UserEmailID;
                        if (patientModel.PatientID == 0)
                        {
                            patientMaster.CreatedDate = DateTime.Now.Date;
                            patientMaster.CreatedBy = UserId;

                            mbprosEntities.PatientMasters.Add(patientMaster);
                            mbprosEntities.SaveChanges();
                            patientID = patientMaster.PatientID;
                        }
                        else
                        {
                            patientID = patientMaster.PatientID;
                            patientMaster.UpdatedDate = DateTime.Now;
                            patientMaster.UpdatedBy = UserId;
                            mbprosEntities.Entry(patientMaster).State = System.Data.Entity.EntityState.Modified;
                            mbprosEntities.SaveChanges();
                        }
                    }
                    ViewBag.CaptchaMessage = "";
                    if (patientModel.PatientID == 0)
                    {
                        return RedirectToAction("Confirmation", new { patientID = patientID, officeName = patientModel.OfficeName});
                    }
                    else
                    {
                        TempData["EditMessage"] = "Y";
                        return RedirectToAction(patientModel.CallFrom, "Search", new { editMessage = "Patient details updated successfully", IsBack = true });
                    }
                }
                else
                {
                    ViewBag.CaptchaMessage = CaptchaMessage;
                }
            }
            patientModel.StateList = new SelectList(CommonFunctions.GetAllDropdownList("STATES"), "ID", "NAME", 0);

            return View(patientModel);
        }

       // 
        //no need as it is just showing pdf to user so session checking is not needed.
        public ActionResult Confirmation(int patientID, string officeName)
        {
            CommonFunctions.SendEmail(officeName, "PATIENT_FORM");
            ViewBag.PatientID = patientID;
            return View();
        }

        public ActionResult PDFForPatient(int patientID)
        {
            MbprosEntities en = new MbprosEntities();
            //get the patient data with matching patient id
            PatientMaster objPatient = en.PatientMasters.Where(x => x.PatientID == patientID).FirstOrDefault();
            if (objPatient != null)
            {
                //Generates pdf for the patient data using PDFForPatient view.
                //return new Rotativa.ViewAsPdf("PDFForPatient", objPatient) { FileName = "PatientInformation.pdf" };
                return new Rotativa.ViewAsPdf("PDFForPatient", objPatient);
                // return View(objPatient);
            }
            return View(objPatient);
        }

        public JsonResult CheckSession()
        {
            string a = "";
            if (Session["USERNAME"] != null)
                a = Session["USERNAME"].ToString();
            return Json(a, JsonRequestBehavior.AllowGet);
        }
    }
}
