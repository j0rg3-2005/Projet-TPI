using MySqlConnector;

using TPI.Tables;

namespace TPI
{
    public partial class frmClient : Form
    {
        TextBox txtSelectedEquipmentId;
        TableLayoutPanel tblEquipment;
        FlowLayoutPanel flpMain;
        Button btnBack;
        Panel pnlAddEquipment;
        int paddingMargin = 15;
        TextBox txtSearch;
        Button btnSearch;

        public frmClient()
        {
            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(paddingMargin);
            this.Controls.Add(btnBack);

            txtSearch = new TextBox();
            txtSearch.Width = 250;
            txtSearch.PlaceholderText = "Rechercher un équipement...";
            txtSearch.Top = paddingMargin;
            txtSearch.Left = (this.ClientSize.Width - txtSearch.Width - 100 - paddingMargin) / 2;
            this.Controls.Add(txtSearch);

            btnSearch = new Button();
            btnSearch.Text = "Rechercher";
            btnSearch.Width = 100;
            btnSearch.Top = paddingMargin;
            btnSearch.Left = txtSearch.Right + paddingMargin;
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            flpMain = new FlowLayoutPanel();
            flpMain.Dock = DockStyle.Fill;
            flpMain.FlowDirection = FlowDirection.LeftToRight;
            flpMain.Padding = new Padding(paddingMargin, paddingMargin * 2, paddingMargin, paddingMargin);
            flpMain.AutoScroll = true;
            this.Controls.Add(flpMain);

            txtSelectedEquipmentId = new TextBox();
            txtSelectedEquipmentId.Text = "Veuillez sélectionner un équipement pour voir ses informations.";
            txtSelectedEquipmentId.ReadOnly = true;
            txtSelectedEquipmentId.Multiline = true;
            txtSelectedEquipmentId.WordWrap = true;
            txtSelectedEquipmentId.ScrollBars = ScrollBars.Vertical;
            txtSelectedEquipmentId.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            txtSelectedEquipmentId.Height = flpMain.Height - (4 * paddingMargin);
            flpMain.Controls.Add(txtSelectedEquipmentId);

            tblEquipment = new TableLayoutPanel();
            tblEquipment.Dock = DockStyle.Fill;
            tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tblEquipment.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tblEquipment.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            tblEquipment.Height = flpMain.Height - (4 * paddingMargin);
            tblEquipment.RowCount = 0;
            tblEquipment.AutoScroll = true;

            List<Equipment> equipments = Equipment.GetAll(); // Méthode similaire à GetAll() pour Equipment
            AddEquipmentsToTable(equipments);
            flpMain.Controls.Add(tblEquipment);

            pnlAddEquipment = new Panel();
            pnlAddEquipment.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            pnlAddEquipment.Height = flpMain.Height - (4 * paddingMargin);
            pnlAddEquipment.AutoScroll = true;
            pnlAddEquipment.BorderStyle = BorderStyle.FixedSingle;

            AddEquipmentFormControls(pnlAddEquipment);
            flpMain.Controls.Add(pnlAddEquipment);
        }

        private void AddEquipmentsToTable(List<Equipment> equipments)
        {
            flpMain.Controls.Remove(tblEquipment);
            tblEquipment.Controls.Clear();
            tblEquipment.RowCount = 0;

            foreach (var equipment in equipments)
            {
                Label lblEquipment = new Label();
                lblEquipment.Text = equipment.Model + " - " + equipment.InventoryNumber;
                lblEquipment.Anchor = AnchorStyles.None;
                lblEquipment.Padding = new Padding(5);
                lblEquipment.AutoSize = true;

                Button btnShowEquipmentInfo = new Button();
                btnShowEquipmentInfo.Text = "Afficher infos";
                btnShowEquipmentInfo.AutoSize = true;
                btnShowEquipmentInfo.Padding = new Padding(5);
                btnShowEquipmentInfo.Anchor = AnchorStyles.None;
                btnShowEquipmentInfo.Click += (s, ev) =>
                {
                    txtSelectedEquipmentId.Text = "INFOS DE L'ÉQUIPEMENT" +
                    "\r\n\r\n- ID : " + equipment.Id.ToString() +
                    "\r\n\r\n- Modèle : " + equipment.Model +
                    "\r\n\r\n- Numéro d'inventaire : " + equipment.InventoryNumber +
                    "\r\n\r\n- Numéro de série : " + equipment.SerialNumber +
                    "\r\n\r\n- Catégorie ID : " + equipment.CategoryId.ToString() +
                    "\r\n\r\n- Disponible : " + (equipment.Available ? "Oui" : "Non");
                };

                tblEquipment.RowCount++;
                tblEquipment.Controls.Add(lblEquipment, 0, tblEquipment.RowCount);
                tblEquipment.Controls.Add(btnShowEquipmentInfo, 1, tblEquipment.RowCount);
            }
            tblEquipment.RowCount++;
            tblEquipment.Controls.Add(new Label(), 0, tblEquipment.RowCount);
            tblEquipment.Controls.Add(new Label(), 1, tblEquipment.RowCount);

            flpMain.Controls.Add(txtSelectedEquipmentId);
            flpMain.Controls.Add(tblEquipment);
            flpMain.Controls.Add(pnlAddEquipment);
        }

