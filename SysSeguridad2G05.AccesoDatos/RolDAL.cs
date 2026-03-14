using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using SysSeguridad2G05.EntidadesNegocio;

namespace SysSeguridad2G05.AccesoDatos
{
    public class RolDAL
    {
        public static async Task<int> CrearAsync(Rol pRol)
        {
            var result = 0;//entero
            using (var dbContexto = new DBContexto())
            {
                dbContexto.Add(pRol);//Insert into Rol
                result = await dbContexto.SaveChangesAsync();//
            }
            return result;
        }

        public static async Task<int> ModificarAsync(Rol pRol)
        { 
            var result = 0;
            using (var dbContexto = new DBContexto())
            {
                var rol = await dbContexto.Rol.FirstOrDefaultAsync(s => s.Id == 
                pRol.Id);//Select * From Rol Where Id = 1
                rol.Nombre = pRol.Nombre;
                dbContexto.Update(rol);//Update 
                result = await dbContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> EliminarAsync(Rol pRol)
        { 
            int result = 0;
            using (var dbContecto = new DBContexto())
            {
                var rol = await dbContecto.Rol.FirstOrDefaultAsync(a => a.Id == pRol.Id);
                dbContecto.Rol.Remove(rol);//Delete from
                result = await dbContecto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<Rol> ObtenerPorIdAsync(Rol pRol)
        {
            var rol = new Rol();
            using (var dbContexto = new DBContexto())
            { 
                rol = await dbContexto.Rol.FirstOrDefaultAsync(b => b.Id == pRol.Id);
            }
            return rol;
        }

        public static async Task<List<Rol>> ObtenerTodosAsync()
        { 
            var roles = new List<Rol>();
            using (var dbContexto = new DBContexto())
            { 
                roles = await dbContexto.Rol.ToListAsync();
            }
            return roles;
        }

        internal static IQueryable<Rol> QuerySelect(IQueryable<Rol> pQuery
            , Rol pRol)
        {
            if (pRol.Id > 0)
                pQuery = pQuery.Where(s => s.Id == pRol.Id);//Where Id = 1
            if(!string.IsNullOrWhiteSpace(pRol.Nombre))
                pQuery = pQuery.Where(s => s.Nombre.Contains(pRol.Nombre));//Where Nombre like '%A%'
            pQuery = pQuery.OrderByDescending(s => s.Id).AsQueryable();//Order by Id Desc
            if(pRol.Top_Aux > 0)
                pQuery = pQuery.Take(pRol.Top_Aux).AsQueryable();
            return pQuery;
        }

        public static async Task<List<Rol>> BuscarAsync(Rol pRol)
        {
            var roles = new List<Rol>();
            using (var dbContexto = new DBContexto())
            { 
                var select = dbContexto.Rol.AsQueryable();//Select * From
                select = QuerySelect(select, pRol);//Where
                roles = await select.ToListAsync();
            }
            return roles;
        }
    }
}
