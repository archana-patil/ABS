using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mbpros.Models
{
    public class LoginModel
    {
        public int UserID{ get; set; }

        [Required(ErrorMessage="The User name field is required.")]
        public string LoginUserName { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public string LoginUserPassword { get; set; }

    }
}