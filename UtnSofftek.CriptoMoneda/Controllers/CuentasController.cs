using Microsoft.AspNetCore.Mvc;
using UtnSofftek.CriptoMoneda.Datos;
using UtnSofftek.CriptoMoneda.Models;

namespace UtnSofftek.CriptoMoneda.Controllers
{
    public class CuentasController : Controller
    {
        CuentaDatos cuentaDatos = new CuentaDatos();
        public IActionResult Lista()
        {
            List<FiduciariaModel>? listaFiduciaria = new List<FiduciariaModel>();
            List<CriptoModel>? listaCripto = new List<CriptoModel>();
            listaFiduciaria = cuentaDatos.ListarFiduciaria(1);
            listaCripto = cuentaDatos.ListarCripto(1);
            ListaCuentaModel listaCuentas = new(listaFiduciaria, listaCripto);
            return View(listaCuentas);
        }

        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(CuentasModel cuentas)

        {
            if (cuentas.Tipo != null)
            {
                if (cuentas.Tipo == "ARG" || cuentas.Tipo == "USD")
                {
                    string cbu = "05480000" + (string)DateTime.Now.ToString("yyyyMMddHHmmss");
                    FiduciariaModel fiduciariaModel = new FiduciariaModel();
                    fiduciariaModel.Saldo = 0;
                    fiduciariaModel.Tipo = cuentas.Tipo;
                    fiduciariaModel.IdCliente = 1;
                    fiduciariaModel.CBU = cbu;
                    fiduciariaModel.Alias = GenerarAlias();
                    fiduciariaModel.NroCuenta = Convert.ToInt32(DateTime.Now.ToString("ddHHmmss"));

                    var respuesta = cuentaDatos.CrearCuentaFiduciaria(fiduciariaModel);
                    if (respuesta)
                    {
                        return RedirectToAction("Lista");
                    }
                }
                else
                {
                    CriptoModel criptoModel = new CriptoModel();
                    criptoModel.Saldo = 0;
                    criptoModel.Tipo = cuentas.Tipo;
                    criptoModel.IdCliente = 1;
                    criptoModel.Uuid = (string)DateTime.Now.ToString("yyyyMMddHHmmss");

                    var respuesta = cuentaDatos.CrearCuentaCripto(criptoModel);
                    if (respuesta)
                    {
                        return RedirectToAction("Lista");
                    }
                }
            }
            return View();
        }

        public IActionResult DatosCuentas(int id)
        {
            Cuenta cuenta = cuentaDatos.ObtenerCuenta(id);
            CuentasModel cuentas = new CuentasModel();
            if (cuenta.Tipo == "ARG" || cuenta.Tipo == "USD")
            {
                cuentas.Fiduciaria = (FiduciariaModel)cuenta;
            }
            else
            {
                cuentas.Cripto = (CriptoModel)cuenta;

            }
            return View(cuentas);
        }

        public IActionResult Depositar(int id)
        {
            CuentasModel cuentas = new CuentasModel();
            cuentas.Fiduciaria = (FiduciariaModel)cuentaDatos.ObtenerCuenta(id);
            return View(cuentas);
        }

        [HttpPost]
        public IActionResult Depositar(FiduciariaModel fiduciaria)
        {
            FiduciariaModel fiduModel = (FiduciariaModel)cuentaDatos.ObtenerCuenta(fiduciaria.IdCuenta);
            decimal newSaldo = fiduModel.Saldo;
            if (fiduciaria.Saldo > 0)
            {
                decimal conversion = 0;
                if (fiduciaria.Tipo == "ARG")
                {
                    newSaldo += fiduciaria.Saldo;
                    conversion = fiduciaria.Saldo;
                }
                if (fiduciaria.Tipo == "USD")
                {
                    newSaldo += (fiduciaria.Saldo / (decimal)208.88d);
                    conversion = (fiduciaria.Saldo / (decimal)208.88d);
                }
                var respuesta = cuentaDatos.ActualizarSaldo(newSaldo, fiduciaria.IdCuenta);
                if (respuesta)
                {
                    HistorialModel historial = new HistorialModel();
                    historial.Fecha = DateTime.Now;
                    historial.IdCuenta = fiduciaria.IdCuenta;
                    historial.Monto = conversion;
                    historial.Tipo = "Deposito";
                    cuentaDatos.CrearHistorial(historial);
                    return RedirectToAction("Lista");
                }
            }
            return View();
        }

