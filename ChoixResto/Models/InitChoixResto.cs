using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

            base.Seed(context);
        }
    }
}