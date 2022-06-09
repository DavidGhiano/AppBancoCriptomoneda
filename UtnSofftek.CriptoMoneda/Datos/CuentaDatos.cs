using System.Data;
using System.Data.SqlClient;
using UtnSofftek.CriptoMoneda.Models;

namespace UtnSofftek.CriptoMoneda.Datos
{
    public class CuentaDatos
    {
        public List<FiduciariaModel>? ListarFiduciaria(int id)
        {
            //C.IdCuenta, C.Saldo, C.Tipo, C.IdCliente, F.Cbu, F.Alias, F.NroCuenta
            List<FiduciariaModel> oLista = new List<FiduciariaModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("CuentaFiduciariaListar", conexion);
                cmd.Parameters.AddWithValue("IdCliente", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var datoReader = cmd.ExecuteReader())
                {
                    while (datoReader.Read())
                        oLista.Add(new FiduciariaModel()
                        {
                            IdCuenta = Convert.ToInt32(datoReader["IdCuenta"]),
                            Saldo = Convert.ToDecimal(datoReader["Saldo"]),
                            Tipo = datoReader["Tipo"].ToString(),
                            IdCliente = Convert.ToInt32(datoReader["IdCliente"]),
                            CBU = datoReader["Cbu"].ToString(),
                            Alias = datoReader["Alias"].ToString(),
                            NroCuenta = Convert.ToInt32(datoReader["NroCuenta"])
                        });
                }
            }
            return oLista;
        }
        public List<CriptoModel>? ListarCripto(int id)
        {
            //Cu.IdCuenta, Cu.Saldo, Cu.Tipo, Cu.IdCliente, Cr.Uuid
            List<CriptoModel> oLista = new List<CriptoModel>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("CuentaCriptoListar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var datoReader = cmd.ExecuteReader())
                {
                    while (datoReader.Read())
                        oLista.Add(new CriptoModel()
                        {
                            IdCuenta = Convert.ToInt32(datoReader["IdCuenta"]),
                            Saldo = Convert.ToDecimal(datoReader["Saldo"]),
                            Tipo = datoReader["Tipo"].ToString(),
                            IdCliente = Convert.ToInt32(datoReader["IdCliente"]),
                            Uuid = datoReader["Uuid"].ToString()
                        });
                }
            }
            return oLista;
        }

        public bool CrearCuentaFiduciaria(FiduciariaModel fiduciaria)
        {
            bool respuesta;
            int idCuenta = 0;

            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("CuentaCrear", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Tipo", fiduciaria.Tipo);
                    cmd.Parameters.AddWithValue("IdCliente", fiduciaria.IdCliente);
                    idCuenta = (int)cmd.ExecuteScalar();
                }
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("CuentaFiduciariaCrear", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdCuenta", idCuenta);
                    cmd.Parameters.AddWithValue("Cbu", fiduciaria.CBU);
                    cmd.Parameters.AddWithValue("Alias", fiduciaria.Alias);
                    cmd.Parameters.AddWithValue("NroCuenta", fiduciaria.NroCuenta);
                    cmd.ExecuteNonQuery();
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public bool CrearCuentaCripto(CriptoModel cripto)
        {
            bool respuesta;
            int idCuenta = 0;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("CuentaCrear", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Tipo", cripto.Tipo);
                    cmd.Parameters.AddWithValue("IdCliente", cripto.IdCliente);
                    idCuenta = (int)cmd.ExecuteScalar();
                }
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("CuentaCriptoCrear", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdCuenta", idCuenta);
                    cmd.Parameters.AddWithValue("Uuid", cripto.Uuid);
                    cmd.ExecuteNonQuery();
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public Cuenta ObtenerCuenta(int id)
        {
            Cuenta cuenta = null;
            string tipo = "";
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerCuenta", conexion);
                cmd.Parameters.AddWithValue("IdCuenta", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tipo = dr["Tipo"].ToString();
                    }
                }
            }
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                if (tipo == "ARG" || tipo == "USD")
                {
                    SqlCommand cmd = new SqlCommand("CuentaFiduciariaObtener", conexion);
                    cmd.Parameters.AddWithValue("IdCuenta", id);
                    var fiduciaria = new FiduciariaModel();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            fiduciaria.IdCuenta = Convert.ToInt32(dr["IdCuenta"]);
                            fiduciaria.Saldo = Convert.ToDecimal(dr["Saldo"]);
                            fiduciaria.Tipo = dr["Tipo"].ToString();
                            fiduciaria.IdCliente = Convert.ToInt32(dr["IdCLiente"]);
                            fiduciaria.CBU = dr["Cbu"].ToString();
                            fiduciaria.Alias = dr["Alias"].ToString();
                            fiduciaria.NroCuenta = Convert.ToInt32(dr["NroCuenta"]);
                        }
                        cuenta = fiduciaria;
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("CuentaCriptoObtener", conexion);
                    cmd.Parameters.AddWithValue("IdCuenta", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var cripto = new CriptoModel();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cripto.IdCuenta = Convert.ToInt32(dr["IdCuenta"]);
                            cripto.Saldo = Convert.ToDecimal(dr["Saldo"]);
                            cripto.Tipo = dr["Tipo"].ToString();
                            cripto.IdCliente = Convert.ToInt32(dr["IdCLiente"]);
                            cripto.Uuid = dr["Uuid"].ToString();
                        }
                        cuenta = cripto;
                    }
                }
            }
            return cuenta;
        }

        public bool ActualizarSaldo(decimal saldo, int idCuenta)
        {
            bool respuesta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("CuentaActualizar", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Saldo", saldo);
                    cmd.Parameters.AddWithValue("IdCuenta", idCuenta);
                    cmd.ExecuteNonQuery();
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }



        /*HISTORIAL*/
        public bool CrearHistorial(HistorialModel historial)
        {
            bool respuesta;
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("HistorialCrear", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Fecha", historial.Fecha);
                    cmd.Parameters.AddWithValue("IdCuenta", historial.IdCuenta);
                    cmd.Parameters.AddWithValue("Monto", historial.Monto);
                    cmd.Parameters.AddWithValue("Tipo", historial.Tipo);
                    cmd.ExecuteNonQuery();
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public List<HistorialModel> ListarHistorial(int id)
        {
            var lista = new List<HistorialModel>();
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("HistorialListar", conexion);
                cmd.Parameters.AddWithValue("IdCuenta", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(new HistorialModel()
                        {
                            IdHistorial = Convert.ToInt32(dr["IdHistorial"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            IdCuenta = Convert.ToInt32(dr["IdCuenta"]),
                            Monto = Convert.ToDecimal(dr["Monto"]),
                            Tipo = dr["Tipo"].ToString()
                        });
                }
            }
            return lista;
        }
    }
}
