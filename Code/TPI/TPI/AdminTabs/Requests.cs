using TPI.Tables;
namespace TPI.AdminTabs
{
    public partial class Requests : UserControl
    {
        private TableLayoutPanel tblEquipment;
        private TextBox txtSelectedEquipmentId;
        private FlowLayoutPanel pnlButtons;
        private Button btnAccept;
        private Button btnReject;
        private Button btnRefresh;
        private ComboBox cmbStatusFilter;
        private TableLayoutPanel mainLayout;
        private int paddingMargin = 15;
        private List<Request> requests;

        public Requests()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponents();
            LoadRequests();
        }
        private void InitializeComponents()
        {
            this.Controls.Clear();
            this.AutoScroll = true;

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(paddingMargin),
                AutoSize = true
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            this.Controls.Add(mainLayout);
            tblEquipment = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            for (int i = 0; i < 5; i++)
            {
                tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }
            var rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                BorderStyle = BorderStyle.FixedSingle,
            };
            txtSelectedEquipmentId = new TextBox
            {
                ReadOnly = true,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical,
                Width = 300,
                Height = 200,
                Margin = new Padding(10),
                Text = "Veuillez sélectionner une requête."
            };
            rightPanel.Controls.Add(txtSelectedEquipmentId);

            cmbStatusFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 300,
                Margin = new Padding(10)
            };
            cmbStatusFilter.Items.Add("Tous");
            cmbStatusFilter.Items.Add("en attente");
            cmbStatusFilter.Items.Add("accepté");
            cmbStatusFilter.Items.Add("refusé");
            cmbStatusFilter.SelectedIndex = 0;
            cmbStatusFilter.SelectedIndexChanged += (s, e) => LoadRequests();
            rightPanel.Controls.Add(cmbStatusFilter);

            btnRefresh = new Button
            {
                Text = "Rafraîchir",
                Width = 300,
                Height = 30,
                Margin = new Padding(10)
            };
            btnRefresh.Click += (s, e) => RefreshUI();
            rightPanel.Controls.Add(btnRefresh);

            pnlButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Width = 300,
                Height = 40,
                Margin = new Padding(10),
                WrapContents = false,
                AutoSize = false
            };

            btnAccept = new Button
            {
                Text = "Accepter",
                Width = 100,
                Height = 30,
                Margin = new Padding(5),
                Enabled = false
            };
            btnAccept.Click += BtnAccept_Click;

            btnReject = new Button
            {
                Text = "Refuser",
                Width = 100,
                Height = 30,
                Margin = new Padding(5),
                Enabled = false
            };
            btnReject.Click += BtnReject_Click;

            pnlButtons.Controls.Add(btnAccept);
            pnlButtons.Controls.Add(btnReject);
            rightPanel.Controls.Add(pnlButtons);

            mainLayout.Controls.Add(rightPanel, 1, 0);
        }
        private Request selectedRequest;
        private void LoadRequests()
        {
            mainLayout.Controls.Remove(tblEquipment);
            requests = Request.GetAll();

            string filter = cmbStatusFilter?.SelectedItem?.ToString();
            var filtered = string.IsNullOrEmpty(filter) || filter == "Tous"
                ? requests
                : requests.FindAll(r => r.Status.Equals(filter, StringComparison.OrdinalIgnoreCase));

            tblEquipment.Controls.Clear();
            tblEquipment.RowCount = 1;

            tblEquipment.Controls.Add(new Label() { Text = "Statut", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Date", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 1, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Quantité", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 2, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Utilisateur", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 3, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Consommable", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true }, 4, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Action", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 5, 0);

            int row = 1;
            foreach (var req in filtered)
            {
                var lblStatus = new Label() { Text = req.Status };
                var lblDate = new Label() { Text = req.RequestDate.ToShortDateString() };
                var lblQty = new Label() { Text = req.ConsumableQuantity.ToString() };
                var lblUser = new Label() { Text = User.GetUserById(req.UserId).FirstName + " " + User.GetUserById(req.UserId).LastName };
                var lblConsumable = new Label() { Text = Consumable.GetById(req.ConsumableId).Model };

                var btnShow = new Button()
                {
                    Text = "Afficher les infos",
                    Tag = req,
                    Width = 120,
                    Height = 25
                };
                btnShow.Click += BtnShow_Click;

                tblEquipment.RowCount++;
                tblEquipment.Controls.Add(lblStatus, 0, row);
                tblEquipment.Controls.Add(lblDate, 1, row);
                tblEquipment.Controls.Add(lblQty, 2, row);
                tblEquipment.Controls.Add(lblUser, 3, row);
                tblEquipment.Controls.Add(lblConsumable, 4, row);
                tblEquipment.Controls.Add(btnShow, 5, row);
                row++;
            }
            mainLayout.Controls.Add(tblEquipment, 0, 0);
        }
        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Request req)
            {
                selectedRequest = req;
                txtSelectedEquipmentId.Text = $"Statut: {req.Status}\r\n" +
                                              $"Date: {req.RequestDate.ToShortDateString()}\r\n" +
                                              $"Quantité: {req.ConsumableQuantity}\r\n" +
                                              $"Utilisateur: {User.GetUserById(req.UserId).FirstName + " " + User.GetUserById(req.UserId).LastName}\r\n" +
                                              $"Consommable: {Consumable.GetById(req.ConsumableId).Model}";
            }
            if (selectedRequest.Status == "en attente")
            {
                btnAccept.Enabled = true;
                btnReject.Enabled = true;
            }
            else
            {
                btnAccept.Enabled = false;
                btnReject.Enabled = false;
            }
        }
        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (selectedRequest == null)
            {
                MessageBox.Show("Veuillez d'abord sélectionner une requête.");
                return;
            }
            var consumable = Consumable.GetById(selectedRequest.ConsumableId);
            if (consumable == null)
            {
                MessageBox.Show("Consommable introuvable.");
                return;
            }
            if (consumable.Stock < selectedRequest.ConsumableQuantity)
            {
                MessageBox.Show("Stock insuffisant pour valider cette requête.\nLe stock actuel est de " + consumable.Stock + ".", "Erreur de stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Request.UpdateRequestStatus(selectedRequest.Id, "accepté");

            RefreshUI();
        }
        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (selectedRequest == null)
            {
                MessageBox.Show("Veuillez d'abord sélectionner une requête.");
                return;
            }
            Request.UpdateRequestStatus(selectedRequest.Id, "refusé");
            RefreshUI();
        }
        private void RefreshUI()
        {
            selectedRequest = null;
            txtSelectedEquipmentId.Text = "Veuillez sélectionner une requête.";
            LoadRequests();
        }
    }
}
