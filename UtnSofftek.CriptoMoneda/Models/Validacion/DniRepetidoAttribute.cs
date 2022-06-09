using System.ComponentModel.DataAnnotations;
using UtnSofftek.CriptoMoneda.Datos;

namespace UtnSofftek.CriptoMoneda.Models.Validacion
{
    public class DniRepetidoAttribute : ValidationAttribute
    {
        private ClienteDatos _clienteDatos = new ClienteDatos();

        public DniRepetidoAttribute() : base("El DNI ya existe")
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {
                ClienteModel cliente = null;
                cliente = _clienteDatos.ObtenerByDni((string)value);
                if (cliente != null)
                {
                    var mensajeDeError = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(mensajeDeError);
                }
            }
            return ValidationResult.Success;
        }
    }
}

