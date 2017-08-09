using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class Resultats
    {
        public string Nom { get; set; }
        [Display(Name = "Téléphone")]
        public string Telephone { get; set; }
        [Display(Name = "Nombre de votes")]
        public int NombreDeVotes { get; set; }
    }
}