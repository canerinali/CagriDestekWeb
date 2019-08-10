using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destek.BLL
{
    public class Baglan
    {
        public Baglan()
        {
            Destek.DAL.DatabaseContext db = new DAL.DatabaseContext();
            db.Database.CreateIfNotExists();
        }
    }
}
