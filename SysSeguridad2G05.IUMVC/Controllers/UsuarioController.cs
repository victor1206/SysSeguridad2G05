using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SysSeguridad2G05.EntidadesNegocio;
using SysSeguridad2G05.LogicaNegocios;
using System.Security.Claims;

namespace SysSeguridad2G05.IUMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsuarioController : Controller
    {
        UsuarioBL usuarioBl = new UsuarioBL();
        RolBL rolBl = new RolBL();
        // GET: UsuarioController
        public async Task<IActionResult> Index(Usuario pUsuario = null)
        {
            if (pUsuario == null)
                pUsuario = new Usuario();
            if (pUsuario.Top_Aux == 0)
                pUsuario.Top_Aux = 10;
            else
                if (pUsuario.Top_Aux == -1)
                    pUsuario.Top_Aux = 0;

            var taskBuscar = usuarioBl.BuscarIncluirRolAsync(pUsuario);
            var taskObtenerTodosRoles = rolBl.ObtenerTodosAsync();
            var usuarios = await taskBuscar;
            ViewBag.Top = pUsuario.Top_Aux;
            ViewBag.Roles = await taskObtenerTodosRoles;

            return View(usuarios);
        }

        // GET: UsuarioController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await usuarioBl.BuscarIncluirRolAsync(new Usuario { Id = id});
            return View(usuario.FirstOrDefault());
        }

        // GET: UsuarioController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await rolBl.ObtenerTodosAsync();
            ViewBag.Error = "";
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario pUsuario)
        {
            try
            {
                int result = await usuarioBl.CrearAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await rolBl.ObtenerTodosAsync();
                return View(pUsuario);
            }
        }

        // GET: UsuarioController/Edit/5
        public async Task<IActionResult> Edit(Usuario pUsuario)
        {
            var usuario = await usuarioBl.ObtenerPorIdAsync(pUsuario);
            ViewBag.Roles = await rolBl.ObtenerTodosAsync();
            ViewBag.Error = "";
            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario pUsuario)
        {
            try
            {
                int result = await usuarioBl.ModificarAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await rolBl.ObtenerTodosAsync();
                return View(pUsuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<IActionResult> Delete(Usuario pUsuario)
        {
            var usuario = await usuarioBl.BuscarIncluirRolAsync(pUsuario);
            ViewBag.Error = "";
            return View(usuario.FirstOrDefault());
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Usuario pUsuario)
        {
            try
            {
                int result = await usuarioBl.EliminarAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                var usuario = await usuarioBl.BuscarIncluirRolAsync(pUsuario);
                return View(usuario.FirstOrDefault());
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string ReturnUrl = null)
        { 
            ViewBag.Url = ReturnUrl;
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Usuario pUsuario, string pReturnUrl = null)
        {
            try
            {
                var usuario = await usuarioBl.LoginAsync(pUsuario);
                if (usuario != null && usuario.Id > 0 && pUsuario.Login == usuario.Login)
                {
                    usuario.Rol = await rolBl.ObtenerPorIdAsync(new Rol { Id = usuario.IdRol });
                    var claims = new[] { new Claim(ClaimTypes.Name, usuario.Login),
                        new Claim(ClaimTypes.Role, usuario.Rol.Nombre)};
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity));
                }
                else
                    throw new Exception("Credenciales Incorrectas");

                if (!string.IsNullOrWhiteSpace(pReturnUrl))
                    return Redirect(pReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) 
            {
                ViewBag.Error = ex.Message;
                ViewBag.Url = pReturnUrl;
                return View(new Usuario { Login = pUsuario.Login});
            }
        }

        public async Task<IActionResult> CerrarSesion(string pReturnUrl)
        { 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }
    }
}
