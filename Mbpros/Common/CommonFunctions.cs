using Mbpros.DAL;
using Mbpros.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Mbpros.Common
{
    public static class CommonFunctions
    {
        public static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CommonFunctions));  //Declaring Log4Net
        public static IEnumerable GetAllDropdownList(string tableName)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            switch (tableName)
            {
                case "STATES":
                    var states = (from d in mbprosEntities.States
                                  select new { ID = d.StateCode, NAME = d.StateName }).OrderBy(d => d.NAME).ToList();
                    //if (states.Count > 0)
                    //{
                    //    states.Insert(0, new { ID = "", NAME = "" });
                    //}
                    return (IEnumerable)states;
                case "ArchiveOptions":
                    List<Options> options = new List<Options>();
                    options.Add(new Options() { ID = 0, Name = "All" });
                    options.Add(new Options() { ID = 1, Name = "Archive" });
                    options.Add(new Options() { ID = 2, Name = "Un-Archive" });
                    return options;
            }

            return Enumerable.Empty<string>();
        }

        public static string GetUserOffice(int UserID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var office = mbprosEntities.Users.SingleOrDefault(u => u.UserId == UserID);
            return office == null ? "" : office.OfficeName;
        }

        public static string GetUserEmailID(int UserID)
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var userEmail = mbprosEntities.Users.SingleOrDefault(u => u.UserId == UserID);
            return userEmail == null ? "" : userEmail.EmailID;
        }

        public static User GetAdminUserDetails()
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            var adminUsers = mbprosEntities.Users.Where(u => u.IsAdmin==true).OrderBy(u=>u.UserId).ToList();
            if (adminUsers.Count > 0)
                return adminUsers[0];
            return new User();
        }

        public static bool GetCaptchaResponse(string recaptcha_challenge_field, string recaptcha_response_field,string captchaSecretkey, out string message)
        {
            bool flag = false;
            message = "";

            string[] result;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/verify");
            request.ProtocolVersion = HttpVersion.Version10;
            request.Timeout = 0x7530;
            request.Method = "POST";
            request.UserAgent = "reCAPTCHA/ASP.NET";
            request.ContentType = "application/x-www-form-urlencoded";
            string formData = string.Format(
                "privatekey={0}&remoteip={1}&challenge={2}&response={3}",
                new object[]{
                        HttpUtility.UrlEncode(captchaSecretkey),
                        HttpUtility.UrlEncode(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString()),
                        HttpUtility.UrlEncode(recaptcha_challenge_field),
                        HttpUtility.UrlEncode(recaptcha_response_field)
                    });
            byte[] formbytes = Encoding.ASCII.GetBytes(formData);

            using (System.IO.Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formbytes, 0, formbytes.Length);
            }

            try
            {
                using (WebResponse httpResponse = request.GetResponse())
                {
                    using (System.IO.TextReader readStream = new System.IO.StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd().Split(new string[] { "\n", @"\n" }, StringSplitOptions.RemoveEmptyEntries);
                        message = result[1];//incorrect-captcha-sol
                        if (result[1] == "incorrect-captcha-sol")
                        {
                            //message = "You must enter the characters exactly as it is shown below";
                            message = "You must enter the characters exactly as it is shown in captcha";
                        }
                        flag = Convert.ToBoolean(result[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return flag;
        }

        public static int GetNextPatientBillingID()
        {
            MbprosEntities mbprosEntities = new MbprosEntities();
            int nextPatientID = 1;
            var patientBilling = mbprosEntities.PatientBillings.OrderByDescending(u => u.PatientBillingID).FirstOrDefault();
            if (patientBilling != null) nextPatientID = patientBilling.PatientBillingID + 1;
            return nextPatientID;
        }

        public static void SendEmail(string officeName, string callFrom)
        {
            MbprosEntities en = new MbprosEntities();
            var lstconfigkeys = en.Configs.ToList();
            string emailFrom = "", emailFromName = "", subject = "", password = "";
            string smtpAddress = lstconfigkeys.Where(x => x.key == "SmtpAddress").FirstOrDefault().value;//ConfigurationManager.AppSettings["SmtpAddress"].ToString().Trim();
            int portNumber = Convert.ToInt32(lstconfigkeys.Where(x => x.key == "PortNumber").FirstOrDefault().value);//ConfigurationManager.AppSettings["PortNumber"].ToString().Trim() == "" ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"].ToString().Trim());
            if (callFrom == "PATIENT_FORM")
            {
                emailFrom = lstconfigkeys.Where(x => x.key == "EmailSenderForPatientForm").FirstOrDefault().value;//ConfigurationManager.AppSettings["EmailSenderForPatientForm"].ToString().Trim();
                emailFromName = lstconfigkeys.Where(x => x.key == "EmailSenderNameForPatientForm").FirstOrDefault().value;// ConfigurationManager.AppSettings["EmailSenderNameForPatientForm"].ToString().Trim();
                subject = officeName + " has submitted a patient form";
                password = lstconfigkeys.Where(x => x.key == "PasswordForPatientSender").FirstOrDefault().value;//ConfigurationManager.AppSettings["PasswordForPatientSender"].ToString().Trim();
            }
            else if (callFrom == "BILLING_FORM")
            {
                emailFrom = lstconfigkeys.Where(x => x.key == "EmailSenderForBillingLog").FirstOrDefault().value;//ConfigurationManager.AppSettings["EmailSenderForBillingLog"].ToString().Trim();
                emailFromName = lstconfigkeys.Where(x => x.key == "EmailSenderNameForBillingLog").FirstOrDefault().value;// ConfigurationManager.AppSettings["EmailSenderNameForBillingLog"].ToString().Trim();
                subject = officeName + " has submitted a billing form";
                password = lstconfigkeys.Where(x => x.key == "PasswordForBillingSender").FirstOrDefault().value; //ConfigurationManager.AppSettings["PasswordForBillingSender"].ToString().Trim();
            }

            string pathToFiles = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();

            string body = "Dear Medical Billing Professionals," +
                           "<br/><br/>" +
                       subject +
                        "<br/>" +
                       "Click here to go to Form Management Center: <a href='" + pathToFiles + "/Account/Login'>https://www.mbpros.com/Account/Login</a>" +
                        "<br/><br/>" +
                       "Regards," +
                        "<br/>" +
                        "Notification Mailer";
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom, emailFromName);
                    User user = CommonFunctions.GetAdminUserDetails();
                    if (user != null)
                    {
                        mail.To.Add(new MailAddress(user.EmailID, user.UserName));
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            smtp.Credentials = new NetworkCredential(emailFrom, password);
                            smtp.EnableSsl = false;
                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch (Exception ex) {
                logger.Error(ex.ToString());
                logger.Info("Action send mail has been fired.");
            }
        }

        public static List<string> UserMenuAccess(int userid)
        {
            MbprosEntities en = new MbprosEntities();
            var lstFeatures = (from u in en.Feature_User
                            join m in en.FeatureMasters on u.FeatureID equals m.FeatureID 
                            where u.UserID == userid && u.CanView == true
                            select m.FeatureName).ToList();


            return lstFeatures;
        }
    }
}