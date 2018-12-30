using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum_v1.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Subjects
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Create(int?id)
        {

            ViewBag.CategoryId = id;
            return View("CreateSubjectView");
            
        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectID,Title,Description,Date,UserId,CategoryID")] Subject subject)
        {
            subject.UserId = User.Identity.GetUserId();
            subject.Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Hour, DateTime.Today.Minute, DateTime.Today.Second);
            Debug.WriteLine(subject.SubjectId);
            Debug.WriteLine(subject.Title);
            Debug.WriteLine(subject.Description);
            Debug.WriteLine(subject.Date);
            Debug.WriteLine(subject.UserId);
            Debug.WriteLine(subject.CategoryID);

            if (ModelState.IsValid)
            {
                Debug.WriteLine("A ajuns unde trebuie");
                db.Subjects.Add(subject);
                db.SaveChanges();
                
            }

            return Redirect("/Subjects/Index");
        }
    }
}