using Forum_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum_v1.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Show(string id)
        {
            ApplicationUser UserE = db.Users.FirstOrDefault(x => x.Id == id);
            var imageData = UserE.UserPhoto;

            return File(imageData, "imageUser/jpg");
        }
    }
}