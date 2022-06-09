namespace UtnSofftek.CriptoMoneda.Models.Interface
{
    public interface ICuenta
    {
        public List<HistorialModel> VerMovimiento();

        public bool Deposito(double monto);
        public bool Extraer(double monton);
        public bool Transferir(Cuenta oDestino);
    }
}
