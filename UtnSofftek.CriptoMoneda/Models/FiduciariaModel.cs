using UtnSofftek.CriptoMoneda.Models.Interface;

namespace UtnSofftek.CriptoMoneda.Models
{
    public class FiduciariaModel : Cuenta
    {
        public string CBU { get; set; }
        public string Alias { get; set; }
        public int NroCuenta { get; set; }


    }
}
