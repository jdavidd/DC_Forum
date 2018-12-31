using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Forum_v1.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Subjects
        public ActionResult Index(int? id)
        {
            ViewBag.CategoryId = id;
            Category Category = db.Categories.Find(id);
            ViewBag.Category = Category;
            var subiecte = from Subject in Category.Subjects select Subject;

            return View(subiecte.ToList());
        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Create(int? id)
        {
            Debug.WriteLine(id);
            ViewBag.CategoryId = id;
            return View("Create");

        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectID,Title,Description,Date,UserId,CategoryID")] Subject subject)
        {
            subject.UserId = User.Identity.GetUserId();
            subject.Date = DateTime.Now;
            /*Debug.WriteLine(subject.SubjectId);
            Debug.WriteLine(subject.Title);
            Debug.WriteLine(subject.Description);
            Debug.WriteLine(subject.Date);
            Debug.WriteLine(subject.UserId);*/
            //Debug.WriteLine(subject.CategoryID);

            if (ModelState.IsValid)
            {
                //Debug.WriteLine("A ajuns unde trebuie");
                db.Subjects.Add(subject);
                db.SaveChanges();

            }

            return Redirect("/Subjects/Index/" + subject.CategoryID);
        }
        // GET: Categories/Edit/5
        [Authorize(Roles = "Moderator,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.subject = subject;
            return View("Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Edit([Bind(Include = "SubjectID,Title,Description,Date,UserId,CategoryID")] Subject subject)
        {
            subject.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Redirect("/Subjects/Index/" + subject.CategoryID);
        }
    }
}