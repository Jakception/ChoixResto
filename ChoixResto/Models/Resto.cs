using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Models
{
    [Table("Restos")]
    public class Resto //: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom du restaurant doit être saisi")]
        [StringLength(80)]
        [Remote("VerifNomResto", "Restaurant", ErrorMessage="Ce nom de restaurant existe déjà")]
        public string Nom { get; set; }
        [Display(Name ="Téléphone")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Le numéro de téléphone est incorrect")]
        [AuMoinsUnDesDeux(Parametre1 = "Telephone", Parametre2 = "Email", ErrorMessage = "Vous devez saisir au moins un moyen de contacter le restaurant")]
        public string Telephone { get; set; }
        [AuMoinsUnDesDeux(Parametre1 = "Telephone", Parametre2 = "Email", ErrorMessage = "Vous devez saisir au moins un moyen de contacter le restaurant")]
        public string Email { get; set; }

        // Validation côté serveur
        //
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrWhiteSpace(Telephone) && string.IsNullOrWhiteSpace(Email))
        //        yield return new ValidationResult("Vous devez saisir au moins un moyen de contacter le restaurant", new[] { "Telephone", "Email"});
        //    // etc.
        //}
    }
}