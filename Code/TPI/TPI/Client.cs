using TPI.Tables;

namespace TPI
{
    public partial class frmClient : Form
    {
        TextBox txtSelectedEquipmentId;
        TableLayoutPanel tblEquipment;
        FlowLayoutPanel flpMain;
        Label lblRole;
        Button btnBack;
        Button btnShowEquipment;
        Button btnShowConsummables;
        Panel pnlAddEquipment;
        int paddingMargin = 15;
        TextBox txtSearch;
        Button btnSearch;


        ComboBox cmbModel;
        TextBox txtInventoryNumber;
        TextBox txtSerialNumber;
        ComboBox cmbCategory;


        private enum ViewMode { Equipment, Consumables }
        private ViewMode currentView = ViewMode.Equipment;


        public frmClient()
        {
            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(paddingMargin);
            this.Controls.Add(btnBack);

            lblRole = new Label();
            lblRole.Text = "Bonjour " + Session.Role + " " + Session.FirstName + " " + Session.LastName + " !";
            lblRole.Top = paddingMargin;
            lblRole.Left = (paddingMargin * 2);
            lblRole.AutoSize = true;
            this.Controls.Add(lblRole);

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

            btnShowEquipment = new Button();
            btnShowEquipment.Text = "Afficher le matériel";
            btnShowEquipment.Width = 100;
            btnShowEquipment.AutoSize = true;
            btnShowEquipment.Top = paddingMargin;
            btnShowEquipment.Left = txtSearch.Right + btnSearch.Width + paddingMargin * 2;
            btnShowEquipment.Click += btnShowEquipment_Click;
            this.Controls.Add(btnShowEquipment);

            btnShowConsummables = new Button();
            btnShowConsummables.Text = "Afficher les consommables";
            btnShowConsummables.AutoSize = true;
            btnShowConsummables.Width = 100;
            btnShowConsummables.Top = paddingMargin;
            btnShowConsummables.Left = txtSearch.Right + btnSearch.Width + btnShowEquipment.Width + paddingMargin * 3;
            btnShowConsummables.Click += btnShowConsummables_Click;
            this.Controls.Add(btnShowConsummables);

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


            tblEquipment = new TableLayoutPanel();
            tblEquipment.Dock = DockStyle.Fill;
            tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tblEquipment.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tblEquipment.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            tblEquipment.Height = flpMain.Height - (4 * paddingMargin);
            tblEquipment.RowCount = 0;
            tblEquipment.AutoScroll = true;

            pnlAddEquipment = new Panel();
            pnlAddEquipment.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            pnlAddEquipment.Height = flpMain.Height - (4 * paddingMargin);
            pnlAddEquipment.AutoScroll = true;
            pnlAddEquipment.BorderStyle = BorderStyle.FixedSingle;

            // Modèle
            Label lblModel = new Label() { Text = "Modèle:", Left = 10, Top = 50, AutoSize = true };
            cmbModel = new ComboBox() { Left = 120, Top = 50, Width = 200 };
            cmbModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModel.Enabled = false;

            Label lblStartDate = new Label() { Text = "Date de début:", Left = 10, Top = 80 };
            lblStartDate.AutoSize = true;
            DateTimePicker dtpStartDate = new DateTimePicker() { Left = 120, Top = 80, Width = 200, Format = DateTimePickerFormat.Short };


            Label lblEndDate = new Label() { Text = "Date de fin :", Left = 10, Top = 110 };
            lblEndDate.AutoSize = true;
            DateTimePicker dtpEndDate = new DateTimePicker() { Left = 120, Top = 110, Width = 200, Format = DateTimePickerFormat.Short };


            dtpStartDate.ValueChanged += (s, e) =>
            {
                if (dtpEndDate.Value < dtpStartDate.Value)
                {
                    dtpEndDate.Value = dtpStartDate.Value;
                }
            };
            dtpEndDate.ValueChanged += (s, e) =>
            {
                if (dtpEndDate.Value < dtpStartDate.Value)
                {
                    MessageBox.Show("La date de fin ne peut pas être avant la date de début.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpEndDate.Value = dtpStartDate.Value;
                }
            };

            Label lblCategory = new Label() { Text = "Catégorie :", Left = 10, Top = 20, AutoSize = true };
            cmbCategory = new ComboBox() { Left = 120, Top = 20, Width = 200 };
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            List<Category> categories = Category.GetAllEquipment();
            foreach (var category in categories)
            {
                cmbCategory.Items.Add((category.Name));
            }
            cmbCategory.SelectedIndexChanged += (s, ev) =>
            {
                cmbModel.Enabled = true;
                cmbModel.Items.Clear();
                List<Equipment> equipmentByCategory = Equipment.Search(cmbCategory.SelectedItem.ToString());
                foreach (var equipment in equipmentByCategory)
                {
                    cmbModel.Items.Add(equipment.Model);
                }
                cmbCategory.SelectedIndexChanged += (s, ev) =>
                {
                    cmbModel.Enabled = true;
                };
            };


            Button btnSubmit = new Button() { Text = "Soumettre", Left = 120, Top = 150 };

            btnSubmit.Click += (sender, e) =>
            {
                if (cmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une catégorie.");
                    return;
                }




                cmbModel.SelectedIndex = -1;
                txtInventoryNumber.Clear();
                txtSerialNumber.Clear();
                cmbCategory.SelectedIndex = -1;

                cmbModel.SelectedIndex = -1;
                txtInventoryNumber.Enabled = false;
                txtSerialNumber.Enabled = false;
            };

            pnlAddEquipment.Controls.Add(lblEndDate);
            pnlAddEquipment.Controls.Add(dtpEndDate);
            pnlAddEquipment.Controls.Add(lblModel);
            pnlAddEquipment.Controls.Add(cmbModel);
            pnlAddEquipment.Controls.Add(lblStartDate);
            pnlAddEquipment.Controls.Add(dtpStartDate);
            pnlAddEquipment.Controls.Add(txtInventoryNumber);
            pnlAddEquipment.Controls.Add(txtSerialNumber);
            pnlAddEquipment.Controls.Add(lblCategory);
            pnlAddEquipment.Controls.Add(btnSubmit);
            pnlAddEquipment.Controls.Add(cmbCategory);

            List<Equipment> equipments = Equipment.GetAllAvailableEquipment();
            AddEquipmentsToTable(equipments);
        }

        private void AddEquipmentsToTable(List<Equipment> equipments)
        {
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
                    "\r\n\r\n- Catégorie : " + equipment.CategoryId.ToString() +
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

        private void AddConsumablesToTable(List<Consumables> consumables)
        {
            flpMain.Controls.Remove(tblEquipment);
            tblEquipment.Controls.Clear();
            tblEquipment.RowCount = 0;

            foreach (var consumable in consumables)
            {
                Label lblConsumable = new Label();
                lblConsumable.Text = consumable.Model + " - " + consumable.Id;
                lblConsumable.Anchor = AnchorStyles.None;
                lblConsumable.Padding = new Padding(5);
                lblConsumable.AutoSize = true;

                Button btnShowInfo = new Button();
                btnShowInfo.Text = "Afficher infos";
                btnShowInfo.AutoSize = true;
                btnShowInfo.Padding = new Padding(5);
                btnShowInfo.Anchor = AnchorStyles.None;
                btnShowInfo.Click += (s, ev) =>
                {
                    txtSelectedEquipmentId.Text = "INFOS DU CONSOMMABLE" +
                    "\r\n\r\n- ID : " + consumable.Id.ToString() +
                    "\r\n\r\n- Modèle : " + consumable.Model +
                    "\r\n\r\n- Stock : " + consumable.Stock +
                    "\r\n\r\n- Min stock : " + consumable.MinStock +
                    "\r\n\r\n- Catégorie ID : " + consumable.CategoryId.ToString();
                };

                tblEquipment.RowCount++;
                tblEquipment.Controls.Add(lblConsumable, 0, tblEquipment.RowCount);
                tblEquipment.Controls.Add(btnShowInfo, 1, tblEquipment.RowCount);
            }

            tblEquipment.RowCount++;
            tblEquipment.Controls.Add(new Label(), 0, tblEquipment.RowCount);
            tblEquipment.Controls.Add(new Label(), 1, tblEquipment.RowCount);

            flpMain.Controls.Add(txtSelectedEquipmentId);
            flpMain.Controls.Add(tblEquipment);
            flpMain.Controls.Add(pnlAddEquipment);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return;

            if (currentView == ViewMode.Equipment)
            {
                List<Equipment> filteredEquipments = Equipment.Search(searchTerm);
                flpMain.Controls.Clear();
                AddEquipmentsToTable(filteredEquipments);
            }
            else if (currentView == ViewMode.Consumables)
            {
                List<Consumables> filteredConsumables = Consumables.Search(searchTerm);
                flpMain.Controls.Clear();
                AddConsumablesToTable(filteredConsumables);
            }
        }


        private void btnShowEquipment_Click(object sender, EventArgs e)
        {
            currentView = ViewMode.Equipment;
            List<Equipment> equipments = Equipment.GetAllAvailableEquipment();
            flpMain.Controls.Clear();
            AddEquipmentsToTable(equipments);
        }

        private void btnShowConsummables_Click(object sender, EventArgs e)
        {
            currentView = ViewMode.Consumables;
            List<Consumables> consumables = Consumables.GetAll();
            flpMain.Controls.Clear();
            AddConsumablesToTable(consumables);
        }
    }
}
