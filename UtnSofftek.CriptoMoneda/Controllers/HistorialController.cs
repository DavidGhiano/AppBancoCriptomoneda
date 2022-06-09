using Microsoft.AspNetCore.Mvc;
using UtnSofftek.CriptoMoneda.Datos;
using UtnSofftek.CriptoMoneda.Models;

namespace UtnSofftek.CriptoMoneda.Controllers
{
    public class HistorialController : Controller
    {
        CuentaDatos cuentasDatos = new CuentaDatos();
        public IActionResult Listar(int id)
        {
            List<HistorialModel> listaHistoria = cuentasDatos.ListarHistorial(id);
            return View(listaHistoria);
        }
    }
}
