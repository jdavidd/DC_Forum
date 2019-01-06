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
        public ActionResult Index(int? id, int page = 0, int sortare = 0)
        {
            ViewBag.CategoryId = id;
            Category Category = db.Categories.Find(id);
            ViewBag.Category = Category;
            var subiecte = from Subject in Category.Subjects select Subject;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var subiectele = subiecte.ToList();

            int nrSub = subiecte.ToList().Count();
            ViewBag.NrPages = nrSub / 10;
            if (nrSub % 10 != 0)
                ViewBag.NrPages = ViewBag.NrPages + 1;

            if (page < 0)
                page = 0;
            ViewBag.pagina = page;
            if (((page * 10) > subiectele.Count() - 1))
            {
                page = ViewBag.NrPages - 1;
                ViewBag.pagina = ViewBag.NrPages;
            }


            if (sortare == 0) ViewBag.tipSort = 0;//sortare==0 inseamna ordonare dupa data de la cel mai vechi la cel mai nou, nu e nevoie de sortare
            if (sortare == 1)    //sortare==1 inseamna ordonare alfabetica dupa titlu
            {
                subiecte = subiecte.OrderBy(o => o.Title).ToList();
                ViewBag.tipSort = 1;
            }
            if (sortare == 2)    //sortare==2 inseamna sortare dupa numele creatorului subiectului
            {
                subiecte = subiecte.OrderBy(o => o.User.FirstName).ThenBy(o => o.User.LastName).ToList();
                ViewBag.tipSort = 2;
            }
            if (sortare == 3)    //sortare==3 inseamna sortare dupa data, de la cel mai nou la cel mai vechi
            {
                subiecte = subiecte.OrderByDescending(o => o.Date).ToList();
                ViewBag.tipSort = 3;
            }

            if ((page * 10) + 9 <= subiectele.Count() - 1)
                return View(subiecte.ToList().GetRange(page * 10, 10));
            else
                return View(subiecte.ToList().GetRange(page * 10, subiectele.Count() - (page * 10)));
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
                return Redirect("/Subjects/Index/" + subject.CategoryID);

            }
            else
            {
                ViewBag.CategoryId = subject.CategoryID;
                return View("Create");
            }

        }
        // GET: Categories/Edit/5
        [Authorize(Roles = "User,Moderator,Administrator")]
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
            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
            {
                ViewBag.subject = subject;
                var categorii = GetAllCategories();
                ViewBag.ListaCategorii = categorii;
                ViewBag.categorie = subject.CategoryID.ToString();
                return View(subject);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a modifca subiecte care nu sunt create de dumneavoastra!";
                return Redirect("/Subjects/Index/" + subject.CategoryID);
            }
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categorii = from Category in db.Categories select Category;
            foreach (var categorie in categorii)
            {
                selectList.Add(new SelectListItem
                {
                    Value = categorie.CategoryID.ToString(),
                    Text = categorie.Title.ToString()
                });
            }
            return selectList;
        }
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Moderator,Administrator")]
        public ActionResult Edit(int SubjectId, Subject newData)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Subject subject = db.Subjects.Find(SubjectId);
                    if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
                    {
                        if (TryUpdateModel(subject))
                        {
                            subject.Title = newData.Title;
                            subject.Description = newData.Description;
                            if (User.IsInRole("Administrator") || User.IsInRole("Moderator"))
                            {
                                subject.CategoryID = Int32.Parse(HttpContext.Request.Params.Get("newCategory"));
                            }
                            db.SaveChanges();
                            TempData["message"] = "Subiect editat cu succes";
                        }

                        return Redirect("/Subjects/Index/" + subject.CategoryID);
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul de a modifca subiecte care nu sunt create de dumneavoastra!";
                        return Redirect("/Subjects/Index/" + subject.CategoryID);
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
            Subject subject = db.Subjects.Find(id);

            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubTitle = subject.Title;
            ViewBag.subject = subject;
            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
            {
                ViewBag.subject = subject;
                return View(subject);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a sterge subiecte care nu sunt create de dumneavoastra!";
                return Redirect("/Subjects/Index/" + subject.CategoryID);
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
            Subject subject = db.Subjects.Find(id);

            if (subject == null)
            {
                return HttpNotFound();
            }
            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.IsInRole("Moderator"))
            {
                db.Subjects.Remove(subject);
                db.SaveChanges();
                TempData["message"] = "Subiectul a fost sters!";
                return Redirect("/Subjects/Index/" + subject.CategoryID);

            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a sterge subiecte care nu au fost create de dumneavoastra!";
                return Redirect("/Subjects/Index/" + subject.CategoryID);
            }
        }

    }
}