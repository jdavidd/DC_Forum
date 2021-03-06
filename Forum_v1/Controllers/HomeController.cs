﻿using System;
using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Forum_v1.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult About()
        {
            string currentTime = DateTime.Now.ToLongTimeString();
            ViewBag.Message = "The current time is " + currentTime;
            return View();
        }
        public ActionResult Index()
        {
            var top_subjects = db.Subjects.OrderByDescending(t => t.Date).Take(12);
            var count_subject = db.Subjects.Select(o => o.SubjectId).Count();
            ViewBag.count_subject = count_subject;
            ViewBag.last_subject = top_subjects.Take(1);
            var categories = db.Categories.ToList();
            var tuple = new Tuple<IEnumerable<Category>, IEnumerable <Subject> >(categories, top_subjects);
            return View(tuple);
        }

        public JsonResult GetLastSubject(int id)
        {
            Category Category = db.Categories.Find(id);
            ViewBag.Category = Category;
            var subiecte = from Subject in Category.Subjects select Subject;
            var s = subiecte.OrderByDescending(t => t.Date).Count();

            if (s == 0)
            {
                return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
            }
            else {
            var subiect = subiecte.OrderByDescending(t => t.Date).First();
            return Json( new {
                               Count = 1,
                               Title = subiect.Title,
                               FirstName = subiect.User.FirstName,
                               LastName = subiect.User.LastName,
                               Date = subiect.Date,
                               CategoryID = subiect.CategoryID,
                               SubjectID = subiect.SubjectId,
                               UserID = subiect.UserId
                              }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNumberOfSubjects(int id)
        {
            Category Category = db.Categories.Find(id);
            ViewBag.Category = Category;
            var subiecte = from Subject in Category.Subjects select Subject;
            var count = subiecte.Count();

            return Json(new { Count = count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileContentResult UserPhotos ()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();

                string fileName = HttpContext.Server.MapPath(@"/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                if (userId == null)
                    return File(imageData, "image/png");

                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userImage.UserPhoto == null || userImage.UserPhoto.Length <= 0)
                    return File(imageData, "image/png");
                else
                    return new FileContentResult(userImage.UserPhoto, "image/jpeg");

            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");

            }
        }

        public FileContentResult GetPhoto(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string fileName = HttpContext.Server.MapPath(@"/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                if (id == null)
                    return File(imageData, "image/png");

                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == id).FirstOrDefault();
                if (userImage.UserPhoto == null || userImage.UserPhoto.Length <= 0)
                    return File(imageData, "image/png");
                else
                    return new FileContentResult(userImage.UserPhoto, "image/jpeg");

            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");

            }
        }
        public FileContentResult GetLogo()
        {
            string fileName = HttpContext.Server.MapPath(@"/Images/logo.png");
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(fileName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);

            return File(imageData, "image/png");

        }
    }
}