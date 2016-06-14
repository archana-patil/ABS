using Mbpros.Common;
using Mbpros.DAL;
using Mbpros.Models;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mbpros.Controllers
{
    [SessionExpire]
    public class BillingController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BillingController));  //Declaring Log4Net
        //////////////////////this is my change
        public ActionResult sdfdsf()
        {
            return dsh;
        }

        public ActionResult BillingLog(int billingID = 0, string callFrom = "")
        {

            MbprosEntities en = new MbprosEntities();
            var lstconfigkeys = en.Configs.ToList();

            BillingModel billingModel = new BillingModel();
            billingModel.CaptchaSitekeychallenge = "https://www.google.com/recaptcha/api/challenge?k=" + lstconfigkeys.Where(x => x.key == "CaptchaSitekey").FirstOrDefault().value;//ConfigurationManager.AppSettings["CaptchaSitekey"].ToString().Trim();
            billingModel.CaptchaSitekeynoscript = "https://www.google.com/recaptcha/api/noscript?k=" + lstconfigkeys.Where(x => x.key == "CaptchaSitekey").FirstOrDefault().value;//ConfigurationManager.AppSettings["CaptchaSitekey"].ToString().Trim();
            billingModel.CaptchaSecretkey = lstconfigkeys.Where(x => x.key == "CaptchaSecretkey").FirstOrDefault().value; //ConfigurationManager.AppSettings["CaptchaSecretkey"].ToString().Trim();

            billingModel.CallFrom = callFrom;
            if (billingID > 0)
            {
                MbprosEntities mbprosEntities = new MbprosEntities();
                var billDetails = mbprosEntities.PatientBillings.Where(x => x.PatientBillingID == billingID);
                if (billDetails != null)
                {
                    billingModel.OfficeName = billDetails.FirstOrDefault().OfficeName;
                    billingModel.AdditionalComments = billDetails.FirstOrDefault().Note;
                    billingModel.BillingID = billingID;
                    if (billDetails.FirstOrDefault().CreatedDate != null)
                    {
                        billingModel.CreatedDate = Convert.ToDateTime(billDetails.FirstOrDefault().CreatedDate).ToString("MM/dd/yyyy");
                    }
                    foreach (var a in billDetails)
                    {
                        if (a.SrNo == 1)
                        {
                            billingModel.ChequeNo1 = a.ChequeNo;
                            if (a.CopayPaid !=null)  billingModel.CopayPaid1 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes1 = a.DXCode;
                            billingModel.PatientName1 = a.PatientName;
                            billingModel.ProcedureCodes1 = a.ProcedureCodes;
                            billingModel.CopayType1 = a.CoPayType;
                            billingModel.ServiceDate1 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo1 = a.SrNo;
                        }
                        else if (a.SrNo == 2)
                        {
                            billingModel.ChequeNo2 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid2 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes2 = a.DXCode;
                            billingModel.PatientName2 = a.PatientName;
                            billingModel.ProcedureCodes2 = a.ProcedureCodes;
                            billingModel.ServiceDate2 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo2 = a.SrNo;
                            billingModel.CopayType2 = a.CoPayType;
                        }
                        else if (a.SrNo == 3)
                        {
                            billingModel.ChequeNo3 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid3 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes3 = a.DXCode;
                            billingModel.PatientName3 = a.PatientName;
                            billingModel.ProcedureCodes3 = a.ProcedureCodes;
                            billingModel.ServiceDate3 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo3 = a.SrNo;
                            billingModel.CopayType3 = a.CoPayType;
                        }
                        else if (a.SrNo == 4)
                        {
                            billingModel.ChequeNo4 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid4 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes4 = a.DXCode;
                            billingModel.PatientName4 = a.PatientName;
                            billingModel.ProcedureCodes4 = a.ProcedureCodes;
                            billingModel.ServiceDate4 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo4 = a.SrNo;
                            billingModel.CopayType4 = a.CoPayType;
                        }
                        else if (a.SrNo == 5)
                        {
                            billingModel.ChequeNo5 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid5 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes5 = a.DXCode;
                            billingModel.PatientName5 = a.PatientName;
                            billingModel.ProcedureCodes5 = a.ProcedureCodes;
                            billingModel.ServiceDate5 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo5 = a.SrNo;
                            billingModel.CopayType5 = a.CoPayType;
                        }
                        else if (a.SrNo == 6)
                        {
                            billingModel.ChequeNo6 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid6 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes6 = a.DXCode;
                            billingModel.PatientName6 = a.PatientName;
                            billingModel.ProcedureCodes6 = a.ProcedureCodes;
                            billingModel.ServiceDate6 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo6 = a.SrNo;
                            billingModel.CopayType6 = a.CoPayType;
                        }
                        else if (a.SrNo == 7)
                        {
                            billingModel.ChequeNo7 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid7 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes7 = a.DXCode;
                            billingModel.PatientName7 = a.PatientName;
                            billingModel.ProcedureCodes7 = a.ProcedureCodes;
                            billingModel.ServiceDate7 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo7 = a.SrNo;
                            billingModel.CopayType7 = a.CoPayType;
                        }
                        else if (a.SrNo == 8)
                        {
                            billingModel.ChequeNo8 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid8 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes8 = a.DXCode;
                            billingModel.PatientName8 = a.PatientName;
                            billingModel.ProcedureCodes8 = a.ProcedureCodes;
                            billingModel.ServiceDate8 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo8 = a.SrNo;
                            billingModel.CopayType8 = a.CoPayType;
                        }
                        else if (a.SrNo == 9)
                        {
                            billingModel.ChequeNo9 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid9 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes9 = a.DXCode;
                            billingModel.PatientName9 = a.PatientName;
                            billingModel.ProcedureCodes9 = a.ProcedureCodes;
                            billingModel.ServiceDate9 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo9 = a.SrNo;
                            billingModel.CopayType9 = a.CoPayType;
                        }
                        else if (a.SrNo == 10)
                        {
                            billingModel.ChequeNo10 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid10 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes10 = a.DXCode;
                            billingModel.PatientName10 = a.PatientName;
                            billingModel.ProcedureCodes10 = a.ProcedureCodes;
                            billingModel.ServiceDate10 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo10 = a.SrNo;
                            billingModel.CopayType10 = a.CoPayType;
                        }
                        else if (a.SrNo == 11)
                        {
                            billingModel.ChequeNo11 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid11 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes11 = a.DXCode;
                            billingModel.PatientName11 = a.PatientName;
                            billingModel.ProcedureCodes11 = a.ProcedureCodes;
                            billingModel.ServiceDate11 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo11 = a.SrNo;
                            billingModel.CopayType11 = a.CoPayType;
                        }
                        else if (a.SrNo == 12)
                        {
                            billingModel.ChequeNo12 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid12 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes12 = a.DXCode;
                            billingModel.PatientName12 = a.PatientName;
                            billingModel.ProcedureCodes12 = a.ProcedureCodes;
                            billingModel.ServiceDate12 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo12 = a.SrNo;
                            billingModel.CopayType12 = a.CoPayType;
                        }
                        else if (a.SrNo == 13)
                        {
                            billingModel.ChequeNo13 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid13 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes13 = a.DXCode;
                            billingModel.PatientName13 = a.PatientName;
                            billingModel.ProcedureCodes13 = a.ProcedureCodes;
                            billingModel.ServiceDate13 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo13 = a.SrNo;
                            billingModel.CopayType13 = a.CoPayType;
                        }
                        else if (a.SrNo == 14)
                        {
                            billingModel.ChequeNo14 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid14 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes14 = a.DXCode;
                            billingModel.PatientName14 = a.PatientName;
                            billingModel.ProcedureCodes14 = a.ProcedureCodes;
                            billingModel.ServiceDate14 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo14 = a.SrNo;
                            billingModel.CopayType14 = a.CoPayType;
                        }
                        else if (a.SrNo == 15)
                        {
                            billingModel.ChequeNo15 = a.ChequeNo;
                            if (a.CopayPaid != null) billingModel.CopayPaid15 = Convert.ToDouble(a.CopayPaid);
                            billingModel.NewDXCodes15 = a.DXCode;
                            billingModel.PatientName15 = a.PatientName;
                            billingModel.ProcedureCodes15 = a.ProcedureCodes;
                            billingModel.ServiceDate15 = Convert.ToDateTime(a.ServiceDate).ToString("MM/dd/yyyy");
                            billingModel.SrNo15 = a.SrNo;
                            billingModel.CopayType15 = a.CoPayType;
                        }
                    }
                }
            }
            else
            {
                int UserID = Session["USERID"] == null ? 0 : Convert.ToInt32(Session["USERID"]);
                if (UserID > 0) billingModel.OfficeName = CommonFunctions.GetUserOffice(UserID);
            }
            return View(billingModel);
        }

        [HttpPost]
        public ActionResult BillingLog(BillingModel billingModel)
        {
            
            if (ModelState.IsValid)
            {
                var UserId = Convert.ToString(Session["USERID"]) == "" ? 0 : Convert.ToInt32(Session["USERID"]);
                // check captcha code is valid or not
                bool isCaptchaCodeValid = false;
                string ReCaptchaMessage = "";
                if (billingModel.BillingID == 0)// Code for Add Billing details
                {
                    if (UserId > 0)
                    {
                        isCaptchaCodeValid = true;
                    }
                    else
                    {
                        isCaptchaCodeValid = CommonFunctions.GetCaptchaResponse(Request.Form["recaptcha_challenge_field"], Request.Form["recaptcha_response_field"], billingModel.CaptchaSecretkey.ToString(), out ReCaptchaMessage);
                    }
                    if (isCaptchaCodeValid)
                    {
                        int patientBillingID = 0;
                        using (MbprosEntities mbprosEntities = new MbprosEntities())
                        {
                            patientBillingID = Common.CommonFunctions.GetNextPatientBillingID();
                            for (int i = 1; i <= 15; i++)
                            {
                                if (!string.IsNullOrEmpty(Request.Form["PatientName" + i]))
                                {
                                    PatientBilling patientBilling = new PatientBilling();
                                    patientBilling.PatientBillingID = patientBillingID;
                                    patientBilling.OfficeName = billingModel.OfficeName;
                                    patientBilling.PatientName = Request.Form["PatientName" + i];
                                    if (!string.IsNullOrEmpty(Request.Form["ServiceDate" + i])) patientBilling.ServiceDate = DateTime.ParseExact(Request.Form["ServiceDate" + i], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    patientBilling.CoPayType = Request.Form["CopayType" + i];
                                    if (!string.IsNullOrEmpty(patientBilling.CoPayType))
                                    {
                                        patientBilling.ChequeNo = Request.Form["ChequeNo" + i];
                                        patientBilling.CopayPaid = Convert.ToDecimal(Request.Form["CopayPaid" + i]);
                                    }
                                    patientBilling.ProcedureCodes = Request.Form["ProcedureCodes" + i];
                                    patientBilling.DXCode = Request.Form["NewDXCodes" + i];
                                    patientBilling.SrNo = i;

                                    patientBilling.CreatedDate = DateTime.Now.Date;
                                    patientBilling.CreatedBy = UserId;
                                    patientBilling.Active = true;
                                    patientBilling.IsArchived = false;
                                    patientBilling.Note = billingModel.AdditionalComments;

                                    mbprosEntities.PatientBillings.Add(patientBilling);
                                }
                            }

                            mbprosEntities.SaveChanges();
                        }
                        ViewBag.ReCaptchaMessage = "";
                        return RedirectToAction("Confirmation","Billing", new { billingFormID = patientBillingID, officeName = billingModel.OfficeName });
                    }
                    else
                    {
                        ViewBag.ReCaptchaMessage = ReCaptchaMessage;
                    }
                }
                else// Code for edit Billing details
                {
                    // here we will have to add 3 codes first for edit existing , other for add new if not exist in DB and one for for deleting if user deletes exisitng record.
                    MbprosEntities mbprosEntities = new MbprosEntities();
                    var billDetails = mbprosEntities.PatientBillings.Where(x => x.PatientBillingID == billingModel.BillingID);
                    int patientBillingID = 0;
                    for (int i = 1; i <= 15; i++)
                    {
                        PatientBilling objBilling = billDetails.Where(x => x.SrNo == i).FirstOrDefault();
                        if (!string.IsNullOrEmpty(Request.Form["PatientName" + i]))
                        {
                            if (objBilling != null) // it means update the record.
                            {
                                if (i == 1) patientBillingID = objBilling.PatientBillingID;
                                objBilling.OfficeName = billingModel.OfficeName;
                                objBilling.PatientName = Request.Form["PatientName" + i];
                                if (!string.IsNullOrEmpty(Request.Form["ServiceDate" + i])) objBilling.ServiceDate = DateTime.ParseExact(Request.Form["ServiceDate" + i], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                objBilling.CoPayType = Request.Form["CopayType" + i];
                                if (!string.IsNullOrEmpty(objBilling.CoPayType))
                                {
                                    objBilling.ChequeNo = Request.Form["ChequeNo" + i];
                                    objBilling.CopayPaid = Convert.ToDecimal(Request.Form["CopayPaid" + i]);
                                }
                                objBilling.ProcedureCodes = Request.Form["ProcedureCodes" + i];
                                objBilling.DXCode = Request.Form["NewDXCodes" + i];
                                objBilling.SrNo = i;

                                objBilling.Active = true;
                                objBilling.Note = billingModel.AdditionalComments;

                                objBilling.BillingFormID = objBilling.BillingFormID;
                                objBilling.UpdatedDate = DateTime.Now;
                                objBilling.UpdatedBy = UserId;
                                mbprosEntities.Entry(objBilling).State = System.Data.Entity.EntityState.Modified;
                                mbprosEntities.SaveChanges();
                            }
                            else
                            { // 2 conditions add
                                PatientBilling patientBilling = new PatientBilling();
                                patientBilling.PatientBillingID = patientBillingID;
                                patientBilling.OfficeName = billingModel.OfficeName;
                                patientBilling.PatientName = Request.Form["PatientName" + i];
                                if (!string.IsNullOrEmpty(Request.Form["ServiceDate" + i])) patientBilling.ServiceDate = DateTime.ParseExact(Request.Form["ServiceDate" + i], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                patientBilling.CoPayType = Request.Form["CopayType" + i];
                                if (!string.IsNullOrEmpty(patientBilling.CoPayType))
                                {
                                    patientBilling.ChequeNo = Request.Form["ChequeNo" + i];
                                    patientBilling.CopayPaid = Convert.ToDecimal(Request.Form["CopayPaid" + i]);
                                }
                                patientBilling.ProcedureCodes = Request.Form["ProcedureCodes" + i];
                                patientBilling.DXCode = Request.Form["NewDXCodes" + i];
                                patientBilling.SrNo = i;

                                patientBilling.Active = true;
                                patientBilling.Note = billingModel.AdditionalComments;
                                patientBilling.CreatedDate = DateTime.Now.Date;
                                patientBilling.CreatedBy = UserId;
                                patientBilling.IsArchived = false;
                                mbprosEntities.PatientBillings.Add(patientBilling);
                                mbprosEntities.SaveChanges();
                            }
                        }
                        else
                        {
                            if (objBilling != null) // it means delete the record.
                            {
                                mbprosEntities.PatientBillings.Attach(objBilling);
                                mbprosEntities.PatientBillings.Remove(objBilling);
                                mbprosEntities.SaveChanges();
                            }
                        }
                    }
                    TempData["EditMessage"] = "Y";
                    return RedirectToAction(billingModel.CallFrom, "Search", new { editMessage = "Patient billing details updated successfully", IsBack = true });
                }

            }

            return View(billingModel);
        }

        
        public ActionResult Confirmation(int billingFormID, string officeName)
        {
            try
            {
                CommonFunctions.SendEmail(officeName, "BILLING_FORM");
            }
            catch (Exception ex)
            {
                // log.WriteLog("Page= addeditevent.aspx" + DateTime.Now + "\n" + ex.Message + "\n" + ex.StackTrace);
                logger.Error(ex.ToString());
                logger.Info("Action send mail has been fired.");
            }
            ViewBag.BillingFormID = billingFormID;
            return View();
        }

        /// <summary>
        /// Generate PDF for patient billing form
        /// </summary>
        /// <param name="billingID">Billing Id</param>
        /// <returns>PDF File</returns>        /// 
        public ActionResult PDFForPatientBilling(int billingID)
        {
            MbprosEntities en = new MbprosEntities();
            //get the patient billing data with matching billing form id
            List<PatientBilling> objPatientBilling = en.PatientBillings.Where(x => x.PatientBillingID == billingID).ToList();
            if (objPatientBilling != null)
            {
                //Generates pdf for the patient billing data using PDFForPatientBilling view.
                return new Rotativa.ViewAsPdf("PDFForPatientBilling", objPatientBilling)
                {
                    PageOrientation = Orientation.Landscape
                };
            }

            return View(objPatientBilling);
        }
    }
}
