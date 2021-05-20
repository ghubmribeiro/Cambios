namespace Cambios
{
    using Cambios.Servicos;
    using Modelos;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        #region Atributos
        private NetworkService networkService;
        private ApiService apiService;
        #endregion

        public List<Rate> Rates { get; set; } = new List<Rate>();

        public Form1()
        {
            InitializeComponent();
            networkService = new NetworkService();
            apiService = new ApiService();
            LoadRates();
        }

        private async void LoadRates()
        {
            //bool load;

            lbl_resultado.Text = "A actualizar taxas...";
            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                MessageBox.Show(connection.Message);
                return;
            }
            else
            {
                await LoadApiRates();
            }

            cb_origem.DataSource = Rates;
            cb_origem.DisplayMember = "Name";

            //Corrige bug das comboBoxes
            cb_destino.BindingContext = new BindingContext();

            cb_destino.DataSource = Rates;
            cb_destino.DisplayMember = "Name";

            progressBar1.Value = 100;

            lbl_resultado.Text = "Taxas carregadas";
        }

        private async Task LoadApiRates()
        {
            progressBar1.Value = 0;

            var response = await apiService.GetRates("http://cambios.somee.com", "/api/rates");

            Rates = (List<Rate>) response.Result;
        }
    }
}
