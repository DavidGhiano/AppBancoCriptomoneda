using System.ComponentModel.DataAnnotations;
using UtnSofftek.CriptoMoneda.Models.Validacion;

namespace UtnSofftek.CriptoMoneda.Models
{
    public class ClienteModel
    {
        public int IdCliente { get; set; }
        [Required(ErrorMessage="El nombre es obligatorio")]
        [StringLength(45, ErrorMessage = "Supera la cantidad de caracteres permitido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage ="El apellido es obligatorio")]
        [StringLength(45, ErrorMessage = "Supera la cantidad de caracteres permitido")]
        public string Apellido { get; set; }
        [StringLength(9, ErrorMessage = "Supera la cantidad de caracteres permitido")]
        [DniRepetido()]
        public string? Dni { get; set; }
    }
}
