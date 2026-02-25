using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SysSeguridad2G05.EntidadesNegocio;
using System.Security.Cryptography;

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
            if (loginUsuarioExiste != null && loginUsuarioExiste.Id >0 && 
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

        #endregion
    }
}
