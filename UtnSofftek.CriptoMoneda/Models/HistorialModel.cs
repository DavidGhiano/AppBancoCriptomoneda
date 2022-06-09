namespace UtnSofftek.CriptoMoneda.Models
{
    public class HistorialModel
    {
        public int IdHistorial { get; set; }
        public DateTime Fecha { get; set; }

        public int IdCuenta { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; }
    }
}
