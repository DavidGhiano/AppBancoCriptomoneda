namespace UtnSofftek.CriptoMoneda.Models
{
    public class TransferenciaModel
    {
        public List<FiduciariaModel>? ListaFiduciarias { get; set; }
        public List<CriptoModel>? ListaCripto { get; set; }

        public FiduciariaModel? FiduOrigen { get; set; }
        public FiduciariaModel? FiduDestino { get; set; }

        public CriptoModel? CriptoOrigen { get; set; }
        public CriptoModel? CriptoDestino { get; set; }

        public string? IdCuentaDestino { get; set; }
        public decimal Saldo { get; set; }
    }
}
