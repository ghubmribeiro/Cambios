namespace Cambios
{
    using Modelos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadRates();
        }

        private async void LoadRates()
        {
            //bool load;

            progressBar1.Value = 0;

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://cambios.somee.com");

            var response = await client.GetAsync("/api/rates");

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.ReasonPhrase);
                return;
            }

            var rates = JsonConvert.DeserializeObject<List<Rate>>(result);

            cb_origem.DataSource = rates;
            cb_origem.DisplayMember = "Name";

            //Corrige bug das comboBoxes
            cb_destino.BindingContext = new BindingContext();

            cb_destino.DataSource = rates;
            cb_destino.DisplayMember = "Name";

            progressBar1.Value = 100;
        }
    }
}