        private void AddEquipmentFormControls(Panel pnlAddEquipment)
        {
            Label lblModel = new Label() { Text = "Modèle:", Left = 10, Top = 20 };
            lblModel.AutoSize = true;
            TextBox txtModel = new TextBox() { Left = lblModel.Width + 50, Top = 20, Width = 200 };

            Label lblInventoryNumber = new Label() { Text = "Numéro d'inventaire:", Left = 10, Top = 50 };
            lblInventoryNumber.AutoSize = true;
            TextBox txtInventoryNumber = new TextBox() { Left = lblInventoryNumber.Width + 50, Top = 50, Width = 200 };

            Label lblSerialNumber = new Label() { Text = "Numéro de série:", Left = 10, Top = 80 };
            lblSerialNumber.AutoSize = true;
            TextBox txtSerialNumber = new TextBox() { Left = lblSerialNumber.Width + 50, Top = 80, Width = 200 };

            Label lblCategory = new Label() { Text = "Catégorie ID:", Left = 10, Top = 110 };
            lblCategory.AutoSize = true;
            TextBox txtCategory = new TextBox() { Left = lblCategory.Width + 50, Top = 110, Width = 200 };

            Button btnSubmit = new Button() { Text = "Soumettre", Left = 120, Top = 150 };
            btnSubmit.Click += (sender, e) =>
            {
                string model = txtModel.Text.Trim();
                string inventoryNumber = txtInventoryNumber.Text.Trim();
                string serialNumber = txtSerialNumber.Text.Trim();
                int categoryId = int.TryParse(txtCategory.Text.Trim(), out int result) ? result : 0;

                if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(inventoryNumber) || string.IsNullOrEmpty(serialNumber))
                {
                    MessageBox.Show("Veuillez remplir tous les champs du formulaire avant de soumettre.",
                                    "Champs manquants",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string query = "INSERT INTO equipment (model, inventoryNumber, serialNumber, categoryId) VALUES (@Model, @InventoryNumber, @SerialNumber, @CategoryId);";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@InventoryNumber", inventoryNumber);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Les informations ont été enregistrées dans la base de données.",
                                "Succès",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                AddEquipmentsToTable(Equipment.GetAll());
                txtModel.Clear();
                txtInventoryNumber.Clear();
                txtSerialNumber.Clear();
                txtCategory.Clear();
            };

            pnlAddEquipment.Controls.Add(lblModel);
            pnlAddEquipment.Controls.Add(txtModel);
            pnlAddEquipment.Controls.Add(lblInventoryNumber);
            pnlAddEquipment.Controls.Add(txtInventoryNumber);
            pnlAddEquipment.Controls.Add(lblSerialNumber);
            pnlAddEquipment.Controls.Add(txtSerialNumber);
            pnlAddEquipment.Controls.Add(lblCategory);
            pnlAddEquipment.Controls.Add(txtCategory);
            pnlAddEquipment.Controls.Add(btnSubmit);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            List<Equipment> equipments = Equipment.GetAll();
            List<Equipment> filteredEquipments = new List<Equipment>();

            foreach (var equipment in equipments)
            {
                if (equipment.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.InventoryNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.SerialNumber.Contains(searchTerm) ||
                    equipment.CategoryId.ToString().Contains(searchTerm))
                {
                    filteredEquipments.Add(equipment);
                }
            }
            AddEquipmentsToTable(filteredEquipments);
        }
    }
}
