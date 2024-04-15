using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DoAnCoSo.ViewModels
{
    public class ChangePassword
    {
        [StringLength(150)]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }

        [StringLength(32)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [StringLength(32)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("newPassword", ErrorMessage = "Do not match")]
        public string confirmPassword { get; set; }

        [StringLength(32)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string newPassword { get; set; }
    }
}