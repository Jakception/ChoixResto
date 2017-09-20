using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());

            // On y découvre que le filtre HandleErrorAttribute dont nous avons parlé un peu plus haut
            // est défini globalement dans notre site.Pour rappel, ce filtre permet d’afficher la vue Error 
            // (définie dans le répertoire Shared) si une exception est levée dans une méthode. En fait, pour 
            // être un peu plus précis, c’est le cas uniquement s’il y a une configuration particulière 
            // dans le web.config, à savoir :
            // <customErrors mode = "On" defaultRedirect = "Error" />
            // que l’on positionne dans la section<system.web>.
            // Ceci permet d’éviter que vos utilisateurs finaux voient toute la pile d’appel de votre code
            // si jamais il y a une erreur, ce qui ne fait pas très classe et surtout qui peut renseigner 
            // vos utilisateurs sur le contenu de votre code et pourquoi pas sur des éventuelles failles…

            //Si vous avez créé le modèle d'application empty, alors il vous faudra rajouter la ligne suivante 
            //dans le global.asax, afin de dire à ASP.NET MVC de gérer les filtres.
        }
    }
}