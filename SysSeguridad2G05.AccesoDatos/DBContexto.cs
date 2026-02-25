using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SysSeguridad2G05.EntidadesNegocio;

namespace SysSeguridad2G05.AccesoDatos
{
    public class DBContexto : DbContext 
    {
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("@Data Source=VictorDuran;Initial Catalog=dbSysSeguridadG05;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;");
        }
    }
}
