using DoAnCoSo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Speech.Synthesis;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {


            return View();
        }



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

        //////////////////////////////////////////////PRODUCT
        // GET: Admin/Manager
        TruyenModel dbContext = new TruyenModel();
        public ActionResult ManagerProduct(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.AdminPriceDes = "price_Des";
            ViewBag.AdminPriceAsc = "price_Asc";

            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var product = from s in dbContext.Stories
                              select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    product = product.Where(s => s.title.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "price_Des":
                        product = product.OrderByDescending(s => s.title);
                        break;
                    case "price_Asc":
                        product = product.OrderBy(s => s.title);
                        break;
                    default:
                        product = product.OrderBy(s => s.title);
                        break;
                }


                int pageSize = 6;
                int pageIndex = page.HasValue ? page.Value : 1;

                return View(product.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }


        }



        /////////////////////////////////Create
        [HttpGet]
        public ActionResult CreateProduct()
        {
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                ViewBag.ListCategory = dbContext.Genres.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }


        }

        [HttpPost]
        public ActionResult CreateProduct(Story newProduct)
        {

            if (ModelState.IsValid)
            {
                //if (newProduct.thumbnail != null && newProduct.ImageFile.ContentLength > 0)
                //{
                //    var fileName = Path.GetFileName(newProduct.ImageFile.FileName);
                //    var filePath = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                //    newProduct.ImageFile.SaveAs(filePath);
                //    newProduct.thumbnail = "/Content/Images/" + fileName;
                //}
                dbContext.Stories.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("ManagerProduct");
            }
            else
                return RedirectToAction("CreateProduct", "Manager");
        }


        [HttpGet]
        public ActionResult EditProduct(int id)
        {

            //string admin = (string)Session["Role"];
            //if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            //{
            var _product = dbContext.Stories.FirstOrDefault(p => p.story_id == id);
            ViewBag.ListCategory = dbContext.Genres.ToList();
            if (_product != null)
            {
                return View(_product);
            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
            //}
            //else
            //{
            //    return RedirectToAction("DangNhap", "Account", new { area = "" });
            //}
        }

        [HttpPost]
        public ActionResult EditProduct(Story editProduct)
        {

            var _product = dbContext.Stories.FirstOrDefault(p => p.story_id == editProduct.story_id);
            if (_product != null)
            {
                //var f_password = GetMD5(editUser.password);

                _product.title = editProduct.title;
                _product.author = editProduct.author;
                _product.cover_image_url = editProduct.cover_image_url;
                _product.description = editProduct.description;
                dbContext.SaveChanges();
                return RedirectToAction("ManagerProduct", "Manager");
            }
            else
            {
                return HttpNotFound("tim khong thay");
            }
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            var _product = dbContext.Stories.FirstOrDefault(p => p.story_id == id);
            return View(_product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(Story delProduct)
        {
            var _product = dbContext.Stories.FirstOrDefault(p => p.story_id == delProduct.story_id);
            if (_product != null)
            {
                var check = dbContext.Chapters.FirstOrDefault(p => p.story_id == _product.story_id);
                if (check != null)
                {
                    TempData["Message"] = "Không thể xóa truyện này";
                    return RedirectToAction("ManagerProduct", "Manager");
                }
                else
                {


                    dbContext.Stories.Remove(_product);
                    dbContext.SaveChanges();
                    return RedirectToAction("ManagerProduct", "Manager");
                }


            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
        }



////////////////////////////////////////////////////////////USER
public ActionResult ManagerUser(string sortUser, string currentFilter, string searchString, int? page)
{
    ViewBag.CurrentSort = sortUser;
    ViewBag.AdminUserDes = "user_Des";
    ViewBag.AdminUserAsc = "user_Asc";


    string admin = (string)Session["Role"];
    if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
    {
        if (searchString != null)
        {
            page = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        ViewBag.CurrentFilter = searchString;

        var user = from s in dbContext.Users
                   select s;
        if (!String.IsNullOrEmpty(searchString))
        {
            user = user.Where(s => s.username.Contains(searchString));
        }


        switch (sortUser)
        {
            case "user_Des":
                user = user.OrderByDescending(s => s.username);
                break;
            case "user_Asc":
                user = user.OrderBy(s => s.username);
                break;
            default:  // Name ascending                    
                break;
        }


        int pageSize = 6;
        int pageIndex = page.HasValue ? page.Value : 1;



        //  var listProduct = dbContext.Product.Where(p =>p.title == name)
        //var listProduct = dbContext.Product.ToList();
        return View(user.ToList().ToPagedList(pageIndex, pageSize));
    }
    else
    {
        return RedirectToAction("DangNhap", "Account", new { area = "" });
    }
}

[HttpGet]
public ActionResult CreateUser()
{
    string admin = (string)Session["Role"];
    if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
    {
        ViewBag.ListRoleUser = dbContext.Roles.ToList();
        return View();
    }
    else
    {
        return RedirectToAction("Login", "Account", new { area = "" });
    }



}

[HttpPost]
public ActionResult CreateUser(User newUser)
{
    newUser.password = GetMD5(newUser.password);

    if (ModelState.IsValid)
    {
        var check = dbContext.Users.FirstOrDefault(s => s.email == newUser.email);
        if (check == null)
        {
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges(); ;
            return RedirectToAction("ManagerUser");
        }
        else
        {
            ViewBag.error = "Email đã tồn tại";
            ViewBag.ListRoleUser = dbContext.Roles.ToList();
            return View();
        }
    }
    else
        return RedirectToAction("CreateUser");
}

public ActionResult EditUser(int id)
{
    var _user = dbContext.Users.FirstOrDefault(p => p.user_id == id);
    ViewBag.ListRoleUser = dbContext.Roles.ToList();
    return View(_user);
}
[HttpPost]
public ActionResult EditUser(User editUser)
{

    var _user = dbContext.Users.FirstOrDefault(p => p.user_id == editUser.user_id);
    if (_user != null)
    {
        var f_password = GetMD5(editUser.password);
        _user.username = editUser.username;
        _user.role_id = editUser.role_id;
        _user.password = f_password;
        _user.email = editUser.email;
        dbContext.SaveChanges();
        return RedirectToAction("ManagerUser", "Manager");
    }
    else
    {
        return HttpNotFound("khong tim thay hoac loi");
    }
}

public ActionResult DeleteUser(int id)
{
    var _User = dbContext.Users.FirstOrDefault(p => p.user_id == id);
    return View(_User);
}

[HttpPost]
public ActionResult DeleteUser(User delUser)
{


    var _User = dbContext.Users.FirstOrDefault(p => p.user_id == delUser.user_id);
    if (_User != null)
    {
        var _UserFind = dbContext.Bookmarks.FirstOrDefault(p => p.user_id == _User.user_id);
        var _UserFind1 = dbContext.Ratings.FirstOrDefault(p => p.user_id == _User.user_id);
        if (_UserFind != null || _UserFind1 != null)
        {
            TempData["Message"] = "Ban khong the xoa nguoi dung nay";
            return View();

        }
        else
        {
            dbContext.Users.Remove(_User);
            dbContext.SaveChanges();
            return RedirectToAction("ManagerUser", "Manager");
        }
    }
    else
    {
        ViewBag.ErroDellUser1 = "Khong co nguoi dung hoac nguoi dung da bi xoa";
        return View();
    }

}






}



