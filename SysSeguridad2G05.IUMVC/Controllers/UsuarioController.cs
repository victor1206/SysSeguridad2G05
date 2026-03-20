using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SysSeguridad2G05.EntidadesNegocio;
using SysSeguridad2G05.LogicaNegocios;

namespace SysSeguridad2G05.IUMVC.Controllers
{
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
    }
}
