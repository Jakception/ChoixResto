using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoixResto.Models;

namespace ChoixResto.Controllers
{
    public class RestaurantTestController : Controller
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
            // Ou retourner un code 404 avec
            //
            // return HttpNotFound();
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
            // Vérification par rapport au résultat
            //
            //if (string.IsNullOrWhiteSpace(resto.Nom))
            //{
            //    ViewBag.MessageErreur = "Le nom du restaurant doit être rempli";
            //    return View(resto);
            //}

            // On regarde la propriété IsVAlid de l'objet ModelState qui contient l'état du modlèle et la validation des données associée.
            //if (!ModelState.IsValid)
            //{
            //    //ViewBag.MessageErreur = ModelState["Nom"].Errors[0].ErrorMessage;
            //    return View(resto);
            //}
            using (IDal dal = new Dal())
            {
                dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            using (IDal dal = new Dal())
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
        }

        // Nous avons à notre disposition les méthodes Redirect  et RedirectPermanent  qui renvoient respectivement le code 302 et le code 301.
        // Redirection vers un autre site ou une autre page
        //
        public ActionResult AfficheOpenClassRooms(string id)
        {
            return Redirect("http://fr.openclassrooms.com/");
        }

        // Redirection via une route
        //
        public ActionResult RetourAccueil(string id)
        {
            return RedirectToRoute(new { controller = "Accueil", action = "index" });
        }

        // Retourne une chaîne (mais on peut utiliser ActionResult comme type de retour)
        //
        public ActionResult AfficheChaine()
        {
            return Content("Pas de HTML, juste une chaine");
        }

        // Retourne un fichier Json
        //
        public ActionResult AfficheJson()
        {
            Resto resto = new Resto { Id = 1, Nom = "Resto pinambour" };
            return Json(resto, JsonRequestBehavior.AllowGet);
            // {"Id":1,"Nom":"Resto pinambour","Telephone":null}
        }

        // Téléchargement par l'utilisateur d'un fichier contenu dans le dossier App_Data
        //
        public ActionResult ObtientFichier()
        {
            string fichier = Server.MapPath("~/App_Data/MonFichier.txt");
            return File(fichier, "application/octet-stream", "MonFichier.txt");
        }

        // Renvoi une page vide
        //
        //public ActionResult RenvoiDuVide()
        //{
        //    return EmptyResult();
        //}

        // Renvoyer un code HTTP d’erreur 401, indiquant que l’accès à la ressource est non autorisé, grâce à la classe HttpUnauthorizedResult
        //
        public ActionResult AccesAuthentifie()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return View();
            return new HttpUnauthorizedResult();
        }

        // Renvoyer n’importe quel code HTTP avec HttpStatusCodeResult
        //
        public ActionResult AccesAuthentifie2()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return View();
            return new HttpStatusCodeResult(401);
        }
    }
}