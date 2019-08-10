using Destek.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destek.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<DestekUser> DestekUsers { get; set; }
        public DbSet<Brans> Brans { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
