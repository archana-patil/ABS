using Mbpros.Common;
using Mbpros.DAL;
using Mbpros.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Mbpros.Controllers
{
    public class AccountController : Controller
    {
        //comment hete
        MbprosEntities mbprosEntities = new MbprosEntities();
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AccountController));  //Declaring Log4Net
        public ActionResult Login()
        {
            Session["NONLOGIN"] = "YES";
            ////
            try
            {
                var lstUsers = mbprosEntities.Users.ToList();
                if (lstUsers.Count() == 0)
                {
                    MbprosEntities en = new MbprosEntities();
                    var lstconfigkeys = en.Configs.ToList();

                    string FirstAdminUserName = lstconfigkeys.Where(x => x.key == "FirstAdminUserName").FirstOrDefault().value;//ConfigurationManager.AppSettings["FirstAdminUserName"].ToString().Trim();
                    string FirstAdminUserPswd = lstconfigkeys.Where(x => x.key == "FirstAdminUserPswd").FirstOrDefault().value; //ConfigurationManager.AppSettings["FirstAdminUserPswd"].ToString().Trim();
                    string FirstAdminUserOffice = lstconfigkeys.Where(x => x.key == "FirstAdminUserOffice").FirstOrDefault().value; // ConfigurationManager.AppSettings["FirstAdminUserOffice"].ToString().Trim();
                    string FirstAdminUserEmailID = lstconfigkeys.Where(x => x.key == "FirstAdminUserEmailID").FirstOrDefault().value; //ConfigurationManager.AppSettings["FirstAdminUserEmailID"].ToString().Trim();

                    //create a admin user with websecurity
                    WebSecurity.CreateUserAndAccount(FirstAdminUserName, FirstAdminUserPswd);

                    //Create ADMIN role if not exist in the system.
                    if (!Roles.RoleExists("ADMIN"))
                    {
                        Roles.CreateRole("ADMIN");
                    }
                    Roles.AddUserToRole(FirstAdminUserName, "ADMIN");

                    //Add entries in Users table
                    User user = new User();
                    user.UserName = FirstAdminUserName;
                    user.OfficeName = FirstAdminUserOffice;
                    user.Password = FirstAdminUserPswd;
                    user.Status = true;
                    user.IsAdmin = true;
                    user.EmailID = FirstAdminUserEmailID;
                    user.IsAdmin = true;
                    user.CreatedDate = DateTime.Now.Date;
                    user.CreatedBy = 1;

                    mbprosEntities.Users.Add(user);
                    mbprosEntities.SaveChanges();

                    //Add entries in FeatuereUser table for Admin user
                    var idParam = new SqlParameter
                    {
                        ParameterName = "UserID",
                        Value = user.UserId
                    };
                    var idParamB = new SqlParameter
                    {
                        ParameterName = "IsAdminUser",
                        Value = 1
                    };
                    mbprosEntities.Database.ExecuteSqlCommand("usp_AddFeatureUser @UserID, @IsAdminUser", idParam, idParamB);
                }
            }
            catch (Exception ex)
            {
                // log.WriteLog("Page= addeditevent.aspx" + DateTime.Now + "\n" + ex.Message + "\n" + ex.StackTrace);
                logger.Error(ex.ToString());
                logger.Info("Action Index has been fired.");
                return RedirectToAction("Login", "Account");
            }
            ////
            if (Session["USERNAME"] != null)
            {
                return RedirectToAction("AddPatient", "Patient");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(model.LoginUserName, model.LoginUserPassword))
                {
                    Session["USERROLE"] = IsAdminUser(model.LoginUserName) ? "ADMIN" : "USER";
                    Session["USERNAME"] = model.LoginUserName;
                    Session["USERID"] = GetUserID(model.LoginUserName);
                    Session["UserMenuAccess"] = CommonFunctions.UserMenuAccess(Convert.ToInt32(Session["USERID"]));
                    Session["NONLOGIN"] = null;

                    //Session["ISLOGOUT"] = null;
                    //return RedirectToAction("Home", "Account");
                    return RedirectToAction("AddPatient", "Patient");
                }
                else
                {
                    //ViewBag.Message = "Please enter valid username & password.";
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }

        public ActionResult Home()
        {
            try
            {
                var lstUsers = mbprosEntities.Users.ToList();
                if (lstUsers.Count() == 0)
                {
                    MbprosEntities en = new MbprosEntities();
                    var lstconfigkeys = en.Configs.ToList();

                    string FirstAdminUserName = lstconfigkeys.Where(x => x.key == "FirstAdminUserName").FirstOrDefault().value;//ConfigurationManager.AppSettings["FirstAdminUserName"].ToString().Trim();
                    string FirstAdminUserPswd = lstconfigkeys.Where(x => x.key == "FirstAdminUserPswd").FirstOrDefault().value; //ConfigurationManager.AppSettings["FirstAdminUserPswd"].ToString().Trim();
                    string FirstAdminUserOffice = lstconfigkeys.Where(x => x.key == "FirstAdminUserOffice").FirstOrDefault().value; // ConfigurationManager.AppSettings["FirstAdminUserOffice"].ToString().Trim();
                    string FirstAdminUserEmailID = lstconfigkeys.Where(x => x.key == "FirstAdminUserEmailID").FirstOrDefault().value; //ConfigurationManager.AppSettings["FirstAdminUserEmailID"].ToString().Trim();

                    //create a admin user with websecurity
                    WebSecurity.CreateUserAndAccount(FirstAdminUserName, FirstAdminUserPswd);

                    //Create ADMIN role if not exist in the system.
                    if (!Roles.RoleExists("ADMIN"))
                    {
                        Roles.CreateRole("ADMIN");
                    }
                    Roles.AddUserToRole(FirstAdminUserName, "ADMIN");

                    //Add entries in Users table
                    User user = new User();
                    user.UserName = FirstAdminUserName;
                    user.OfficeName = FirstAdminUserOffice;
                    user.Password = FirstAdminUserPswd;
                    user.Status = true;
                    user.IsAdmin = true;
                    user.EmailID = FirstAdminUserEmailID;
                    user.IsAdmin = true;
                    user.CreatedDate = DateTime.Now.Date;
                    user.CreatedBy = 1;

                    mbprosEntities.Users.Add(user);
                    mbprosEntities.SaveChanges();

                    //Add entries in FeatuereUser table for Admin user
                    var idParam = new SqlParameter
                    {
                        ParameterName = "UserID",
                        Value = user.UserId
                    };
                    var idParamB = new SqlParameter
                    {
                        ParameterName = "IsAdminUser",
                        Value = 1
                    };
                    mbprosEntities.Database.ExecuteSqlCommand("usp_AddFeatureUser @UserID, @IsAdminUser", idParam, idParamB);

                }
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERNAME"])))
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return RedirectToAction("AddPatient", "Patient");
                }
            }
            catch (Exception ex)
            {
                // log.WriteLog("Page= addeditevent.aspx" + DateTime.Now + "\n" + ex.Message + "\n" + ex.StackTrace);
                logger.Error(ex.ToString());
                logger.Info("Action Index has been fired.");
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public JsonResult IsLoggedIn()
        {
            var isLogged = Session["USERROLE"] == null ? false : true;
            return Json(new { isLogged = isLogged, error = "", rededirection_url = "/Account/Login" }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult IsNonLoggedIn()
        //{
        //    var isLogged = Session["NONLOGIN"] == "YES" ? true : false;
        //    return Json(new { isLogged = isLogged, error = "", rededirection_url = "/Account/Login" }, JsonRequestBehavior.AllowGet);
        //}
        
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        private bool IsAdminUser(string userName)
        {
            var user = mbprosEntities.Users.Where(u => u.IsAdmin == true && u.UserName == userName);

            if (user.Count() > 0) return true;
            else return false;
        }

        private bool ValidateUser(string userName, string password)
        {
            var user = mbprosEntities.Users.Where(s => s.UserName == userName && s.Status == true && s.Password == password);
            if (user.Count() > 0) return true;
            else return false;
        }

        private int GetUserID(string UserName)
        {
            var user = mbprosEntities.Users.SingleOrDefault(u => u.UserName == UserName).UserId;
            return user;
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            Session["USERROLE"] = null;
            Session["USERNAME"] = null;
            Session["USERID"] = null;
            Session["NONLOGIN"] = null;
            Session["ISLOGOUT"] = "YES";
            return RedirectToAction("Login", "Account");
        }

    }
}
