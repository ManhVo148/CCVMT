using DoAnCoSo.Models;
using DoAnCoSo.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class AccountController : Controller
    {
        TruyenModel dbContext = new TruyenModel();
       
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public ActionResult DangKy()
        {
           
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(RegisterModel user)
        {
            TempData["Message"] = null;
            var dbContext = new TruyenModel();
            var newUser = new User();
            if (ModelState.IsValid)
            {
                var check = dbContext.Users.FirstOrDefault(s => s.email == user.email);
                if (check == null)
                {
                    newUser.role_id = 2;
                    user.password = GetMD5(user.password);
                    dbContext.Configuration.ValidateOnSaveEnabled = false;
                    newUser.email = user.email;
                    newUser.username = user.fullname;
                    newUser.password = user.password;
                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                    TempData["Message"] = "Đăng ký thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";
                    return View();
                }


            }
            return View();
        }



        // Login

        public ActionResult DangNhap()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string email, string password)
        {
            TempData["Message"] = null;
            var dbContext = new TruyenModel();
            if (ModelState.IsValid)
            {

                var f_password = GetMD5(password);
                var data = dbContext.Users.Where(s => s.email.Equals(email) && s.password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {

                    Session["FullName"] = data.FirstOrDefault().username;
                    Session["idUser"] = data.FirstOrDefault().user_id;
                    Session["Role"] = data.FirstOrDefault().Role.role_name;
                    TempData["Message"] = "Đăng nhập thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Sai email hoặc mật khẩu";
                    return RedirectToAction("DangNhap", "Account"); ;
                }
            }
            return View();
        }


        public ActionResult LogOff()
        {
           
            Session.Clear();
            TempData["Message"] = null;
            return RedirectToAction("Index", "Home");
        }



        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword editUser)
        {
            var dbContext = new TruyenModel();
            var f_password = GetMD5(editUser.password);
            var check = dbContext.Users.FirstOrDefault(s => s.email.Equals(editUser.email) && s.password.Equals(f_password));
            if (check != null)
            {
                var new_password = GetMD5(editUser.newPassword);
                check.password = new_password;
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return HttpNotFound("tim khong thay");
            }

        }
       


}