        public IActionResult Extraer(int id)
        {
            CuentasModel cuentas = new CuentasModel();
            cuentas.Fiduciaria = (FiduciariaModel)cuentaDatos.ObtenerCuenta(id);
            return View(cuentas);
        }
        [HttpPost]
        public IActionResult Extraer(FiduciariaModel fiduciaria)
        {
            FiduciariaModel fiduModel = (FiduciariaModel)cuentaDatos.ObtenerCuenta(fiduciaria.IdCuenta);
            decimal newSaldo = fiduModel.Saldo;
            if (fiduciaria.Saldo <= fiduModel.Saldo)
            {
                decimal conversion = 0;
                if (fiduciaria.Tipo == "ARG")
                {
                    conversion = fiduciaria.Saldo;
                    newSaldo -= conversion;
                }
                if (fiduciaria.Tipo == "USD")
                {
                    conversion = (fiduciaria.Saldo / (decimal)208.88d);
                    newSaldo -= conversion;
                }
                var respuesta = cuentaDatos.ActualizarSaldo(newSaldo, fiduciaria.IdCuenta);
                if (respuesta)
                {
                    HistorialModel historial = new HistorialModel();
                    historial.Fecha = DateTime.Now;
                    historial.IdCuenta = fiduciaria.IdCuenta;
                    historial.Monto = conversion;
                    historial.Tipo = "Extraccion";
                    cuentaDatos.CrearHistorial(historial);
                    return RedirectToAction("Lista");
                }
            }
            return View();
        }

        public IActionResult TransferirFiducaria(int id)
        {
            FiduciariaModel cuentaOrigen = (FiduciariaModel)cuentaDatos.ObtenerCuenta(id);
            TransferenciaModel transfeModel = new TransferenciaModel();
            transfeModel.ListaFiduciarias = cuentaDatos.ListarFiduciaria(1);
            transfeModel.ListaCripto = cuentaDatos.ListarCripto(1);
            transfeModel.FiduOrigen = cuentaOrigen;

            return View(transfeModel);
        }

        [HttpPost]
        public IActionResult TransferirFiducaria(TransferenciaModel transfeModel)
        {
            //VARIABLES
            var cuentaOrigen = cuentaDatos.ObtenerCuenta(transfeModel.FiduOrigen.IdCuenta);
            string[] datos = transfeModel.IdCuentaDestino.Split(',');
            int idCuentaDestino = Convert.ToInt32(datos[0]);
            string tipoOrigen = cuentaOrigen.Tipo;
            string tipoDestino = datos[1];
            var cuentaDestino = cuentaDatos.ObtenerCuenta(idCuentaDestino);
            decimal conversion = 0;
            decimal saldoOrigen = cuentaOrigen.Saldo;
            decimal saldoDestino = cuentaDestino.Saldo;

            if (saldoOrigen >= transfeModel.Saldo && transfeModel.Saldo != 0)
            {
                switch (tipoOrigen)
                {
                    case "ARG":
                        if (tipoDestino == "ARG")
                        {
                            conversion = transfeModel.Saldo;
                            saldoOrigen -= conversion;
                            saldoDestino += conversion;
                        }
                        if (tipoDestino == "USD")
                        {
                            conversion = (transfeModel.Saldo * (decimal)0.004787d); //(207) 0,990909
                            saldoOrigen -= transfeModel.Saldo;
                            saldoDestino += conversion;
                        }
                        if (tipoDestino == "BTC")
                        {
                            conversion = transfeModel.Saldo * (decimal)0.004787d; //(207) 0,990909
                            conversion = (conversion * (decimal)0.000032d); //0,000031
                            saldoOrigen -= transfeModel.Saldo;
                            saldoDestino += conversion;
                        }
                        break;
                    case "USD":
                        if (tipoDestino == "ARG")
                        {
                            conversion = (transfeModel.Saldo * (decimal)208.88d); // (1) 208,88
                            saldoOrigen -= transfeModel.Saldo;
                            saldoDestino += conversion;
                        }
                        if (tipoDestino == "USD")
                        {
                            conversion = transfeModel.Saldo;
                            saldoOrigen -= transfeModel.Saldo;
                            saldoDestino += conversion;
                        }
                        if (tipoDestino == "BTC")
                        {
                            conversion = (transfeModel.Saldo * (decimal)0.000032d); // (5) 0.00016
                            saldoOrigen -= transfeModel.Saldo;
                            saldoDestino += conversion;
                        }
                        break;

                }
                //Actualizar Saldo en Origen
                var respuesta = cuentaDatos.ActualizarSaldo(saldoOrigen, cuentaOrigen.IdCuenta);
                HistorialModel historial = new HistorialModel();
                if (respuesta)
                {
                    historial.Fecha = DateTime.Now;
                    historial.IdCuenta = cuentaOrigen.IdCuenta;
                    historial.Monto = transfeModel.Saldo;
                    historial.Tipo = "Transf. Realizada";
                    cuentaDatos.CrearHistorial(historial);
                }
                respuesta = cuentaDatos.ActualizarSaldo(saldoDestino, idCuentaDestino);
                if (respuesta)
                {
                    historial.Fecha = DateTime.Now;
                    historial.IdCuenta = idCuentaDestino;
                    historial.Monto = conversion;
                    historial.Tipo = "Transf. Recibida";
                    cuentaDatos.CrearHistorial(historial);
                    return RedirectToAction("Lista");
                }
            }
            return View(transfeModel);
        }

