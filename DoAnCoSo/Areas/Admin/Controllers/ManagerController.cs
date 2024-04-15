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







}



