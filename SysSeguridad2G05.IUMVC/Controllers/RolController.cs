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
    }
}
