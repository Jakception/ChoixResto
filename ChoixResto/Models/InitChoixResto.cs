using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChoixResto.Models
{
    public class InitChoixResto : DropCreateDatabaseAlways<BddContext>
    {
        protected override void Seed(BddContext context)
        {
            context.Restos.Add(new Resto { Id = 1, Nom = "Resto pinambour", Telephone = "0102030405" });
            context.Restos.Add(new Resto { Id = 2, Nom = "Resto pinière", Telephone = "0102030405" });
            context.Restos.Add(new Resto { Id = 3, Nom = "Resto toro", Telephone = "0102030405" });

            context.Utilisateurs.Add(new Utilisateur { Id = 1, Prenom = "Test", Age = 18, MotDePasse = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes("ChoixRestoTestASP.NET MVC"))) });

            base.Seed(context);
        }

    }
}