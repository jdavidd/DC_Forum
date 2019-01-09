using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Forum_v1.Controllers
{
    public class UserEditController : Controller
    {
        // GET: UserEdit
        private ApplicationDbContext context = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser UserE = context.Users.FirstOrDefault(x => x.Id == userId);
            UserEdit NewUser = new UserEdit();

            NewUser.Email = UserE.Email;
            NewUser.FirstName = UserE.FirstName;
            NewUser.LastName = UserE.LastName;
            NewUser.Adress = UserE.Adress;
            NewUser.City = UserE.City;
            NewUser.State = UserE.State;
            NewUser.UserPhoto = UserE.UserPhoto;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View(NewUser);
        }
        public ActionResult UserShow(string id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser UserE = context.Users.FirstOrDefault(x => x.Id == id);
            UserEdit NewUser = new UserEdit();

            NewUser.Email = UserE.Email;
            NewUser.FirstName = UserE.FirstName;
            NewUser.LastName = UserE.LastName;
            NewUser.Adress = UserE.Adress;
            NewUser.City = UserE.City;
            NewUser.State = UserE.State;
            NewUser.UserPhoto = UserE.UserPhoto;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.id = id;
            return View(NewUser);
        }

        public ActionResult Edit()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser UserE = context.Users.FirstOrDefault(x => x.Id == userId);
            UserEdit NewUser = new UserEdit();
            NewUser.Email = UserE.Email;
            NewUser.FirstName = UserE.FirstName;
            NewUser.LastName = UserE.LastName;
            NewUser.Adress = UserE.Adress;
            NewUser.City = UserE.City;
            NewUser.State = UserE.State;
            NewUser.UserPhoto = UserE.UserPhoto;
            return View(NewUser);
        }
        [HttpPut]
        public ActionResult Edit([Bind(Exclude = "UserPhoto")] UserEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    byte[] imageData = null;
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];
                            using (var binary = new BinaryReader(poImgFile.InputStream))

                            {
                                imageData = binary.ReadBytes(poImgFile.ContentLength);
                            }
                    }
                    string userId = User.Identity.GetUserId();
                    ApplicationUser UserEdited = context.Users.FirstOrDefault(x => x.Id == userId);
                    /*
                    if (TryUpdateModel(UserEdited))
                    {
                        UserEdited.FirstName = model.FirstName;
                        UserEdited.LastName = model.LastName;
                        UserEdited.Adress = model.Adress;
                        UserEdited.City = model.City;
                        UserEdited.State = model.State;
                        UserEdited.UserPhoto = imageData;
                        context.SaveChanges();
                        TempData["message"] = "Profilul a fost editat!";
                    }
                   */
                    UserEdited.FirstName = model.FirstName;
                    UserEdited.LastName = model.LastName;
                    UserEdited.Adress = model.Adress;
                    UserEdited.City = model.City;
                    UserEdited.State = model.State;
                    if (imageData != null && imageData.Length > 0)
                        UserEdited.UserPhoto = imageData;
                    context.SaveChanges();
                    TempData["message"] = "Profilul a fost editat!";
                    return RedirectToAction("Index");

                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception e )
            {
                return View(model);
            }
        }
    }
}