using ChoixResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixResto.ViewModels
{
    public class RechercheViewModel
    {
        public string Recherche { get; set; }
        public List<Resto> ListeDesRestos { get; set; }
    }
}