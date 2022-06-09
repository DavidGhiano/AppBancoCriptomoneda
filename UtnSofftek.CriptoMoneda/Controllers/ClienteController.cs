using Microsoft.AspNetCore.Mvc;
using UtnSofftek.CriptoMoneda.Datos;
using UtnSofftek.CriptoMoneda.Models;

namespace UtnSofftek.CriptoMoneda.Controllers
{
    public class ClienteController : Controller
    {
        ClienteDatos clienteDatos = new ClienteDatos();
        public IActionResult VerDatos()
        {
            ClienteModel cliente = clienteDatos.ObtenerCliente(1);
            return View(cliente);
        }

        public IActionResult Editar(int id)
        {
            ClienteModel cliente = clienteDatos.ObtenerCliente(id);
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(ClienteModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var respuesta = clienteDatos.Editar(cliente);
            if (respuesta)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}
