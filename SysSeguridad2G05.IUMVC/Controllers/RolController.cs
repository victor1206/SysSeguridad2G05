using Microsoft.AspNetCore.Mvc;

using SysSeguridad2G05.EntidadesNegocio;
using SysSeguridad2G05.LogicaNegocios;

namespace SysSeguridad2G05.IUMVC.Controllers
{
    public class RolController : Controller
    {
        RolBL rolBl = new RolBL();
        public async Task<IActionResult> Index(Rol pRol = null)
        {
            var roles = new List<Rol>();
            try
            {
                if (pRol == null)
                    pRol = new Rol();
                if (pRol.Top_Aux == 0)
                    pRol.Top_Aux = 10;
                else
                    if (pRol.Top_Aux == -1)
                    pRol.Top_Aux = 0;

                roles = await rolBl.BuscarAsync(pRol);
                ViewBag.Top = pRol.Top_Aux;
            }
            catch (Exception ex)
            {
            }
            return View(roles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rol = await rolBl.ObtenerPorIdAsync(new Rol { Id = id });
            return View(rol);
        }

        public IActionResult Create()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol pRol)
        {
            try
            {
                int result = await rolBl.CrearAsync(pRol);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View(pRol);
            }
        }

        public async Task<IActionResult> Edit(Rol pRol)
        {
            var rol = await rolBl.ObtenerPorIdAsync(pRol);
            ViewBag.Error = "";
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int pId, Rol pRol)
        {
            try
            {
                int result = await rolBl.ModificarAsync(pRol);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View(pRol);
            }
        }

        public async Task<IActionResult> Delete(Rol pRol)
        {
            ViewBag.Error = "";
            var rol = await rolBl.ObtenerPorIdAsync(pRol);
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Rol pRol)
        {
            try
            {
                var result = await rolBl.EliminarAsync(pRol);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex) 
            { 
                ViewBag.Error = ex.ToString();
                return View(pRol);
            }
        }
    }
}
