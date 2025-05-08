using TPI.Tables;
namespace TPI.AdminTabs
{
    public partial class Lends : UserControl
    {
        private TableLayoutPanel tblLends;
        private TextBox txtSelectedLend;
        private List<Lend> lends;
        private Lend selectedLend;
        private Button btnAccept;
        private Button btnReject;
        private Button btnReturn;

        public Lends()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponents();
            LoadLends();
        }
        private void InitializeComponents()
        {
            Controls.Clear();
            AutoScroll = true;
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(15),
                AutoSize = true
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            Controls.Add(mainLayout);

            InitializeLendTable();
            mainLayout.Controls.Add(tblLends, 0, 0);

            var rightPanel = InitializeRightPanel();
            mainLayout.Controls.Add(rightPanel, 1, 0);
        }
        private void InitializeLendTable()
        {
            tblLends = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
        }
        private FlowLayoutPanel InitializeRightPanel()
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                BorderStyle = BorderStyle.FixedSingle,
            };
            btnAccept = CreateActionButton("Accepter la demande", BtnAccept_Click);
            btnReject = CreateActionButton("Refuser la demande", BtnReject_Click);
            btnReturn = CreateActionButton("Marquer comme retourné", BtnReturn_Click, enabled: false);

            panel.Controls.Add(btnAccept);
            panel.Controls.Add(btnReject);
            panel.Controls.Add(btnReturn);

            txtSelectedLend = new TextBox
            {
                ReadOnly = true,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical,
                Width = 300,
                Height = 200,
                Margin = new Padding(10),
                Text = "Veuillez sélectionner un prêt."
            };
            panel.Controls.Add(txtSelectedLend);

            return panel;
        }
        private Button CreateActionButton(string text, EventHandler handler, bool enabled = true)
        {
            var button = new Button
            {
                Text = text,
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
                Enabled = enabled
            };
            button.Click += handler;
            return button;
        }
        private void AddHeaderRow(string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                tblLends.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / headers.Length));
                tblLends.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }
        }
        private void LoadLends()
        {
            selectedLend = null;
            lends = Lend.GetAll();

            tblLends.Controls.Clear();
            tblLends.RowCount = 1;

            string[] headers = { "Statut", "Début", "Fin", "Retour", "Utilisateur", "Matériel", "Afficher infos" };
            AddHeaderRow(headers);

            int row = 1;
            foreach (var lend in lends)
            AddLendRow(lend, row++);
        }
        private void AddLendRow(Lend lend, int row)
        {
            tblLends.RowCount++;
            tblLends.Controls.Add(new Label() { Text = lend.Status }, 0, row);
            tblLends.Controls.Add(new Label() { Text = lend.StartDate?.ToShortDateString() ?? "-" }, 1, row);
            tblLends.Controls.Add(new Label() { Text = lend.EndDate?.ToShortDateString() ?? "-" }, 2, row);
            tblLends.Controls.Add(new Label() { Text = lend.ReturnDate?.ToShortDateString() ?? "-" }, 3, row);
            tblLends.Controls.Add(new Label() { Text = User.GetUserById(lend.UserId).FirstName + " " + User.GetUserById(lend.UserId).LastName }, 4, row);
            tblLends.Controls.Add(new Label() { Text = Equipment.GetById(lend.EquipmentId).Model }, 4, row);

            var btnShow = new Button()
            {
                Text = "Afficher les infos",
                Tag = lend,
                Width = 120,
                Height = 25
            };
            btnShow.Click += BtnShow_Click;
            tblLends.Controls.Add(btnShow, 5, row);
        }
        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Lend lend)
            {
                selectedLend = lend;
                txtSelectedLend.Text = $"Statut: {lend.Status}\r\n" +
                                       $"Date début: {lend.StartDate?.ToShortDateString()}\r\n" +
                                       $"Date fin: {lend.EndDate?.ToShortDateString()}\r\n" +
                                       $"Date de retour: {lend.ReturnDate?.ToShortDateString()}\r\n" +
                                       $"Date demande: {lend.RequestDate?.ToShortDateString()}\r\n" +
                                       $"Utilisateur: {User.GetUserById(lend.UserId).FirstName + " " + User.GetUserById(lend.UserId).LastName}\r\n" +
                                       $"Matériel: {Equipment.GetById(lend.EquipmentId).Model}";

                btnAccept.Enabled = lend.Status == "en attente";
                btnReject.Enabled = lend.Status == "en attente";
                btnReturn.Enabled = lend.Status == "accepté";
            }
        }
        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (selectedLend == null) return;

            var reservedDates = Lend.GetReservedDates(selectedLend.EquipmentId);
            foreach (var (start, end) in reservedDates)
            {
                if ((selectedLend.StartDate >= start && selectedLend.StartDate <= end) ||
                    (selectedLend.EndDate >= start && selectedLend.EndDate <= end) ||
                    (selectedLend.StartDate <= start && selectedLend.EndDate >= end))
                {
                    MessageBox.Show("Ce matériel est déjà réservé pendant la période demandée.",
                                    "Période indisponible",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
            selectedLend.Status = "accepté";
            selectedLend.StartDate = DateTime.Now;
            selectedLend.Update();

            LoadLends();
            txtSelectedLend.Text = "Demande acceptée.";
        }
        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (selectedLend == null) return;

            selectedLend.Status = "refusé";
            selectedLend.Update();

            LoadLends();
            txtSelectedLend.Text = "Demande refusée.";
            selectedLend = null;
        }
        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (selectedLend == null || selectedLend.Status != "accepté") return;

            selectedLend.Status = "retourné";
            selectedLend.EndDate = DateTime.Now;
            selectedLend.Update();

            LoadLends();
            txtSelectedLend.Text = "Matériel marqué comme retourné.";
            selectedLend = null;
        }
    }
}
