using Mbpros.Common;
using Mbpros.DAL;
using Mbpros.Models;
using System;
using System.Collections.Generic;
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
        //Patient search for admin dgfdgdfgdfg ggfhfghfgh
        public ActionResult PatientSearchForAdmin(string editMessage, bool IsBack = false)
        {
            PatientModel patientModel = new PatientModel();

            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;

            if (IsBack)
            {
                patientModel = GetModelIFBacktoPatSrch4Admin(patientModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
            }
            patientModel.OptionList = new SelectList(CommonFunctions.GetAllDropdownList("ArchiveOptions"), "ID", "Name", patientModel.ArchiveOption);

            return View(patientModel);
        }

        public JsonResult DynamicGridData1(string sidx, string sord, int page, int rows, FormCollection postData, bool firstLoading)
        {
            PatientModel patientModel = new PatientModel();
            if (postData["filters[IsBack]"] == null)
            {
                patientModel = GetModelIFBacktoPatSrch4Admin(patientModel);
            }
            else
            {
                if (string.IsNullOrEmpty(postData["filters[OfficeName]"])) postData["filters[OfficeName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[PatientName]"])) postData["filters[PatientName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[FormId]"])) postData["filters[FormId]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SSN]"])) postData["filters[SSN]"] = null;
                if (string.IsNullOrEmpty(postData["filters[PolicyID]"])) postData["filters[PolicyID]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubFrom]"])) postData["filters[SubFrom]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubTo]"])) postData["filters[SubTo]"] = null;
                if (string.IsNullOrEmpty(postData["filters[ArchiveOption]"])) postData["filters[ArchiveOption]"] = null;

                patientModel.OfficeName = postData["filters[OfficeName]"] == null ? null : Convert.ToString(postData["filters[OfficeName]"]);
                patientModel.PatientName = postData["filters[PatientName]"] == null ? null : Convert.ToString(postData["filters[PatientName]"]).Trim();
                if (postData["filters[FormId]"] != null)
                {
                    patientModel.FormId = Convert.ToInt32(postData["filters[FormId]"]);
                }
                patientModel.SSN = postData["filters[SSN]"] == null ? null : Convert.ToString(postData["filters[SSN]"]).Trim();
                patientModel.PolicyID = postData["filters[PolicyID]"] == null ? null : Convert.ToString(postData["filters[PolicyID]"]).Trim();
                patientModel.SubFrom = postData["filters[SubFrom]"] == null ? null : Convert.ToString(postData["filters[SubFrom]"]).Trim();
                patientModel.SubTo = postData["filters[SubTo]"] == null ? null : Convert.ToString(postData["filters[SubTo]"]).Trim();
                patientModel.ArchiveOption = Convert.ToInt32(postData["filters[ArchiveOption]"]);
            }
            patientModel.PatientList = GetPatientSearchForAdmin(patientModel);

            if (!string.IsNullOrEmpty(postData["filters[PageNumber]"])) page = Convert.ToInt32(postData["filters[PageNumber]"]);
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = patientModel.PatientList.ToList().Count;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            //.OrderBy(sidx + " " + sord)
            var PatientList = patientModel.PatientList.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from r in PatientList
                    select new
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
                    }).ToArray()
            };
            //Session["SearchCriteria"] = "OfficeName=" + postData["filters[OfficeName]"] + ",PatientName=" + postData["filters[PatientName]"] + ",FormId=" + postData["filters[FormId]"] + ",SSN=" + postData["filters[SSN]"] + ",PolicyID=" + postData["filters[PolicyID]"] + ",SubFrom=" + postData["filters[SubFrom]"] + ",SubTo=" + postData["filters[SubTo]"] + ",ArchiveOption=" + postData["filters[ArchiveOption]"];
            Session["SearchCriteria"] = "OfficeName=" + patientModel.OfficeName + ",PatientName=" + patientModel.PatientName + ",FormId=" + patientModel.FormId + ",SSN=" + patientModel.SSN + ",PolicyID=" + patientModel.PolicyID + ",SubFrom=" + patientModel.SubFrom + ",SubTo=" + patientModel.SubTo + ",ArchiveOption=" + patientModel.ArchiveOption;

            return Json(jsonData);
        }

        public PatientModel GetModelIFBacktoPatSrch4Admin(PatientModel patientModel)
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
                            if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                            { patientModel.ArchiveOption = Convert.ToInt32(keyValues[1]); }
                            break;
                    }
                }
            }
            return patientModel;
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
                    patientModel.SSN = patientModel.SSN.Replace("_", "").Replace("-", "");
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.SSN != null && x.SSN.Replace("_", "").Replace("-", "").Contains(patientModel.SSN)).ToList();
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

        //Billing search for admin
        public ActionResult BillingSearchForAdmin(string editMessage, bool IsBack = false)
        {
            BillingModel billingModel = new BillingModel();
            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;

            if (IsBack)
            {
                billingModel = GetModelIFBacktoBillingSrch4Admin(billingModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
            }

            billingModel.OptionList = new SelectList(CommonFunctions.GetAllDropdownList("ArchiveOptions"), "ID", "Name", billingModel.ArchiveOption);

            return View(billingModel);
        }

        public JsonResult DynamicGridData2(string sidx, string sord, int page, int rows, FormCollection postData, bool firstLoading)
        {
            BillingModel billingModel = new BillingModel();
            if (postData["filters[IsBack]"] == null)
            {
                billingModel = GetModelIFBacktoBillingSrch4Admin(billingModel);
            }
            else
            {
                if (string.IsNullOrEmpty(postData["filters[OfficeName]"])) postData["filters[OfficeName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[PatientName]"])) postData["filters[PatientName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[FormId]"])) postData["filters[FormId]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubFrom]"])) postData["filters[SubFrom]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubTo]"])) postData["filters[SubTo]"] = null;
                if (string.IsNullOrEmpty(postData["filters[ServiceDate]"])) postData["filters[ServiceDate]"] = null;
                if (string.IsNullOrEmpty(postData["filters[ArchiveOption]"])) postData["filters[ArchiveOption]"] = null;

                billingModel.OfficeName = postData["filters[OfficeName]"] == null ? null : Convert.ToString(postData["filters[OfficeName]"]);
                billingModel.PatientName = postData["filters[PatientName]"] == null ? null : Convert.ToString(postData["filters[PatientName]"]).Trim();
                if (postData["filters[FormId]"] != null)
                {
                    billingModel.FormId = Convert.ToInt32(postData["filters[FormId]"]);
                }
                billingModel.SubFrom = postData["filters[SubFrom]"] == null ? null : Convert.ToString(postData["filters[SubFrom]"]).Trim();
                billingModel.SubTo = postData["filters[SubTo]"] == null ? null : Convert.ToString(postData["filters[SubTo]"]).Trim();
                billingModel.ServiceDate = postData["filters[ServiceDate]"] == null ? null : Convert.ToString(postData["filters[ServiceDate]"]).Trim();
                billingModel.ArchiveOption = Convert.ToInt32(postData["filters[ArchiveOption]"]);
            }
            billingModel.BillingList = GetBillingSearchForAdmin(billingModel);

            if (!string.IsNullOrEmpty(postData["filters[PageNumber]"])) page = Convert.ToInt32(postData["filters[PageNumber]"]);
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = billingModel.BillingList.ToList().Count;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            //.OrderBy(sidx + " " + sord)
            var BillingList = billingModel.BillingList.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from r in BillingList
                    select new
                    {
                        r.PatientBillingID,
                        CreatedDate = r.CreatedDate.ToString("MM/dd/yyyy"),
                        r.PatientName,
                        r.OfficeName,
                        ServiceDate = r.ServiceDate.ToString("MM/dd/yyyy"),
                        r.ProcedureCodes,
                        r.DXCode,
                        r.Note,
                        r.IsArchived
                    }).ToArray()
            };
            //Session["SearchCriteria"] = "OfficeName=" + postData["filters[OfficeName]"] + ",PatientName=" + postData["filters[PatientName]"] + ",FormId=" + postData["filters[FormId]"] + ",SubFrom=" + postData["filters[SubFrom]"] + ",SubTo=" + postData["filters[SubTo]"] + ",ServiceDate=" + postData["filters[ServiceDate]"] + ",ArchiveOption=" + postData["filters[ArchiveOption]"];
            Session["SearchCriteria"] = "OfficeName=" + billingModel.OfficeName + ",PatientName=" + billingModel.PatientName + ",FormId=" + billingModel.FormId + ",SubFrom=" + billingModel.SubFrom + ",SubTo=" + billingModel.SubTo + ",ServiceDate=" + billingModel.ServiceDate + ",ArchiveOption=" + billingModel.ArchiveOption;
            return Json(jsonData);
        }

        public BillingModel GetModelIFBacktoBillingSrch4Admin(BillingModel billingModel)
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
                        case "ServiceDate":
                            billingModel.ServiceDate = keyValues[1];
                            break;
                        case "ArchiveOption":
                            if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                            { billingModel.ArchiveOption = Convert.ToInt32(keyValues[1]); }
                            break;
                    }
                }
            }
            return billingModel;
        }

        public List<PatientBilling> GetBillingSearchForAdmin(BillingModel billingModel)
        {
            if (billingModel == null) billingModel = new BillingModel();

            billingModel.BillingList = (from p in mbprosEntities.PatientBillings
                                        where p.Active == true //&& p.SrNo == 1
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
                if (!string.IsNullOrEmpty(billingModel.ServiceDate))
                {
                    DateTime pDate = DateTime.ParseExact(billingModel.ServiceDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.ServiceDate == pDate).ToList();
                }
                if (billingModel.ArchiveOption == 1)
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.IsArchived == true).ToList();
                }
                else if (billingModel.ArchiveOption == 2)
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.IsArchived == false).ToList();
                }

                var patientBillingIds = billingModel.BillingList.Select(p => p.PatientBillingID).Distinct().ToArray();
                billingModel.BillingList = (from p in mbprosEntities.PatientBillings.Where(p => patientBillingIds.Contains(p.PatientBillingID) && p.SrNo == 1) select p).OrderByDescending(o => o.PatientBillingID).ToList();

            }
            return billingModel.BillingList;
        }

        //Patient search
        public ActionResult PatientSearch(string editMessage, bool IsBack = false)
        {
            PatientModel patientModel = new PatientModel();

            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;

            if (IsBack)
            {
                patientModel = GetModelIFBacktoPatSrch(patientModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
            }
            patientModel.OptionList = new SelectList(CommonFunctions.GetAllDropdownList("ArchiveOptions"), "ID", "Name", patientModel.ArchiveOption);

            return View(patientModel);
        }

        public JsonResult DynamicGridData3(string sidx, string sord, int page, int rows, FormCollection postData, bool firstLoading)
        {
            PatientModel patientModel = new PatientModel();
            if (postData["filters[IsBack]"] == null)
            {
                patientModel = GetModelIFBacktoPatSrch(patientModel);
            }
            else
            {
                if (string.IsNullOrEmpty(postData["filters[PatientName]"])) postData["filters[PatientName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[FormId]"])) postData["filters[FormId]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SSN]"])) postData["filters[SSN]"] = null;
                if (string.IsNullOrEmpty(postData["filters[PolicyID]"])) postData["filters[PolicyID]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubFrom]"])) postData["filters[SubFrom]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubTo]"])) postData["filters[SubTo]"] = null;
                if (string.IsNullOrEmpty(postData["filters[DateofBirth]"])) postData["filters[DateofBirth]"] = null;
                if (string.IsNullOrEmpty(postData["filters[ArchiveOption]"])) postData["filters[ArchiveOption]"] = null;

                patientModel.PatientName = postData["filters[PatientName]"] == null ? null : Convert.ToString(postData["filters[PatientName]"]).Trim();
                if (postData["filters[FormId]"] != null)
                {
                    patientModel.FormId = Convert.ToInt32(postData["filters[FormId]"]);
                }
                patientModel.SSN = postData["filters[SSN]"] == null ? null : Convert.ToString(postData["filters[SSN]"]).Trim();
                patientModel.PolicyID = postData["filters[PolicyID]"] == null ? null : Convert.ToString(postData["filters[PolicyID]"]).Trim();
                patientModel.SubFrom = postData["filters[SubFrom]"] == null ? null : Convert.ToString(postData["filters[SubFrom]"]).Trim();
                patientModel.SubTo = postData["filters[SubTo]"] == null ? null : Convert.ToString(postData["filters[SubTo]"]).Trim();
                patientModel.DateofBirth = postData["filters[DateofBirth]"] == null ? null : Convert.ToString(postData["filters[DateofBirth]"]).Trim();
                patientModel.ArchiveOption = Convert.ToInt32(postData["filters[ArchiveOption]"]);
            }
            patientModel.PatientList = GetPatientSearch(patientModel);

            if (!string.IsNullOrEmpty(postData["filters[PageNumber]"])) page = Convert.ToInt32(postData["filters[PageNumber]"]);
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = patientModel.PatientList.ToList().Count;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            //.OrderBy(sidx + " " + sord)
            var PatientList = patientModel.PatientList.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from r in PatientList
                    select new
                    {
                        CreatedDate = r.CreatedDate.ToString("MM/dd/yyyy"),
                        r.PatientID,
                        r.SSN,
                        r.PatientName,
                        r.OfficeName,
                        r.PolicyID,
                        r.GroupNumber,
                        r.InsuranceCompanyName,
                        r.IsArchived//,
                        //DateofBirth = r.DateofBirth.ToString("MM/dd/yyyy")
                    }).ToArray()
            };
            //Session["SearchCriteria"] = "PatientName=" + postData["filters[PatientName]"] + ",FormId=" + postData["filters[FormId]"] + ",SSN=" + postData["filters[SSN]"] + ",PolicyID=" + postData["filters[PolicyID]"] + ",SubFrom=" + postData["filters[SubFrom]"] + ",SubTo=" + postData["filters[SubTo]"] + ",DateofBirth=" + postData["filters[DateofBirth]"];
            Session["SearchCriteria"] = "PatientName=" + patientModel.PatientName + ",FormId=" + patientModel.FormId + ",SSN=" + patientModel.SSN + ",PolicyID=" + patientModel.PolicyID + ",SubFrom=" + patientModel.SubFrom + ",SubTo=" + patientModel.SubTo + ",DateofBirth=" + patientModel.DateofBirth + ",ArchiveOption=" + patientModel.ArchiveOption;

            return Json(jsonData);
        }

        public PatientModel GetModelIFBacktoPatSrch(PatientModel patientModel)
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
                        case "ArchiveOption":
                            if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                            { patientModel.ArchiveOption = Convert.ToInt32(keyValues[1]); }
                            break;
                    }
                }
            }
            return patientModel;
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
                    patientModel.SSN = patientModel.SSN.Replace("_", "").Replace("-", "");
                    patientModel.PatientList = patientModel.PatientList.Where(x => x.SSN != null && x.SSN.Replace("_", "").Replace("-", "").Contains(patientModel.SSN)).ToList();
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

        //Billing search
        public ActionResult BillingSearch(string editMessage, bool IsBack = false)
        {
            BillingModel billingModel = new BillingModel();

            if (TempData["EditMessage"] != null)
                ViewBag.EditMessage = editMessage;
            if (IsBack)
            {
                billingModel = GetModelIFBacktoBillingSrch(billingModel);
            }
            else
            {
                Session["SearchCriteria"] = "";
            }
            billingModel.OptionList = new SelectList(CommonFunctions.GetAllDropdownList("ArchiveOptions"), "ID", "Name", billingModel.ArchiveOption);

            return View(billingModel);
        }

        public JsonResult DynamicGridData4(string sidx, string sord, int page, int rows, FormCollection postData, bool firstLoading)
        {
            BillingModel billingModel = new BillingModel();
            if (postData["filters[IsBack]"] == null)
            {
                billingModel = GetModelIFBacktoBillingSrch(billingModel);
            }
            else
            {
                if (string.IsNullOrEmpty(postData["filters[PatientName]"])) postData["filters[PatientName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[FormId]"])) postData["filters[FormId]"] = null;
                if (string.IsNullOrEmpty(postData["filters[ServiceDate]"])) postData["filters[ServiceDate]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubFrom]"])) postData["filters[SubFrom]"] = null;
                if (string.IsNullOrEmpty(postData["filters[SubTo]"])) postData["filters[SubTo]"] = null;
                if (string.IsNullOrEmpty(postData["filters[ArchiveOption]"])) postData["filters[ArchiveOption]"] = null;

                billingModel.PatientName = postData["filters[PatientName]"] == null ? null : Convert.ToString(postData["filters[PatientName]"]).Trim();
                if (postData["filters[FormId]"] != null)
                {
                    billingModel.FormId = Convert.ToInt32(postData["filters[FormId]"]);
                }
                billingModel.ServiceDate = postData["filters[ServiceDate]"] == null ? null : Convert.ToString(postData["filters[ServiceDate]"]).Trim();
                billingModel.SubFrom = postData["filters[SubFrom]"] == null ? null : Convert.ToString(postData["filters[SubFrom]"]).Trim();
                billingModel.SubTo = postData["filters[SubTo]"] == null ? null : Convert.ToString(postData["filters[SubTo]"]).Trim();
                billingModel.ArchiveOption = Convert.ToInt32(postData["filters[ArchiveOption]"]);
            }
            billingModel.BillingList = GetBillingSearch(billingModel);

            if (!string.IsNullOrEmpty(postData["filters[PageNumber]"])) page = Convert.ToInt32(postData["filters[PageNumber]"]);
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = billingModel.BillingList.ToList().Count;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            //.OrderBy(sidx + " " + sord)
            var BillingList = billingModel.BillingList.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from r in BillingList
                    select new
                    {
                        r.PatientBillingID,
                        CreatedDate = r.CreatedDate.ToString("MM/dd/yyyy"),
                        r.PatientName,
                        r.OfficeName,
                        ServiceDate = r.ServiceDate.ToString("MM/dd/yyyy"),
                        r.ProcedureCodes,
                        r.DXCode,
                        r.Note,
                        r.IsArchived
                    }).ToArray()
            };
            //Session["SearchCriteria"] = "PatientName=" + postData["filters[PatientName]"] + ",FormId=" + postData["filters[FormId]"] + ",ServiceDate=" + postData["filters[ServiceDate]"] + ",SubFrom=" + postData["filters[SubFrom]"] + ",SubTo=" + postData["filters[SubTo]"];
            Session["SearchCriteria"] = "PatientName=" + billingModel.PatientName + ",FormId=" + billingModel.FormId + ",ServiceDate=" + billingModel.ServiceDate + ",SubFrom=" + billingModel.SubFrom + ",SubTo=" + billingModel.SubTo + ",ArchiveOption=" + billingModel.ArchiveOption;
            return Json(jsonData);
        }

        private BillingModel GetModelIFBacktoBillingSrch(BillingModel billingModel)
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
                        case "ArchiveOption":
                            if (!string.IsNullOrEmpty(Convert.ToString(keyValues[1])))
                            { billingModel.ArchiveOption = Convert.ToInt32(keyValues[1]); }
                            break;
                    }
                }
            }
            return billingModel;
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
                                        where p.OfficeName == officename && p.Active == true //&& p.SrNo == 1
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
                if (billingModel.ArchiveOption == 1)
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.IsArchived == true).ToList();
                }
                else if (billingModel.ArchiveOption == 2)
                {
                    billingModel.BillingList = billingModel.BillingList.Where(x => x.IsArchived == false).ToList();
                }

                var patientBillingIds = billingModel.BillingList.Select(p => p.PatientBillingID).Distinct().ToArray();
                billingModel.BillingList = (from p in mbprosEntities.PatientBillings.Where(p => patientBillingIds.Contains(p.PatientBillingID) && p.SrNo == 1) select p).OrderByDescending(o => o.PatientBillingID).ToList();
            }
            return billingModel.BillingList;
        }

        //Other Functions
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
