using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Web.Mvc;
using ChoixResto.Controllers;

namespace ChoixResto.Tests
{
    [TestClass]
    public class AccueilControllerTests
    {
        [TestMethod]
        public void AccueilController_Index_RenvoiVueParDefaut()
        {
            AccueilTestController controller = new AccueilTestController();

            ViewResult resultat = (ViewResult)controller.Index();

            Assert.AreEqual(string.Empty, resultat.ViewName);
        }

        //[TestMethod]
        //public void AccueilController_AfficheDate_RenvoiVueIndexEtViewData()
        //{
        //    AccueilController controller = new AccueilController();

        //    ViewResult resultat = (ViewResult)controller.AfficheDate("Nicolas");

        //    Assert.AreEqual("Index", resultat.ViewName);
        //    Assert.AreEqual(new DateTime(2012, 4, 28), resultat.ViewData["date"]);
        //    Assert.AreEqual("Bonjour Nicolas !", resultat.ViewBag.Message);
        //}

        [TestMethod]
        public void AccueilController_IndexPost_RenvoiActionVote()
        {
            using (IDal dal = new DalEnDur())
            {
                AccueilController controller = new AccueilController(dal);

                RedirectToRouteResult resultat = (RedirectToRouteResult)controller.IndexPost();

                Assert.AreEqual("Index", resultat.RouteValues["action"]);
                Assert.AreEqual("Vote", resultat.RouteValues["controller"]);
                List<Resultats> resultats = dal.ObtenirLesResultats(1);
                Assert.IsNotNull(resultats);
            }
        }
    }
}
