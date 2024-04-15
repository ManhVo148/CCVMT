using DoAnCoSo.Models;

using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Speech.Synthesis;

using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
namespace DoAnCoSo.Controllers

{
    public class TruyenController : Controller
    {
        TruyenModel dbContext = new TruyenModel();











        public ActionResult ChiTiet(string sortOrder, string currentFilter, string searchString, int? page, int id)
        {
            var checkChap = dbContext.Chapters.FirstOrDefault(p => p.story_id == id);

            if (checkChap == null)
            {
                TempData["Message"] = "Truyện chưa cập nhật";
                return RedirectToAction("Index", "Truyen");
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceDes = "price_Des";
            ViewBag.PriceAsc = "price_Asc";


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


            var listChap = dbContext.Chapters.Where(p => p.story_id == id);
            if (listChap != null)
            {

                int pageSize = 30;
                int pageIndex = page.HasValue ? page.Value : 1;
                return View(listChap.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return HttpNotFound("khong tim thay");
            }
        }



        public ActionResult DocTruyen(int id)
        {

            var chap = dbContext.Chapters.FirstOrDefault(p => p.chapter_id == id);

            if (chap != null)
            {

                return View(chap);
            }
            else
            {
                return HttpNotFound("khong tim thay");
            }

        }






    }




}