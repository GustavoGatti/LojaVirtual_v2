using LojaVirtual_v2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Database
{
    public class LojaVirtualContext: DbContext
    {
        public LojaVirtualContext(DbContextOptions<LojaVirtualContext> options): base(options)
        {

        }

       public DbSet<Cliente> cliente { get; set; }
       public DbSet<NewsletterEmail> NewsletterEmail { get; set; }
       public DbSet<Colaborador> Colaboradores { get; set; }

    }
}
