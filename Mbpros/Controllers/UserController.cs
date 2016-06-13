using Mbpros.Common;
using Mbpros.DAL;
using Mbpros.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;


namespace Mbpros.Controllers
{
    [SessionExpire]
    [AccessDeniedAuthorize(Roles = "ADMIN")]
    public class UserController : Controller
    {
        public ActionResult AddUser(int userID = 0, string message = "")
        {
            UserModels userModel = new UserModels();
            if (userID > 0)
            {
                MbprosEntities mbprosEntities = new MbprosEntities();
                var userDetails = mbprosEntities.Users.Find(userID);
                if (userDetails != null)
                {
                    userModel.UserId = userID;
                    userModel.UserName = userDetails.UserName;
                    userModel.Password = userDetails.Password;
                    userModel.OfficeName = userDetails.OfficeName;
                    userModel.EmailID = userDetails.EmailID;
                    userModel.IsAdmin = userDetails.IsAdmin;

                }
            }
            ViewBag.UserMessage = message;
            return View(userModel);
        }

        [HttpPost]
        public ActionResult AddUser(UserModels userModel)
        {

            if (ModelState.IsValid)
            {
                var UserId = Convert.ToString(Session["USERID"]) == "" ? 0 : Convert.ToInt32(Session["USERID"]);
                if (userModel.UserId == 0)//ADD mode
                {
                    if (!IsUserExist(userModel.UserName))
                    {
                        using (MbprosEntities mbprosEntities = new MbprosEntities())
                        {
                            User user = new User();
                            user.UserName = userModel.UserName;
                            user.OfficeName = userModel.OfficeName;
                            user.Password = userModel.Password;
                            user.Status = true;
                            user.IsAdmin = true;
                            user.EmailID = userModel.EmailID;
                            user.IsAdmin = false;
                            //if (userModel.IsAdmin == null)
                            //    user.IsAdmin = false;
                            //else
                            //    user.IsAdmin = userModel.IsAdmin == true ? true : false;
                            user.CreatedDate = DateTime.Now.Date;
                            user.CreatedBy = UserId;

                            mbprosEntities.Users.Add(user);

                            //Save user in membership
                            WebSecurity.CreateUserAndAccount(userModel.UserName, userModel.Password);
                            if (!Roles.RoleExists("USER"))
                            {
                                Roles.CreateRole("USER");
                            }
                            Roles.AddUserToRole(userModel.UserName, "USER");

                            mbprosEntities.SaveChanges();

                            //Add entries in FeatuereUser table for Normal user
                            var idParam = new SqlParameter
                            {
                                ParameterName = "UserID",
                                Value = user.UserId
                            };
                            var idParamB = new SqlParameter
                            {
                                ParameterName = "IsAdminUser",
                                Value = 0
                            };
                            mbprosEntities.Database.ExecuteSqlCommand("usp_AddFeatureUser @UserID, @IsAdminUser", idParam, idParamB);
                            //

                        }
                        ViewBag.IsUserExist = "";
                        TempData["UserMessage"] = "Y";
                        //return RedirectToAction("AddUser", new { userID = 0, message = "User saved successfully." });
                        return RedirectToAction("SearchUser", new { message = "User added successfully.", IsBack = true });
                    }
                    else
                    {
                        ViewBag.IsUserExist = "YES";
                    }
                }
                else//EDIT mode
                {
                    using (MbprosEntities mbprosEntities = new MbprosEntities())
                    {
                        User user = mbprosEntities.Users.Find(userModel.UserId);
                        if (user != null)
                        {
                            WebSecurity.ChangePassword(userModel.UserName, user.Password, userModel.Password);

                            //user.UserName = userModel.UserName; //NEVER
                            user.OfficeName = userModel.OfficeName;
                            user.Password = userModel.Password;
                            user.EmailID = userModel.EmailID;
                            user.UpdatedDate = DateTime.Now;
                            user.UpdatedBy = UserId;
                            user.IsAdmin = userModel.IsAdmin;

                            mbprosEntities.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            mbprosEntities.SaveChanges();

                        }
                    }
                    TempData["UserMessage"] = "Y";
                    return RedirectToAction("SearchUser", new { message = "User updated successfully.", IsBack = true });
                }

            }
            return View(userModel);
        }

        private bool IsUserExist(string userName)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var user = mbprosEntities.Users.Where(model => model.UserName == userName).ToList();
            if (user.Count > 0)
            {
                return true;
            }
            return false;
        }

        public ActionResult SearchUser(string message, bool IsBack = false)
        {
            UserModels userModel = new UserModels();
            if (TempData["UserMessage"] != null)
                ViewBag.UserMessage = message;

            if (IsBack)
            {
                userModel = GetModelIFBacktoUserSrch(userModel);                
            }
            else
            {
                Session["SearchCriteria"] = "";
            }
            return View(userModel);
        }

