using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoixResto.Models;
using ChoixResto.ViewModels;
using System.Threading;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController() : this(new Dal())
        {
        }

        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
            return View(listeDesRestaurants);
        }

        // Limite l'accès au seul compte administrateur
        // [Authorize(Roles="Administrateur")]
        public ActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            if (dal.RestaurantExiste(resto.Nom))
            {
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                return View(resto);
            }
            if (!ModelState.IsValid)
                return View(resto);
            dal.CreerRestaurant(resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }

        public ActionResult RechercheSansAjax(RechercheViewModel rechercheViewModel)
        {
            if (!string.IsNullOrWhiteSpace(rechercheViewModel.Recherche))
                rechercheViewModel.ListeDesRestos = dal.ObtientTousLesRestaurants().Where(r => r.Nom.IndexOf(rechercheViewModel.Recherche, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
            else
                rechercheViewModel.ListeDesRestos = new List<Resto>();
            return View(rechercheViewModel);
        }

        public ActionResult Recherche(RechercheViewModel rechercheViewModel)
        {
            return View(rechercheViewModel);
        }

        //public ActionResult ResultatsRecherche(RechercheViewModel rechercheViewModel)
        //{
        //    if (!string.IsNullOrWhiteSpace(rechercheViewModel.Recherche))
        //        rechercheViewModel.ListeDesRestos = dal.ObtientTousLesRestaurants().Where(r => r.Nom.IndexOf(rechercheViewModel.Recherche, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();

        //    else
        //        rechercheViewModel.ListeDesRestos = new List<Resto>();
        //    return PartialView(rechercheViewModel);
        //}

        public ActionResult ResultatsRecherche(RechercheViewModel rechercheViewModel)
        {
            if (!string.IsNullOrWhiteSpace(rechercheViewModel.Recherche))
            {
                rechercheViewModel.ListeDesRestos = dal.ObtientTousLesRestaurants().Where(r => r.Nom.IndexOf(rechercheViewModel.Recherche, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                Thread.Sleep(1500);
            }
            else
                rechercheViewModel.ListeDesRestos = new List<Resto>();
            return PartialView(rechercheViewModel);
        }
    }
}