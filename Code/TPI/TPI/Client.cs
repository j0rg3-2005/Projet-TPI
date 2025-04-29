using TPI.Tables;

namespace TPI
{
    public partial class frmClient : Form
    {
        TextBox txtSelectedEquipmentId;
        TableLayoutPanel tblEquipment;
        FlowLayoutPanel flpMain;
        Button btnBack;
        Button btnShowEquipment;
        Button btnShowConsummables;
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
                Equipment.AddEquipment(txtModel, txtInventoryNumber, txtSerialNumber, txtCategory);

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

            List<Equipment> equipments = Equipment.GetAll();
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
            List<Equipment> filteredEquipments = Equipment.Search(searchTerm);
            AddEquipmentsToTable(filteredEquipments);
        }

        private void btnShowEquipment_Click(object sender, EventArgs e)
        {
            List<Equipment> equipments = Equipment.GetAll();
            flpMain.Controls.Clear();
            AddEquipmentsToTable(equipments);
        }

        private void btnShowConsummables_Click(object sender, EventArgs e)
        {
            List<Consumables> consumables = Consumables.GetAll();
            flpMain.Controls.Clear();
            AddConsumablesToTable(consumables);
        }
    }
}
