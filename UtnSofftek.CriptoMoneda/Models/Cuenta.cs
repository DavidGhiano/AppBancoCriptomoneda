using System.ComponentModel.DataAnnotations;

namespace UtnSofftek.CriptoMoneda.Models
{
    public class Cuenta
    {
        public int IdCuenta { get; set; }
        public decimal Saldo { get; set; }
        [Required]  
        public string? Tipo { get; set; }
        public int IdCliente { get; set; }
    }
}
