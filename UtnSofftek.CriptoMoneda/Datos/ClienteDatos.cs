using System.Data.SqlClient;
using System.Data;
using UtnSofftek.CriptoMoneda.Models;

namespace UtnSofftek.CriptoMoneda.Datos
{
    public class ClienteDatos
    {
        public ClienteModel ObtenerCliente(int id)
        {
            ClienteModel cliente = null;
            var cn = new Conexion();

            using( var conexion = new SqlConnection( cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ClienteTraerPorID", conexion);
                cmd.Parameters.AddWithValue("IdCliente", id);
                cmd.CommandType = CommandType.StoredProcedure;
                using( var dr = cmd.ExecuteReader())
                {
                    while ( dr.Read())
                    {
                        cliente = new ClienteModel();
                        cliente.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        cliente.Nombre = dr["Nombre"].ToString();
                        cliente.Apellido = dr["Apellido"].ToString();
                        cliente.Dni = dr["Dni"].ToString();
                    }
                }
            }
            return cliente;
        }

        public ClienteModel ObtenerByDni(string dni)
        {
            ClienteModel cliente = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ClienteTraerPorDni", conexion);
                cmd.Parameters.AddWithValue("Dni", dni);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cliente = new ClienteModel();
                        cliente.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        cliente.Nombre = dr["Nombre"].ToString();
                        cliente.Apellido = dr["Apellido"].ToString();
                        cliente.Dni = dr["Dni"].ToString();
                    }
                }
            }
            return cliente;
        }

        public bool Editar(ClienteModel cliente)
        {
            bool respuesta;
            try
            {
                var cn = new Conexion();
                using(var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("ClienteActualizar", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdCliente", cliente.IdCliente);
                    cmd.Parameters.AddWithValue("Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", cliente.Apellido);
                    cmd.ExecuteNonQuery();
                }
                respuesta = true;
            }
            catch(Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
