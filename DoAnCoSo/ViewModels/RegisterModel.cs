using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DoAnCoSo.ViewModels
{
    public class RegisterModel
    {
        [StringLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ họ tên")]
        [Display(Name = "Họ và tên")]
        public string fullname { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string email { get; set; }

        //[StringLength(10)]
        //[Required(ErrorMessage = "Số điện thoại không được để trống")]
        //[Display(Name = "Số điện thoại")]
        //public string phone_number { get; set; }


        [StringLength(32)]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string password { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string confirmPassword { get; set; }

        public int? role_id { get; set; }

    }
}