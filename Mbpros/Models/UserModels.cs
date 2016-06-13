using Mbpros.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mbpros.Models
{
    public class UserModels
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please enter the user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter the password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter the office name")]
        public string OfficeName { get; set; }
        [Required(ErrorMessage = "Please enter the user email ID")]
        [EmailAddress(ErrorMessage = "The User EmailID field is not a valid e-mail address.")]
        public string EmailID { get; set; }
        //public bool Status { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? CreatedDate	{ get; set; }
        public DateTime? UpdatedDate { get; set; }
       // public bool IsUserExist { get; set; }

        public List<User> UserList { get; set; }
    }

    public class UserEmailModel
    {
        public int UserID { get; set; }
        //public string FromUserName { get; set; }
        //public string FromEmail { get; set; }
        public string ToUserName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class Options
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}