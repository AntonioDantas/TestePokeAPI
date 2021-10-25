using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TestePokeAPI.Models
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MestreDbContext : DbContext
    {
        public MestreDbContext() : base("name=MestreDbContext")
        { }
        public virtual DbSet<Mestre> Mestres { get; set; }
        public virtual DbSet<Captura> Capturas { get; set; }
    }
}