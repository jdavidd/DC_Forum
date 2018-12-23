using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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

            NewUser.FirstName = UserE.FirstName;
            NewUser.FirstName = UserE.FirstName;
            NewUser.LastName = UserE.LastName;
            NewUser.Adress = UserE.Adress;
            NewUser.City = UserE.City;
            NewUser.State = UserE.State;
            ViewBag.UserEdit = NewUser;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        public ActionResult Edit()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser UserE = context.Users.FirstOrDefault(x => x.Id == userId);
            UserEdit NewUser = new UserEdit();
            NewUser.FirstName = UserE.FirstName;
            NewUser.LastName = UserE.LastName;
            NewUser.Adress = UserE.Adress;
            NewUser.City = UserE.City;
            NewUser.State = UserE.State;
           
            return View(NewUser);
        }
        [HttpPut]
        public async Task<ActionResult> Edit(UserEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userId = User.Identity.GetUserId();
                    ApplicationUser UserEdited = context.Users.FirstOrDefault(x => x.Id == userId);
                    if (TryUpdateModel(UserEdited))
                    {
                        UserEdited.FirstName = model.FirstName;
                        UserEdited.LastName = model.LastName;
                        UserEdited.Adress = model.Adress;
                        UserEdited.City = model.City;
                        UserEdited.State = model.State;
                        context.SaveChanges();
                        TempData["message"] = "Profilul a fost editat!";
                    }
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