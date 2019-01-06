using Forum_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum_v1.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Results()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string word = HttpContext.Request.Params.Get("Cautare");
            ViewBag.word = word;
            word = word.ToUpper();

            var subiecte = from Subject in db.Subjects select Subject;
            var listaSubiecte = subiecte.ToList();
            int[] index = new int[listaSubiecte.Count()];
            int indice = 0;
            ICollection<Subject> listaRezultateSubiecte = new List<Subject>();
            foreach (var subject in listaSubiecte)
            {

                index[indice] = 0;
                if (subject.Title.ToUpper().Contains(word))
                {
                    index[indice] = 1;
                    listaRezultateSubiecte.Add(subject);
                }
                indice++;
            }
            indice = 0;
            foreach (var subject in listaSubiecte)
                if (index[indice] == 0)
                {
                     if (subject.Description.ToUpper().Contains(word) || subject.Date.ToString().ToUpper().Contains(word) || subject.User.FirstName.ToUpper().Contains(word) || subject.User.LastName.ToUpper().Contains(word))
                    {
                        listaRezultateSubiecte.Add(subject);
                        index[indice] = 1;
                    }
                    indice++;
                }
            if (listaRezultateSubiecte.Any())
                ViewBag.Subiecte = listaRezultateSubiecte;

            var useri = from User in db.Users select User;
            var listaUseri = useri.ToList();
            var listaRezultateUseri = new List<ApplicationUser>();
            index = new int[listaUseri.Count()];
            indice = 0;
            foreach (var user in listaUseri)
            {
                index[indice] = 0;
                if (user.FirstName.ToUpper().Contains(word) || user.LastName.ToUpper().Contains(word))
                {
                    listaRezultateUseri.Add(user);
                    index[indice] = 1;
                }
                indice++;
            }
            if (listaRezultateUseri.Any())
                ViewBag.Useri = listaRezultateUseri;

            var message = from Message in db.Messages select Message;
            var listaMessages = message.ToList();
            var listaRezultateMesaje = new List<Message>();
            index = new int[listaMessages.Count()];
            indice = 0;
            foreach (var mesaj in listaMessages)
            {
                index[indice] = 0;
                if (mesaj.Text.ToUpper().Contains(word))
                {
                    listaRezultateMesaje.Add(mesaj);
                    index[indice] = 1;
                }
                indice++;
            }
            if (listaRezultateMesaje.Any())
                ViewBag.Mesaje = listaRezultateMesaje;



            return View();
        }
    }
}