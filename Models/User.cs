using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace anhemtoicodeweb.Models
{
    public class User
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name not empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password not empty")]
        public string Password { get; set; }
        public string Email { get; set; }

        [NotMapped]
        [Compare("Password")]
        [Display(Name = "Re-enter password")]
        public string ConfirmPass { get; set; }
        [NotMapped]
        public string ErrorLogin { get; set; }
    }
}