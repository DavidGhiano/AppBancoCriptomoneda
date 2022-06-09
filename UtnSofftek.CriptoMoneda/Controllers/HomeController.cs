using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UtnSofftek.CriptoMoneda.Datos;
using UtnSofftek.CriptoMoneda.Models;

namespace UtnSofftek.CriptoMoneda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CuentaDatos cuentaDatos = new CuentaDatos();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<FiduciariaModel>? listaFiduciaria = new List<FiduciariaModel>();
            List<CriptoModel>? listaCripto = new List<CriptoModel>();
            listaFiduciaria = cuentaDatos.ListarFiduciaria(1); 
            listaCripto = cuentaDatos.ListarCripto(1);
            ListaCuentaModel listaCuentas = new( listaFiduciaria,listaCripto);
            
            return View(listaCuentas);
        }
    }
}