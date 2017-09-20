using ChoixResto.Filters;
using ChoixResto.Models;
using ChoixResto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class VoteController : Controller
    {
        private IDal dal;

        public VoteController() : this(new Dal())
        {
        }

        public VoteController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        // ***** Sécuriser la méthode individuellement *****
        //[Authorize]
        // ***** Désécurisé la méthode individuellement *****
        //[AllowAnonymous]
        public ActionResult Index(int id)
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = dal.ObtientTousLesRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NomEtTelephone = string.Format("{0} ({1})", r.Nom, r.Telephone) }).ToList()
            };
            if (dal.ADejaVote(id, Request.Browser.Browser))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            // Utilisateur utilisateur = dal.ObtenirUtilisateur(Request.Browser.Browser);
            Utilisateur utilisateur = dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (utilisateur == null)
                return new HttpUnauthorizedResult();
            foreach (RestaurantCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListeDesResto.Where(r => r.EstSelectionne))
            {
                dal.AjouterVote(id, restaurantCheckBoxViewModel.Id, utilisateur.Id);
            }
            return RedirectToAction("AfficheResultat", new { id = id });
        }

        public ActionResult AfficheResultat(int id)
        {
            if (!dal.ADejaVote(id, HttpContext.User.Identity.Name))
            {
                return RedirectToAction("Index", new { id = id });
            }
            ViewBag.Id = id;
            return View();
        }

        [AjaxFilter]
        public ActionResult AfficheTableau(int id)
        {
            List<Resultats> resultats = dal.ObtenirLesResultats(id);
            return PartialView(resultats.OrderByDescending(r => r.NombreDeVotes).ToList());
        }

        public ActionResult AfficheGraphique(int id)
        {
            List<Resultats> resultats = dal.ObtenirLesResultats(id);

            Chart graphique = new Chart(width: 600, height: 400)
            .AddTitle("Résultats")
            .AddSeries(
            xValue: resultats.Select(resto => resto.Nom).ToArray(),
            yValues: resultats.Select(resto => resto.NombreDeVotes.ToString()).ToArray());
            return File(graphique.GetBytes("png"), "image/png");
        }
    }
}