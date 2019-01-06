using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Forum_v1.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Subjects
        public ActionResult Index(int? id)
        {
            ViewBag.SubjectId = id;
            Subject Subject = db.Subjects.Find(id);
            ViewBag.Subject = Subject;
            var mesaje = from Message in Subject.Messages select Message;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View(mesaje.ToList());
        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Create(int? id)
        {
            //Debug.WriteLine(id);
            ViewBag.SubjectId = id;
            return View("Create");

        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Text,Date,UserId,SubjectID")] Message message)
        {
            message.UserId = User.Identity.GetUserId();
            message.Date = DateTime.Now;
            /*Debug.WriteLine(subject.SubjectId);
            Debug.WriteLine(subject.Title);
            Debug.WriteLine(subject.Description);
            Debug.WriteLine(subject.Date);
            Debug.WriteLine(subject.UserId);*/
            //Debug.WriteLine(subject.CategoryID);

            if (ModelState.IsValid)
            {
                //Debug.WriteLine("A ajuns unde trebuie");
                db.Messages.Add(message);
                db.SaveChanges();

            }

            return Redirect("/Messages/Index/" + message.SubjectID);
        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            if (message.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
            {
                ViewBag.message = message;
                return View(message);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a modifca mesaje care nu va apartin!";
                return Redirect("/Messages/Index/" + message.SubjectID);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Edit(int MessageId, Message newData)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Message message = db.Messages.Find(MessageId);
                    if (message.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
                    {
                        if (TryUpdateModel(message))
                        {
                            message.Text = newData.Text;
                            db.SaveChanges();
                            TempData["message"] = "Mesajul a fost editat cu succes";
                        }

                        return Redirect("/Messages/Index/" + message.SubjectID);
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul de a modifca mesaje care nu sunt create de dumneavoastra!";
                        return Redirect("/Messages/Index/" + message.SubjectID);
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);

            if (message == null)
            {
                return HttpNotFound();
            }

            if (message.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
            {
                ViewBag.message = message;
                return View(message);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a sterge mesaje care nu va apartin!";
                return Redirect("/Messages/Index/" + message.SubjectID);
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);

            if (message == null)
            {
                return HttpNotFound();
            }
            if (message.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
            {
                db.Messages.Remove(message);
                db.SaveChanges();
                TempData["message"] = "Mesajul a fost sters!";
                return Redirect("/Messages/Index/" + message.SubjectID);

            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a sterge mesaje care nu va apartin!";
                return Redirect("/Messages/Index/" + message.SubjectID);
            }
        }
    }
}