        public IActionResult TransferirCripto(int id)
        {
            CriptoModel cuentaOrigen = (CriptoModel)cuentaDatos.ObtenerCuenta(id);
            TransferenciaModel transfeModel = new TransferenciaModel();
            transfeModel.ListaFiduciarias = cuentaDatos.ListarFiduciaria(1);
            transfeModel.ListaCripto = cuentaDatos.ListarCripto(1);
            transfeModel.CriptoOrigen = cuentaOrigen;

            return View(transfeModel);
        }

        [HttpPost]
        public IActionResult TransferirCripto(TransferenciaModel transfeModel)
        {
            var cuentaOrigen = cuentaDatos.ObtenerCuenta(transfeModel.CriptoOrigen.IdCuenta);
            string[] datos = transfeModel.IdCuentaDestino.Split(',');
            int idCuentaDestino = Convert.ToInt32(datos[0]);
            string tipoOrigen = cuentaOrigen.Tipo;
            string tipoDestino = datos[1];
            var cuentaDestino = cuentaDatos.ObtenerCuenta(idCuentaDestino);
            decimal conversion = 0;
            decimal saldoOrigen = cuentaOrigen.Saldo;
            decimal saldoDestino = cuentaDestino.Saldo;
            if (saldoOrigen >= transfeModel.Saldo && transfeModel.Saldo != 0)
            {
                if (tipoDestino == "ARG")
                {
                    conversion = transfeModel.Saldo * (decimal)31199.60d;
                    conversion = (conversion * (decimal)208.88d); // (1) 208,88
                    saldoOrigen -= transfeModel.Saldo;
                    saldoDestino += conversion;
                }
                if (tipoDestino == "USD")
                {
                    conversion = transfeModel.Saldo * (decimal)31199.60d;
                    saldoOrigen -= transfeModel.Saldo;
                    saldoDestino += conversion;
                }
                if (tipoDestino == "BTC")
                {
                    conversion = transfeModel.Saldo;
                    saldoOrigen -= transfeModel.Saldo;
                    saldoDestino += conversion;
                }
                //Actualizar Saldo en Origen
                var respuesta = cuentaDatos.ActualizarSaldo(saldoOrigen, cuentaOrigen.IdCuenta);
                HistorialModel historial = new HistorialModel();
                if (respuesta)
                {
                    historial.Fecha = DateTime.Now;
                    historial.IdCuenta = cuentaOrigen.IdCuenta;
                    historial.Monto = transfeModel.Saldo;
                    historial.Tipo = "Transf. Realizada";
                    cuentaDatos.CrearHistorial(historial);
                }
                respuesta = cuentaDatos.ActualizarSaldo(saldoDestino, idCuentaDestino);
                if (respuesta)
                {
                    historial.Fecha = DateTime.Now;
                    historial.IdCuenta = idCuentaDestino;
                    historial.Monto = conversion;
                    historial.Tipo = "Transf. Recibida";
                    cuentaDatos.CrearHistorial(historial);
                    return RedirectToAction("Lista");
                }
            }
                

            return View();
        }

        //Metodos
        public List<string> ListaAlias()
        {
            List<string> lista = new List<string>();

            lista.Add("CONEJO");
            lista.Add("CEREZA");
            lista.Add("ACTORES");
            lista.Add("CABALLO");
            lista.Add("LIBERAL");
            lista.Add("ACEITAR");
            lista.Add("BESO");
            lista.Add("DUREZA");
            lista.Add("BATALLA");
            return lista;
        }

        public string GenerarAlias()
        {
            var lista = ListaAlias();
            Random rnd = new Random();

            string alias = lista[rnd.Next(0, 8)] + "." + lista[rnd.Next(0, 8)];
            return alias;
        }
    }
}
