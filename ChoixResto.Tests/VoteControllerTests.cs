using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ChoixResto.Models;
using ChoixResto.Controllers;
using ChoixResto.ViewModels;
using System.Web.Mvc;
using System.Collections.Generic;
using Moq;

namespace ChoixResto.Tests
{
    [TestClass]
    public class VoteControllerTests
    {
        private IDal dal;
        private int idSondage;
        private VoteController controleur;

        [TestInitialize]
        public void Init()
        {
            dal = new DalEnDur();
            idSondage = dal.CreerUnSondage();

            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(p => p.HttpContext.Request.Browser.Browser).Returns("1");

            controleur = new VoteController(dal);
            controleur.ControllerContext = controllerContext.Object;
        }

        [TestMethod]
        public void Index_AvecSondageNormalMaisSansUtilisateur_RenvoiLeBonViewModelEtAfficheLaVue()
        {
            ViewResult view = (ViewResult)controleur.Index(idSondage);

            RestaurantVoteViewModel viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(3, viewModel.ListeDesResto.Count);
            Assert.AreEqual(1, viewModel.ListeDesResto[0].Id);
            Assert.IsFalse(viewModel.ListeDesResto[0].EstSelectionne);
            Assert.AreEqual("Resto pinambour (0102030405)", viewModel.ListeDesResto[0].NomEtTelephone);
        }

        [TestMethod]
        public void Index_AvecSondageNormalAvecUtilisateurNayantPasVote_RenvoiLeBonViewModelEtAfficheLaVue()
        {
            dal.AjouterUtilisateur("Nico", "1234");
            dal.AjouterUtilisateur("Jérémie", "1234");

            ViewResult view = (ViewResult)controleur.Index(idSondage);

            RestaurantVoteViewModel viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(3, viewModel.ListeDesResto.Count);
            Assert.AreEqual(1, viewModel.ListeDesResto[0].Id);
            Assert.IsFalse(viewModel.ListeDesResto[0].EstSelectionne);
            Assert.AreEqual("Resto pinambour (0102030405)", viewModel.ListeDesResto[0].NomEtTelephone);
        }

        [TestMethod]
        public void Index_AvecSondageNormalMaisDejaVote_EffectueLeRedirectToAction()
        {
            dal.AjouterUtilisateur("Nico", "1234");
            dal.AjouterUtilisateur("Jérémie", "1234");
            dal.AjouterVote(idSondage, 1, 1);

            RedirectToRouteResult resultat = (RedirectToRouteResult)controleur.Index(idSondage);

            Assert.AreEqual("AfficheResultat", resultat.RouteValues["action"]);
            Assert.AreEqual(idSondage, resultat.RouteValues["id"]);
        }

        [TestMethod]
        public void IndexPost_AvecViewModelInvalide_RenvoiLeBonViewModelEtAfficheLaVue()
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = new List<RestaurantCheckBoxViewModel>
            {
                new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 2, NomEtTelephone = "Resto pinière (0102030405)"},
                new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 3, NomEtTelephone = "Resto toro (0102030405)"},
            }
            };
            controleur.ValideLeModele(viewModel);

            ViewResult view = (ViewResult)controleur.Index(viewModel, idSondage);

            viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(2, viewModel.ListeDesResto.Count);
            Assert.AreEqual(2, viewModel.ListeDesResto[0].Id);
            Assert.IsFalse(viewModel.ListeDesResto[0].EstSelectionne);
            Assert.AreEqual("Resto pinière (0102030405)", viewModel.ListeDesResto[0].NomEtTelephone);
        }

        [TestMethod]
        public void IndexPost_AvecViewModelValideMaisPasDutilisateur_RenvoiUneHttpUnauthorizedResult()
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = new List<RestaurantCheckBoxViewModel>
            {
                new RestaurantCheckBoxViewModel { EstSelectionne = true, Id = 2, NomEtTelephone = "Resto pinière (0102030405)"},
                new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 3, NomEtTelephone = "Resto toro (0102030405)"},
            }
            };
            controleur.ValideLeModele(viewModel);

            HttpUnauthorizedResult view = (HttpUnauthorizedResult)controleur.Index(viewModel, idSondage);

            Assert.AreEqual(401, view.StatusCode);
        }

        [TestMethod]
        public void IndexPost_AvecViewModelValideEtUtilisateur_AppelleBienAjoutVoteEtRenvoiBonneAction()
        {
            Mock<IDal> mock = new Mock<IDal>();
            mock.Setup(m => m.ObtenirUtilisateur("1")).Returns(new Utilisateur { Id = 1, Prenom = "Nico" });

            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(p => p.HttpContext.Request.Browser.Browser).Returns("1");
            controleur = new VoteController(mock.Object);
            controleur.ControllerContext = controllerContext.Object;

            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = new List<RestaurantCheckBoxViewModel>
                {
                    new RestaurantCheckBoxViewModel { EstSelectionne = true, Id = 2, NomEtTelephone = "Resto pinière (0102030405)"},
                    new RestaurantCheckBoxViewModel { EstSelectionne = false, Id = 3, NomEtTelephone = "Resto toro (0102030405)"},
                }
            };
            controleur.ValideLeModele(viewModel);

            RedirectToRouteResult resultat = (RedirectToRouteResult)controleur.Index(viewModel, idSondage);

            mock.Verify(m => m.AjouterVote(idSondage, 2, 1));
            Assert.AreEqual("AfficheResultat", resultat.RouteValues["action"]);
            Assert.AreEqual(idSondage, resultat.RouteValues["id"]);
        }

        [TestMethod]
        public void AfficheResultat_SansAvoirVote_RenvoiVersIndex()
        {
            RedirectToRouteResult resultat = (RedirectToRouteResult)controleur.AfficheResultat(idSondage);

            Assert.AreEqual("Index", resultat.RouteValues["action"]);
            Assert.AreEqual(idSondage, resultat.RouteValues["id"]);
        }

        [TestMethod]
        public void AfficheResultat_AvecVote_RenvoiLesResultats()
        {
            dal.AjouterUtilisateur("Nico", "1234");
            dal.AjouterUtilisateur("Jérémie", "1234");
            dal.AjouterVote(idSondage, 1, 1);

            ViewResult view = (ViewResult)controleur.AfficheResultat(idSondage);

            List<Resultats> model = (List<Resultats>)view.Model;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Resto pinambour", model[0].Nom);
            Assert.AreEqual(1, model[0].NombreDeVotes);
            Assert.AreEqual("0102030405", model[0].Telephone);
        }

        [TestCleanup]
        public void Clean()
        {
            dal.Dispose();
        }
    }
}