        public JsonResult DeleteUser(int userID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var user = mbprosEntities.Users.Find(userID);
            if (user != null)
            {
                mbprosEntities.Users.Attach(user);
                mbprosEntities.Users.Remove(user);
                mbprosEntities.SaveChanges();

                //Delete from membership
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.UserName);
                string[] allRoles = { "USER" };
                Roles.RemoveUserFromRoles(user.UserName, allRoles);
                Membership.Provider.DeleteUser(user.UserName, true);
                Membership.DeleteUser(user.UserName, true);
            }
            TempData["UserMessage"] = "Y";
            return Json("", JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public PartialViewResult _SearchUser(FormCollection formCollection)
        {
            UserModels userModel = new UserModels();
            if (!string.IsNullOrEmpty(formCollection["UserName"]))
            {
                userModel.UserName = formCollection["UserName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["OfficeName"]))
            {
                userModel.OfficeName = formCollection["OfficeName"].ToString();
            }
            if (!string.IsNullOrEmpty(formCollection["EmailID"]))
            {
                userModel.EmailID = formCollection["EmailID"];
            }
            userModel.UserList = GetUserSearch(userModel);
            Session["SearchCriteria"] = "UserName=" + formCollection["UserName"] + ",OfficeName=" + formCollection["OfficeName"] + ",EmailID=" + formCollection["EmailID"];

            return PartialView("_SearchUser", userModel);
        }

        public List<User> GetUserSearch(UserModels userModel)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            if (userModel == null) userModel = new UserModels();
            userModel.UserList = mbprosEntities.Users.Where(o => o.Status == true).OrderByDescending(s => s.UserId).ToList();

            if (!string.IsNullOrEmpty(userModel.UserName))
            {
                userModel.UserList = userModel.UserList.Where(x => x.UserName.ToLower().Contains(userModel.UserName.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(userModel.OfficeName))
            {
                userModel.UserList = userModel.UserList.Where(x => x.OfficeName.ToLower().Contains(userModel.OfficeName.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(userModel.EmailID))
            {
                userModel.UserList = userModel.UserList.Where(x => x.EmailID.ToLower().Contains(userModel.EmailID.Trim().ToLower())).ToList();
            }

            return userModel.UserList;
        }

        public JsonResult DynamicGridData5(string sidx, string sord, int page, int rows, FormCollection postData, bool firstLoading)
        {
            UserModels userModel = new UserModels();
            if (postData["filters[IsBack]"] == null)
            {
                userModel = GetModelIFBacktoUserSrch(userModel);
            }
            else
            {
                if (string.IsNullOrEmpty(postData["filters[UserName]"])) postData["filters[UserName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[OfficeName]"])) postData["filters[OfficeName]"] = null;
                if (string.IsNullOrEmpty(postData["filters[EmailID]"])) postData["filters[EmailID]"] = null;

                userModel.UserName = postData["filters[UserName]"] == null ? null : Convert.ToString(postData["filters[UserName]"]).Trim();
                userModel.OfficeName = postData["filters[OfficeName]"] == null ? null : Convert.ToString(postData["filters[OfficeName]"]).Trim();
                userModel.EmailID = postData["filters[EmailID]"] == null ? null : Convert.ToString(postData["filters[EmailID]"]).Trim();
            }
            userModel.UserList = GetUserSearch(userModel);

            if (!string.IsNullOrEmpty(postData["filters[PageNumber]"])) page = Convert.ToInt32(postData["filters[PageNumber]"]);
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = userModel.UserList.ToList().Count;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            //.OrderBy(sidx + " " + sord)
            var userList = userModel.UserList.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                    from r in userList
                    select new
                    {
                        r.UserId,
                        r.UserName,
                        r.Password,
                        r.OfficeName,
                        r.EmailID
                    }
                    ).ToArray()
            };
            //Session["SearchCriteria"] = "UserName=" + postData["filters[UserName]"] + ",OfficeName=" + postData["filters[OfficeName]"] + ",EmailID=" + postData["filters[EmailID]"];
            Session["SearchCriteria"] = "UserName=" + userModel.UserName  + ",OfficeName=" + userModel.OfficeName  + ",EmailID=" + userModel.EmailID;

            return Json(jsonData);
        }

        private UserModels GetModelIFBacktoUserSrch(UserModels userModel)
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
                        case "UserName":
                            userModel.UserName = keyValues[1];
                            break;
                        case "OfficeName":
                            userModel.OfficeName = keyValues[1];
                            break;
                        case "EmailID":
                            userModel.EmailID = keyValues[1];
                            break;
                    }
                }
            }
            return userModel;
        }

    }
}
