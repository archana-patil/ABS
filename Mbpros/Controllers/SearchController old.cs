using Mbpros.Common;
using Mbpros.DAL;
using Mbpros.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mbpros.Controllers
{
    [SessionExpire]
    public class SearchController : Controller
    {
        MbprosEntities mbprosEntities = new MbprosEntities();

        public ActionResult PatientSearch(string editMessage, bool IsBack = false)
        {
            PatientModel patientModel = new PatientModel();

            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;

            if (IsBack)
            {
                //Get & Set search criteria model
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SearchCriteria"])))
                {
                    string[] SearchCriteria = Session["SearchCriteria"].ToString().Split(",".ToArray());
                    for (int i = 0; i < SearchCriteria.Count(); i++)
                    {
                        string[] keyValues = SearchCriteria[i].Split("=".ToArray());
                        switch (keyValues[0])
                        {
                            case "PatientName":
                                patientModel.PatientName = keyValues[1];
                                break;
                            case "FormId":
                                if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                                { patientModel.FormId = Convert.ToInt32(keyValues[1]); }
                                break;
                            case "SSN":
                                patientModel.SSN = keyValues[1];
                                break;
                            case "PolicyID":
                                patientModel.PolicyID = keyValues[1];
                                break;
                            case "SubFrom":
                                patientModel.SubFrom = keyValues[1];
                                break;
                            case "SubTo":
                                patientModel.SubTo = keyValues[1];
                                break;
                            case "DateofBirth":
                                patientModel.DateofBirth = keyValues[1];
                                break;
                        }
                    }
                }                
                patientModel.PatientList = GetPatientSearch(patientModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
                patientModel.PatientList = GetPatientSearch(null);
            }
            return View(patientModel);
        }

        public List<PatientMaster> GetPatientSearch(PatientModel patientModel)
        {

            var UserId = Convert.ToString(Session["USERID"]) == "" ? 0 : Convert.ToInt32(Session["USERID"]);
            string officename = Common.CommonFunctions.GetUserOffice(UserId);
            if (patientModel == null) patientModel = new PatientModel();

            patientModel.PatientList = (from p in mbprosEntities.PatientMasters
                                        //join fu in mbprosEntities.Feature_User.Where(k => k.CanView == true) on p.CreatedBy equals fu.UserID
                                        //join fm in mbprosEntities.FeatureMasters.Where(k => k.FeatureName == "PATIENT_FORM") on fu.FeatureID equals fm.FeatureID
                                        join u in mbprosEntities.Users on p.CreatedBy equals u.UserId into pu
                                        from u in pu.DefaultIfEmpty()
                                        where p.OfficeName == officename && p.Active == true
                                        select p).OrderByDescending(o => o.PatientID).ToList();

            if (patientModel != null)
            {
                if (!string.IsNullOrEmpty(patientModel.PatientName))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.PatientName.ToLower().Contains(patientModel.PatientName.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(patientModel.FormId)))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.PatientID == Convert.ToInt32(patientModel.FormId)).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.SSN))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.SSN != null && x.SSN.Contains(patientModel.SSN)).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.PolicyID))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.PolicyID.ToLower().Contains(patientModel.PolicyID.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.SubFrom))
                {
                    DateTime pDate = DateTime.ParseExact(patientModel.SubFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.CreatedDate >= pDate).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.SubTo))
                {
                    DateTime pDate = DateTime.ParseExact(patientModel.SubTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.CreatedDate <= pDate).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.DateofBirth))
                {
                    DateTime pDate = DateTime.ParseExact(patientModel.DateofBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.DateofBirth == pDate).ToList();
                }
            }
            return patientModel.PatientList;
        }

        public ActionResult BillingSearch(string editMessage, bool IsBack = false)
        {
            BillingModel billingModel = new BillingModel();

            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;
            if (IsBack)
            {
                //Get & Set search criteria model
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SearchCriteria"])))
                {
                    string[] SearchCriteria = Session["SearchCriteria"].ToString().Split(",".ToArray());
                    for (int i = 0; i < SearchCriteria.Count(); i++)
                    {
                        string[] keyValues = SearchCriteria[i].Split("=".ToArray());
                        switch (keyValues[0])
                        {
                            case "PatientName":
                                billingModel.PatientName = keyValues[1];
                                break;
                            case "FormId":
                                if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                                { billingModel.FormId = Convert.ToInt32(keyValues[1]); }
                                break;
                            case "ServiceDate":
                                billingModel.ServiceDate = keyValues[1];
                                break;
                            case "SubFrom":
                                billingModel.SubFrom = keyValues[1];
                                break;
                            case "SubTo":
                                billingModel.SubTo = keyValues[1];
                                break;
                        }
                    }
                }
                billingModel.BillingList = GetBillingSearch(billingModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
                billingModel.BillingList = GetBillingSearch(null);
            }
            return View(billingModel);
        }

        public List<PatientBilling> GetBillingSearch(BillingModel billingModel)
        {

            var UserId = Convert.ToString(Session["USERID"]) == "" ? 0 : Convert.ToInt32(Session["USERID"]);
            string officename = Common.CommonFunctions.GetUserOffice(UserId);
            if (billingModel == null) billingModel = new BillingModel();

            billingModel.BillingList = (from p in mbprosEntities.PatientBillings
                                        //join fu in mbprosEntities.Feature_User.Where(k => k.CanView == true) on p.CreatedBy equals fu.UserID
                                        //join fm in mbprosEntities.FeatureMasters.Where(k => k.FeatureName == "BILLING_FORM") on fu.FeatureID equals fm.FeatureID
                                        join u in mbprosEntities.Users on p.CreatedBy equals u.UserId into pu
                                        from u in pu.DefaultIfEmpty()
                                        where p.OfficeName == officename && p.Active == true && p.SrNo == 1
                                        select p).OrderByDescending(o => o.PatientBillingID).ToList();

            if (billingModel != null)
            {
                if (!string.IsNullOrEmpty(billingModel.PatientName))
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.PatientName.ToLower().Contains(billingModel.PatientName.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(billingModel.FormId)))
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.PatientBillingID == Convert.ToInt32(billingModel.FormId)).ToList();
                }
                if (!string.IsNullOrEmpty(billingModel.ServiceDate))
                {
                    DateTime pDate = DateTime.ParseExact(billingModel.ServiceDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.ServiceDate == pDate).ToList();
                }               
                if (!string.IsNullOrEmpty(billingModel.SubFrom))
                {
                    DateTime pDate = DateTime.ParseExact(billingModel.SubFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.CreatedDate >= pDate).ToList();
                }
                if (!string.IsNullOrEmpty(billingModel.SubTo))
                {
                    DateTime pDate = DateTime.ParseExact(billingModel.SubTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.CreatedDate <= pDate).ToList();
                }
            }
            return billingModel.BillingList;
        }

        public ActionResult PatientSearchForAdmin(string editMessage, bool IsBack = false)
        {
            PatientModel patientModel = new PatientModel();

            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;

            if (IsBack)
            {
                //Get & Set search criteria model
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SearchCriteria"])))
                {
                    string[] SearchCriteria = Session["SearchCriteria"].ToString().Split(",".ToArray());
                    for (int i = 0; i < SearchCriteria.Count(); i++)
                    {
                        string[] keyValues = SearchCriteria[i].Split("=".ToArray());
                        switch (keyValues[0])
                        {
                            case "OfficeName":
                                patientModel.OfficeName = keyValues[1];
                                break;
                            case "PatientName":
                                patientModel.PatientName = keyValues[1];
                                break;
                            case "FormId":
                                if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                                { patientModel.FormId = Convert.ToInt32(keyValues[1]); }
                                break;
                            case "SSN":
                                patientModel.SSN = keyValues[1];
                                break;
                            case "PolicyID":
                                patientModel.PolicyID = keyValues[1];
                                break;
                            case "SubFrom":
                                patientModel.SubFrom = keyValues[1];
                                break;
                            case "SubTo":
                                patientModel.SubTo = keyValues[1];
                                break;
                            case "ArchiveOption":
                                patientModel.ArchiveOption = Convert.ToInt32(keyValues[1]);
                                break;
                        }
                    }
                }
                //patientModel.PatientList = GetPatientSearchForAdmin(patientModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
                //patientModel.PatientList = GetPatientSearchForAdmin(null);
            }
            patientModel.OptionList = new SelectList(CommonFunctions.GetAllDropdownList("ArchiveOptions"), "ID", "Name", patientModel.ArchiveOption);

            return View(patientModel);
        }

        [HttpPost]
        public ActionResult GetData(FormCollection formCollection)
        {
            PatientModel patientModel = new PatientModel();
            if (!string.IsNullOrEmpty(formCollection["OfficeName"]))
            {
                patientModel.OfficeName = formCollection["OfficeName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["PatientName"]))
            {
                patientModel.PatientName = formCollection["PatientName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["FormId"]))
            {
                patientModel.FormId = Convert.ToInt32(formCollection["FormId"]);
            }
            if (!string.IsNullOrEmpty(formCollection["SSN"]))
            {
                patientModel.SSN = formCollection["SSN"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["PolicyID"]))
            {
                patientModel.PolicyID = formCollection["PolicyID"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["SubFrom"]))
            {
                patientModel.SubFrom = formCollection["SubFrom"];
            }
            if (!string.IsNullOrEmpty(formCollection["SubTo"]))
            {
                patientModel.SubTo = formCollection["SubTo"];
            }
            if (!string.IsNullOrEmpty(formCollection["ArchiveOption"]))
            {
                patientModel.ArchiveOption = Convert.ToInt32(formCollection["ArchiveOption"]);
            }
            //patientModel.PatientList = GetPatientSearchForAdmin(patientModel);
            //
            var draw = 1;// Request.Form.GetValues("draw").FirstOrDefault();
            var start = 0;// Request.Form.GetValues("start").FirstOrDefault();
            var length = 10;// Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;
            patientModel.PatientList = GetPatientSearchForAdmin(patientModel);
            totalRecords = patientModel.PatientList.Count();
            var data = patientModel.PatientList.Skip(skip).Take(pageSize).Select(r => new
            {
                CreatedDate = r.CreatedDate.ToString("MM/dd/yyyy"),
                r.PatientID,
                r.SSN,
                r.PatientName,
                r.OfficeName,
                r.PolicyID,
                r.GroupNumber,
                r.InsuranceCompanyName,
                r.IsArchived
            }).ToList();
            //
            Session["SearchCriteria"] = "OfficeName=" + formCollection["OfficeName"] + ",PatientName=" + formCollection["PatientName"] + ",FormId=" + formCollection["FormId"] + ",SSN=" + formCollection["SSN"] + ",PolicyID=" + formCollection["PolicyID"] + ",SubFrom=" + formCollection["SubFrom"] + ",SubTo=" + formCollection["SubTo"] + ",ArchiveOption=" + formCollection["ArchiveOption"];
           
            return Json(new { draw = draw, recordsFilter = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
   
        public List<PatientMaster> GetPatientSearchForAdmin(PatientModel patientModel)
        {
            if (patientModel == null) patientModel = new PatientModel();

            patientModel.PatientList = (from p in mbprosEntities.PatientMasters
                                        where p.Active == true
                                        select p).OrderByDescending(o => o.PatientID).ToList();

            if (patientModel != null)
            {
                if (!string.IsNullOrEmpty(patientModel.OfficeName))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.OfficeName.ToLower().Contains(patientModel.OfficeName.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.PatientName))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.PatientName.ToLower().Contains(patientModel.PatientName.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(patientModel.FormId)))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.PatientID == Convert.ToInt32(patientModel.FormId)).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.SSN))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.SSN != null && x.SSN.Contains(patientModel.SSN)).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.PolicyID))
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.PolicyID.ToLower().Contains(patientModel.PolicyID.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.SubFrom))
                {
                    DateTime pDate = DateTime.ParseExact(patientModel.SubFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.CreatedDate >= pDate).ToList();
                }
                if (!string.IsNullOrEmpty(patientModel.SubTo))
                {
                    DateTime pDate = DateTime.ParseExact(patientModel.SubTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.CreatedDate <= pDate).ToList();
                }
                if (patientModel.ArchiveOption == 1)
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.IsArchived == true).ToList();
                }
                else if (patientModel.ArchiveOption == 2)
                {
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.IsArchived == false).ToList();
                }                
            }
            return patientModel.PatientList;
        }

        public ActionResult BillingSearchForAdmin(string editMessage, bool IsBack = false)
        {
            BillingModel billingModel = new BillingModel();
            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;

            if (IsBack)
            {
                //Get & Set search criteria model
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SearchCriteria"])))
                {
                    string[] SearchCriteria = Session["SearchCriteria"].ToString().Split(",".ToArray());
                    for (int i = 0; i < SearchCriteria.Count(); i++)
                    {
                        string[] keyValues = SearchCriteria[i].Split("=".ToArray());
                        switch (keyValues[0])
                        {
                            case "OfficeName":
                                billingModel.OfficeName = keyValues[1];
                                break;
                            case "PatientName":
                                billingModel.PatientName = keyValues[1];
                                break;
                            case "FormId":
                                if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                                { billingModel.FormId = Convert.ToInt32(keyValues[1]); }
                                break;
                            case "SubFrom":
                                billingModel.SubFrom = keyValues[1];
                                break;
                            case "SubTo":
                                billingModel.SubTo = keyValues[1];
                                break;
                            case "ArchiveOption":
                                billingModel.ArchiveOption = Convert.ToInt32(keyValues[1]);
                                break;
                        }
                    }
                }
                billingModel.BillingList = GetBillingSearchForAdmin(billingModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
                billingModel.BillingList = GetBillingSearchForAdmin(null);
            }
        
            billingModel.OptionList = new SelectList(CommonFunctions.GetAllDropdownList("ArchiveOptions"), "ID", "Name", billingModel.ArchiveOption);

            return View(billingModel);
        }

        public List<PatientBilling> GetBillingSearchForAdmin(BillingModel billingModel)
        {
            if (billingModel == null) billingModel = new BillingModel();

            billingModel.BillingList = (from p in mbprosEntities.PatientBillings
                                        where p.Active == true && p.SrNo == 1
                                        select p).OrderByDescending(o => o.PatientBillingID).ToList();

            if (billingModel != null)
            {
                if (!string.IsNullOrEmpty(billingModel.OfficeName))
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.OfficeName.ToLower().Contains(billingModel.OfficeName.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(billingModel.PatientName))
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.PatientName.ToLower().Contains(billingModel.PatientName.ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(billingModel.FormId)))
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.PatientBillingID == Convert.ToInt32(billingModel.FormId)).ToList();
                }           
                if (!string.IsNullOrEmpty(billingModel.SubFrom))
                {
                    DateTime pDate = DateTime.ParseExact(billingModel.SubFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.CreatedDate >= pDate).ToList();
                }
                if (!string.IsNullOrEmpty(billingModel.SubTo))
                {
                    DateTime pDate = DateTime.ParseExact(billingModel.SubTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.CreatedDate <= pDate).ToList();
                }
                if (billingModel.ArchiveOption == 1)
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.IsArchived == true).ToList();
                }
                else if (billingModel.ArchiveOption == 2)
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.IsArchived == false).ToList();
                }            
            }
            return billingModel.BillingList;
        }

        
        [HttpPost]
        public PartialViewResult _SearchPatientForAdmin(FormCollection formCollection)
        {
            PatientModel patientModel = new PatientModel();
            if (!string.IsNullOrEmpty(formCollection["OfficeName"]))
            {
                patientModel.OfficeName = formCollection["OfficeName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["PatientName"]))
            {
                patientModel.PatientName = formCollection["PatientName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["FormId"]))
            {
                patientModel.FormId = Convert.ToInt32(formCollection["FormId"]);
            }
            if (!string.IsNullOrEmpty(formCollection["SSN"]))
            {
                patientModel.SSN = formCollection["SSN"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["PolicyID"]))
            {
                patientModel.PolicyID = formCollection["PolicyID"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["SubFrom"]))
            {
                patientModel.SubFrom = formCollection["SubFrom"];
            }
            if (!string.IsNullOrEmpty(formCollection["SubTo"]))
            {
                patientModel.SubTo = formCollection["SubTo"];
            }
            if (!string.IsNullOrEmpty(formCollection["ArchiveOption"]))
            {
                patientModel.ArchiveOption =Convert.ToInt32(formCollection["ArchiveOption"]);
            }
            //patientModel.PatientList = GetPatientSearchForAdmin(patientModel);
            //
            var draw = 1;// Request.Form.GetValues("draw").FirstOrDefault();
            var start = 0;// Request.Form.GetValues("start").FirstOrDefault();
            var length = 10;// Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;
            patientModel.PatientList = GetPatientSearchForAdmin(patientModel);
            totalRecords = patientModel.PatientList.Count();
            var data = patientModel.PatientList.Skip(skip).Take(pageSize).Select(r => new
            {
                CreatedDate = r.CreatedDate.ToString("MM/dd/yyyy"),
                r.PatientID,
                r.SSN,
                r.PatientName,
                r.OfficeName,
                r.PolicyID,
                r.GroupNumber,
                r.InsuranceCompanyName,
                r.IsArchived
            }).ToList();
            //
            Session["SearchCriteria"] = "OfficeName=" + formCollection["OfficeName"] + ",PatientName=" + formCollection["PatientName"] + ",FormId=" + formCollection["FormId"] + ",SSN=" + formCollection["SSN"] + ",PolicyID=" + formCollection["PolicyID"] + ",SubFrom=" + formCollection["SubFrom"] + ",SubTo=" + formCollection["SubTo"] + ",ArchiveOption=" + formCollection["ArchiveOption"];
            return PartialView("_SearchPatientForAdmin", patientModel);
        }

        [HttpPost]
        public PartialViewResult _SearchBillingForAdmin(FormCollection formCollection)
        {
            BillingModel billingModel = new BillingModel();
            if (!string.IsNullOrEmpty(formCollection["OfficeName"]))
            {
                billingModel.OfficeName = formCollection["OfficeName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["PatientName"]))
            {
                billingModel.PatientName = formCollection["PatientName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["FormId"]))
            {
                billingModel.FormId = Convert.ToInt32(formCollection["FormId"]);
            }
            if (!string.IsNullOrEmpty(formCollection["SubFrom"]))
            {
                billingModel.SubFrom = formCollection["SubFrom"];
            }
            if (!string.IsNullOrEmpty(formCollection["SubTo"]))
            {
                billingModel.SubTo = formCollection["SubTo"];
            }
            if (!string.IsNullOrEmpty(formCollection["ArchiveOption"]))
            {
                billingModel.ArchiveOption = Convert.ToInt32(formCollection["ArchiveOption"]);
            }
            billingModel.BillingList = GetBillingSearchForAdmin(billingModel);
            Session["SearchCriteria"] = "OfficeName=" + formCollection["OfficeName"] + ",PatientName=" + formCollection["PatientName"] + ",FormId=" + formCollection["FormId"] + ",SubFrom=" + formCollection["SubFrom"] + ",SubTo=" + formCollection["SubTo"] + ",ArchiveOption=" + formCollection["ArchiveOption"];

            return PartialView("_SearchBillingForAdmin", billingModel);
        }

        [HttpPost]
        public PartialViewResult _SearchPatient(FormCollection formCollection)
        {
            PatientModel patientModel = new PatientModel();
            if (!string.IsNullOrEmpty(formCollection["PatientName"]))
            {
                patientModel.PatientName = formCollection["PatientName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["FormId"]))
            {
                patientModel.FormId = Convert.ToInt32(formCollection["FormId"]);
            }
            if (!string.IsNullOrEmpty(formCollection["SSN"]))
            {
                patientModel.SSN = formCollection["SSN"].ToString(); 
            }
            if (!string.IsNullOrEmpty(formCollection["PolicyID"]))
            {
                patientModel.PolicyID = formCollection["PolicyID"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["SubFrom"]))
            {
                patientModel.SubFrom = formCollection["SubFrom"];
            }
            if (!string.IsNullOrEmpty(formCollection["SubTo"]))
            {
                patientModel.SubTo = formCollection["SubTo"];
            }
            if (!string.IsNullOrEmpty(formCollection["DateofBirth"]))
            {
                patientModel.DateofBirth = formCollection["DateofBirth"];
            }
            patientModel.PatientList = GetPatientSearch(patientModel);
            Session["SearchCriteria"] = "PatientName=" + formCollection["PatientName"] + ",FormId=" + formCollection["FormId"] + ",SSN=" + formCollection["SSN"] + ",PolicyID=" + formCollection["PolicyID"] + ",SubFrom=" + formCollection["SubFrom"] + ",SubTo=" + formCollection["SubTo"] + ",DateofBirth=" + formCollection["DateofBirth"];
            return PartialView("_SearchPatient", patientModel);
        }

        [HttpPost]
        public PartialViewResult _SearchBilling(FormCollection formCollection)
        {
            BillingModel billingModel = new BillingModel();
            if (!string.IsNullOrEmpty(formCollection["PatientName"]))
            {
                billingModel.PatientName = formCollection["PatientName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["FormId"]))
            {
                billingModel.FormId = Convert.ToInt32(formCollection["FormId"]);
            }
            if (!string.IsNullOrEmpty(formCollection["ServiceDate"]))
            {
                billingModel.ServiceDate = formCollection["ServiceDate"];
            }
            if (!string.IsNullOrEmpty(formCollection["SubFrom"]))
            {
                billingModel.SubFrom = formCollection["SubFrom"];
            }
            if (!string.IsNullOrEmpty(formCollection["SubTo"]))
            {
                billingModel.SubTo = formCollection["SubTo"];
            }
            billingModel.BillingList = GetBillingSearch(billingModel);
            Session["SearchCriteria"] = "PatientName=" + formCollection["PatientName"] + ",FormId=" + formCollection["FormId"] + ",ServiceDate=" + formCollection["ServiceDate"] + ",SubFrom=" + formCollection["SubFrom"] + ",SubTo=" + formCollection["SubTo"];

            return PartialView("_SearchBilling", billingModel);
        }

        public JsonResult DeleteBillingRecord(int patientBillingID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var patientBillings = mbprosEntities.PatientBillings.Where(aa => aa.PatientBillingID == patientBillingID).ToList();
            foreach (var patBill in patientBillings)
            {
                mbprosEntities.PatientBillings.Remove(patBill);
                mbprosEntities.SaveChanges();
            }
            TempData["EditMessage"] = "Y";
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ArchiveBillingRecord(int patientBillingID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var patientBillings = mbprosEntities.PatientBillings.Where(aa => aa.PatientBillingID == patientBillingID).ToList();
            foreach (var patBill in patientBillings)
            {
                patBill.IsArchived = true;
                mbprosEntities.Entry(patBill).State = System.Data.Entity.EntityState.Modified;
                mbprosEntities.SaveChanges();
            }
            TempData["EditMessage"] = "Y";
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnarchiveBillingRecord(int patientBillingID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var patientBillings = mbprosEntities.PatientBillings.Where(aa => aa.PatientBillingID == patientBillingID).ToList();
            foreach (var patBill in patientBillings)
            {
                patBill.IsArchived = false;
                mbprosEntities.Entry(patBill).State = System.Data.Entity.EntityState.Modified;
                mbprosEntities.SaveChanges();
            }
            TempData["EditMessage"] = "Y";
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePatient(int patientID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var patients = mbprosEntities.PatientMasters.Where(aa => aa.PatientID == patientID).ToList();
            foreach (var pat in patients)
            {
                mbprosEntities.PatientMasters.Remove(pat);
                mbprosEntities.SaveChanges();
            }
            TempData["EditMessage"] = "Y";

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ArchivePatientRecord(int patientID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var patients = mbprosEntities.PatientMasters.Where(aa => aa.PatientID == patientID).ToList();
            foreach (var pat in patients)
            {
                pat.IsArchived = true;
                mbprosEntities.Entry(pat).State = System.Data.Entity.EntityState.Modified;
                mbprosEntities.SaveChanges();
            }
            TempData["EditMessage"] = "Y";
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnarchivePatientRecord(int patientID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var patients = mbprosEntities.PatientMasters.Where(aa => aa.PatientID == patientID).ToList();
            foreach (var pat in patients)
            {
                pat.IsArchived = false;
                mbprosEntities.Entry(pat).State = System.Data.Entity.EntityState.Modified;
                mbprosEntities.SaveChanges();
            }
            TempData["EditMessage"] = "Y";
            return Json("", JsonRequestBehavior.AllowGet);
        }


    }
}
