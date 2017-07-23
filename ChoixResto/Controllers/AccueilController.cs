using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoixResto.Models;
using ChoixResto.ViewModels;

namespace ChoixResto.Controllers
{
    public class AccueilController : Controller
    {
        // GET: Accueil
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexExemple()
        {
            // Afficher une vue :
            //
            // return View();
            // return View("Index");
            // return View("Bonjour");
            // return View("~/Views/Test/Essai.cshtml");

            // ViewData, dictionnaire fourre-tout où nous pouvons mettre des objets en les associant à des clés.
            //
            ViewData["message"] = "Bonjour depuis le contrôleur !";
            ViewData["date"] = DateTime.Now;
            ViewData["resto"] = new Resto { Nom = "Choucroute party", Telephone = "01 60 14 80 55" };

            //dynamic resto = new Resto();
            //resto.Nom = "Resto dynam-hic";
            //resto.Adresse = "compile, mais ne fonctionnera pas";

            // ViewBag, Objet de type dynamic
            //
            ViewBag.message = "ReBonjour depuis le contrôleur !";
            ViewBag.date = DateTime.Now;
            ViewBag.resto = new Resto { Nom = "Pizza party", Telephone = "01 60 14 55 80" };

            //
            // return View();
            //

            // En passant un objet
            //
            Resto r = new Resto { Nom = "La bonne fourchette", Telephone = "1234" };
            return View(r);
        }

        public ActionResult IndexVM()
        {
            AccueilViewModel vm = new AccueilViewModel
            {
                Message = "Bonjour depuis le contrôleur !",
                Date = DateTime.Now,
                Resto = new Resto { Nom = "Poutine ville", Telephone = "123456" }
            };
            return View(vm);
        }

        public ActionResult DayOfWeek()
        {
            return View(DateTime.Now);
        }

        public ActionResult IndexRestoVM()
        {
            AccueilRestoViewModel rvm = new AccueilRestoViewModel
            {
                Message = "Bonjour depuis le <span style=\"color:red\">contrôleur</span>",
                Date = DateTime.Now,
                ListeDesRestos = new List<Resto>
                {
                    new Resto { Nom = "Resto pinambour", Telephone = "1234" },
                    new Resto { Nom = "Resto tologie", Telephone = "1234" },
                    new Resto { Nom = "Resto ride", Telephone = "5678" },
                    new Resto { Nom = "Resto toro", Telephone = "555" },
                }
            };
            return View(rvm);
        }

        public ActionResult SelectList()
        {
            List<Models.Resto> listeDesRestos = new List<Resto>
            {
                new Resto { Id = 1, Nom = "Resto pinambour", Telephone = "1234" },
                new Resto { Id = 2, Nom = "Resto tologie", Telephone = "1234" },
                new Resto { Id = 5, Nom = "Resto ride", Telephone = "5678" },
                new Resto { Id = 9, Nom = "Resto toro", Telephone = "555" },
            };

            ViewBag.ListeDesRestos = new SelectList(listeDesRestos, "Id", "Nom");
            // Pour préselectionner
            // ViewBag.ListeDesRestos = new SelectList(listeDesRestos, "Id", "Nom", 5);
            return View();
        }

        public ActionResult Formulaire()
        {
            
            return View();
        }
        public ActionResult HelperFortementTypee()
        {

            return View();
        }

        // Appelable que depuis une vue mère
        [ChildActionOnly]
        public ActionResult AfficheListeRestaurant()
        {
            List<Models.Resto> listeDesRestos = new List<Resto>
            {
                new Resto { Id = 1, Nom = "Resto pinambour", Telephone = "1234" },
                new Resto { Id = 2, Nom = "Resto tologie", Telephone = "1234" },
                new Resto { Id = 5, Nom = "Resto ride", Telephone = "5678" },
                new Resto { Id = 9, Nom = "Resto toro", Telephone = "555" },
            };
            return PartialView(listeDesRestos);
        }

        public ActionResult AfficheHTML()
        {
            AccueilViewModel vm = new AccueilViewModel
            {
                Message = "Bonjour depuis le <span style=\"color:red\">contrôleur</span>"
            };
            return View(vm);
        }
    }
}