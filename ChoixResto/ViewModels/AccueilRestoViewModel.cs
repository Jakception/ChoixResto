using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixResto.ViewModels
{
    public class AccueilRestoViewModel
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public List<Models.Resto> ListeDesRestos { get; set; }
    }
}