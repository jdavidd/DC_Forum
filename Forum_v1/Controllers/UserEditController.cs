using Forum_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }
    }
}