using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoixResto.Models;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            using (IDal dal = new Dal())
            {
                List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
                return View(listeDesRestaurants);
            }
        }

        // On peut grâce à ActionName appeler sa méthode comme on le veut
        //
        //[ActionName("Index")]
        //public ActionResult LeNomQueJeVeux()
        //{
        //    return View();
        //}

        // On peut récupérer les valeurs envoyer avec l'objet Request
        //
        //public ActionResult ModifierRestaurant()
        //{
        //    string id = Request.Url.AbsolutePath.Split('/').Last();
        //    ViewBag.Id = id;
        //    return View();
        //}

        // Même chose mais permet de récupérer le paramètre id (on peut donc récupérer plusieurs paramètre via cette méthode (Ex :  l’URL /Restaurant/ModifierRestaurant?id=1&val=3, id et val)
        //
        //public ActionResult ModifierRestaurant()
        //{
        //    string idStr = Request.QueryString["id"];
        //    int id;
        //    if (int.TryParse(idStr, out id))
        //    {
        //        ViewBag.Id = id;
        //        return View();
        //    }
        //    else
        //        return View("Error");
        //}

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                using (IDal dal = new Dal())
                {
                    Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                    if (restaurant == null)
                        return View("Error");
                    return View(restaurant);
                }
            }
            else
                return View("Error");
        }

        // On au type de méthode via la propriété HttpMethod de l'objet Request
        // On accède au élément du formulaire grâce a la propriété Form
        //
        //public ActionResult ModifierRestaurant(int? id)
        //{
        //    if (id.HasValue)
        //    {
        //        using (IDal dal = new Dal())
        //        {
        //            if (Request.HttpMethod == "POST")
        //            {                        
        //                string nouveauNom = Request.Form["Nom"];
        //                string nouveauTelephone = Request.Form["Telephone"];
        //                dal.ModifierRestaurant(id.Value, nouveauNom, nouveauTelephone);
        //            }
        //            Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
        //            if (restaurant == null)
        //                return View("Error");
        //            return View(restaurant);
        //        }
        //    }
        //    else
        //        return View("Error");
        //}

        // Même chose mais on récupère les valeurs via la signature
        //
        //public ActionResult ModifierRestaurant(int? id, string nom, string telephone)
        //{
        //    if (id.HasValue)
        //    {
        //        using (IDal dal = new Dal())
        //        {
        //            if (Request.HttpMethod == "POST")
        //            {
        //                dal.ModifierRestaurant(id.Value, nom, telephone);
        //            }

        //            Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
        //            if (restaurant == null)
        //                return View("Error");
        //            return View(restaurant);
        //        }
        //    }
        //    else
        //        return View("Error");
        //}

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            using (IDal dal = new Dal())
            {
                dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
                return RedirectToAction("Index");
            }
        }
    }
}