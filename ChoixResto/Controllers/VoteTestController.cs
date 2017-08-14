using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoixResto.Models;
using ChoixResto.ViewModels;

namespace ChoixResto.Controllers
{
    // Sécurise un contrôleur
    [Authorize]
    public class VoteTestController : Controller
    {
        private IDal dal;

        public VoteTestController() : this(new Dal())
        {
        }

        public VoteTestController(IDal dalIoc)
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
            // if (!dal.ADejaVote(id, Request.Browser.Browser))
            if (!dal.ADejaVote(id, HttpContext.User.Identity.Name))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Resultats> resultats = dal.ObtenirLesResultats(id);
            return View(resultats.OrderByDescending(r => r.NombreDeVotes).ToList());
        }
    }
}