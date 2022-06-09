namespace UtnSofftek.CriptoMoneda.Models
{
    public class ListaCuentaModel
    {
        public List<FiduciariaModel>? listaFiduciarias { get; set; }
        public List<CriptoModel>? listaCripto { get; set; }

        public ListaCuentaModel() { }
        public ListaCuentaModel(List<FiduciariaModel> listaFiduciarias, List<CriptoModel> listaCripto)
        {
            this.listaFiduciarias = listaFiduciarias;
            this.listaCripto = listaCripto;
        }
    }
}
