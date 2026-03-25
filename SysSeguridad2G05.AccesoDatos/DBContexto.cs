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
            optionsBuilder.UseSqlServer(@"workstation id=dbSysSeguridad2G02.mssql.somee.com;packet size=4096;user id=victor2026_SQLLogin_1;pwd=9e5wwswjx5;data source=dbSysSeguridad2G02.mssql.somee.com;persist security info=False;initial catalog=dbSysSeguridad2G02;TrustServerCertificate=True");
            //optionsBuilder.UseSqlServer(@"Data Source=VictorDuran;Initial Catalog=dbSysSeguridadG05;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
            //optionsBuilder.UseSqlServer(@"Data Source=VictorDuran;Initial Catalog=dbSysSeguridadG05;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }
    }
}
