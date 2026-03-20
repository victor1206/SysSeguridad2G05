using Microsoft.EntityFrameworkCore;
using SysSeguridad2G05.EntidadesNegocio;
using System.Security.Cryptography;
using System.Text;

namespace SysSeguridad2G05.AccesoDatos
{
    public class UsuarioDAL
    {
        private static void EncriptarMD5(Usuario PUsuario)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(
                    PUsuario.Password));
                var strEncriptar = "";
                for (int i = 0; i < result.Length; i++)
                    strEncriptar += result[i].ToString("x2").ToLower();
                PUsuario.Password = strEncriptar;
            }
        }

        private static async Task<bool> ExisteLogin(Usuario pUsuario,
            DBContexto dBContexto)
        {
            bool result = false;
            var loginUsuarioExiste = await dBContexto.Usuario.FirstOrDefaultAsync
                (s => s.Login == pUsuario.Login && s.Id != pUsuario.Id);
            if (loginUsuarioExiste != null && loginUsuarioExiste.Id > 0 &&
                loginUsuarioExiste.Login == pUsuario.Login)
                result = true;
            return result;
        }

        #region CRUD

        public static async Task<int> CrearAsync(Usuario pUsuario)
        {
            int result = 0;
            using (var dbContexto = new DBContexto())
            {
                bool existeLogin = await ExisteLogin(pUsuario, dbContexto);
                if (existeLogin == false)
                {
                    pUsuario.FechaRegistro = DateTime.Now;//Fecha y hora del servidor
                    EncriptarMD5(pUsuario);
                    dbContexto.Add(pUsuario);
                    result = await dbContexto.SaveChangesAsync();
                }
                else
                    throw new Exception("Login ya existe");
            }
            return result;
        }

        public static async Task<int> ModificarAsync(Usuario pUsuario)
        {
            int result = 0;
            using (var dbContexto = new DBContexto())
            {
                bool existeLogin = await ExisteLogin(pUsuario, dbContexto);
                if (existeLogin == false)
                {
                    var usuario = await dbContexto.Usuario.FirstOrDefaultAsync(
                        s => s.Id == pUsuario.Id);
                    usuario.IdRol = pUsuario.IdRol;
                    usuario.Nombre = pUsuario.Nombre;
                    usuario.Apellido = pUsuario.Apellido;
                    usuario.Login = pUsuario.Login;
                    usuario.Estatus = pUsuario.Estatus;
                    dbContexto.Update(usuario);
                    result = await dbContexto.SaveChangesAsync();
                }
                else
                    throw new Exception("Login ya existe");
            }
            return result;
        }

        public static async Task<int> EliminarAsync(Usuario pUsuario)
        {
            int result = 0;
            using (var dbContexto = new DBContexto())
            {
                var usuario = await dbContexto.Usuario.FirstOrDefaultAsync(
                    s => s.Id == pUsuario.Id
                    );
                dbContexto.Usuario.Remove(usuario);
                result = await dbContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<Usuario> ObtenerPorIdAsync(Usuario pUsuario)
        {
            var usuario = new Usuario();
            using (var dbConexto = new DBContexto())
            {
                usuario = await dbConexto.Usuario.FirstOrDefaultAsync(
                    s => s.Id == pUsuario.Id
                    );
            }
            return usuario;
        }

        public static async Task<List<Usuario>> ObtenerTodosAsync()
        {
            var usuarios = new List<Usuario>();
            using (var dbContexto = new DBContexto())
            {
                usuarios = await dbContexto.Usuario.ToListAsync();
            }
            return usuarios;
        }

        internal static IQueryable<Usuario> QuerySelect(
            IQueryable<Usuario> pQuery, Usuario pUsuario)
        {
            if (pUsuario.Id > 0)
                pQuery = pQuery.Where(s => s.Id == pUsuario.Id);
            if (pUsuario.IdRol > 0)
                pQuery = pQuery.Where(s => s.IdRol == pUsuario.IdRol);
            if (!string.IsNullOrWhiteSpace(pUsuario.Nombre))
                pQuery = pQuery.Where(s => s.Nombre.Contains(pUsuario.Nombre));
            if (!string.IsNullOrWhiteSpace(pUsuario.Apellido))
                pQuery = pQuery.Where(s => s.Apellido.Contains(pUsuario.Apellido));
            if (!string.IsNullOrWhiteSpace(pUsuario.Login))
                pQuery = pQuery.Where(s => s.Login == pUsuario.Login);
            if (pUsuario.Estatus > 0)
                pQuery = pQuery.Where(s => s.Estatus == pUsuario.Estatus);
            if (pUsuario.FechaRegistro.Year > 1000)
            {
                DateTime fechaInicial = new DateTime(pUsuario.FechaRegistro.Year,
                    pUsuario.FechaRegistro.Month, pUsuario.FechaRegistro.Day
                    , 0, 0, 0);
                DateTime fechaFinal = fechaInicial.AddDays(1).AddMilliseconds(-1);
                pQuery = pQuery.Where(s => s.FechaRegistro >= fechaInicial &&
                s.FechaRegistro <= fechaFinal);
            }

            pQuery = pQuery.OrderByDescending(s => s.Id).AsQueryable();
            if (pUsuario.Top_Aux > 0)
                pQuery = pQuery.Take(pUsuario.Top_Aux).AsQueryable();
            return pQuery;

        }

        public static async Task<List<Usuario>> BuscarAsync(Usuario pUsuario)
        {
            var Usuarios = new List<Usuario>();
            using (var dbContexto = new DBContexto())
            {
                var select = dbContexto.Usuario.AsQueryable();
                select = QuerySelect(select, pUsuario);
                Usuarios = await select.ToListAsync();
            }
            return Usuarios;
        }

        public static async Task<List<Usuario>> BuscarIncluirRolesAsync(Usuario pUsuario)
        {
            var Usuarios = new List<Usuario>();
            using (var dbContexto = new DBContexto())
            {
                var select = dbContexto.Usuario.AsQueryable();
                select = QuerySelect(select, pUsuario)
                    .Include(s => s.Rol).AsQueryable();//Inner Join
                Usuarios = await select.ToListAsync();
            }
            return Usuarios;
        }
        #endregion

        public static async Task<Usuario> LoginAsync(Usuario pUsuario)
        { 
            var usuario = new Usuario();
            using (var dbContexto = new DBContexto())
            {
                EncriptarMD5(pUsuario);
                usuario = await dbContexto.Usuario.FirstOrDefaultAsync(a => 
                a.Login == pUsuario.Login && a.Password == pUsuario.Password && 
                a.Estatus == (byte)Estatus_Usuario.ACTIVO);
            }
            return usuario;
        }
    }
}
