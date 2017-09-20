using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Filters
{
    public class AjaxFilterAttribute : ActionFilterAttribute
    {
        // On peut cumuler les différents filtres sur nos actions ou contrôleurs, 
        //    // mais il faut prendre garde à l’ordre dans lesquels ils sont exécutés. 
        //    // D’abord il y a les filtres d’autorisations, ensuite les filtres d’actions, puis les filtres de résultats et enfin les filtres d’exceptions.

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext.HttpContext.Request.Headers != null && filterContext.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
        //        filterContext.Result = new HttpNotFoundResult();
        //    base.OnActionExecuting(filterContext);   
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.Result = new HttpNotFoundResult();
            base.OnActionExecuting(filterContext);
        }
    }
}