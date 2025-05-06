using System;
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
            this.Controls.Clear();
            this.AutoScroll = true;

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
            this.Controls.Add(mainLayout);

            tblLends = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            string[] headers = { "ID", "Statut", "Début", "Fin", "Retour", "Utilisateur", "Action" };
            foreach (var h in headers)
                tblLends.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / headers.Length));

            for (int i = 0; i < headers.Length; i++)
            {
                tblLends.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }

            mainLayout.Controls.Add(tblLends, 0, 0);

            var rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false
            };
            var btnAccept = new Button
            {
                Text = "Accepter la demande",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
            };
            btnAccept.Click += BtnAccept_Click;

            var btnReject = new Button
            {
                Text = "Refuser la demande",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
            };
            btnReject.Click += BtnReject_Click;

            var btnReturn = new Button
            {
                Text = "Marquer comme retourné",
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
                Enabled = false
            };
            btnReturn.Click += BtnReturn_Click;

            rightPanel.Controls.Add(btnAccept);
            rightPanel.Controls.Add(btnReject);
            rightPanel.Controls.Add(btnReturn);

            this.btnAccept = btnAccept;
            this.btnReject = btnReject;
            this.btnReturn = btnReturn;

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
            rightPanel.Controls.Add(txtSelectedLend);

            mainLayout.Controls.Add(rightPanel, 1, 0);


        }

        private void LoadLends()
        {
            selectedLend = null;
            lends = Lend.GetAll();

            tblLends.Controls.Clear();
            tblLends.RowCount = 1;

            string[] headers = { "ID", "Statut", "Début", "Fin", "Retour", "Utilisateur", "Action" };
            for (int i = 0; i < headers.Length; i++)
            {
                tblLends.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }

            int row = 1;
            foreach (var lend in lends)
            {
                var lblId = new Label() { Text = lend.Id.ToString() };
                var lblStatus = new Label() { Text = lend.Status };
                var lblStart = new Label() { Text = lend.StartDate?.ToShortDateString() ?? "-" };
                var lblEnd = new Label() { Text = lend.EndDate?.ToShortDateString() ?? "-" };
                var lblReturn = new Label() { Text = lend.ReturnDate?.ToShortDateString() ?? "-" };
                var lblUser = new Label() { Text = lend.UserId.ToString() };

                var btnShow = new Button()
                {
                    Text = "Afficher les infos",
                    Tag = lend,
                    Width = 120,
                    Height = 25
                };
                btnShow.Click += BtnShow_Click;

                tblLends.RowCount++;
                tblLends.Controls.Add(lblId, 0, row);
                tblLends.Controls.Add(lblStatus, 1, row);
                tblLends.Controls.Add(lblStart, 2, row);
                tblLends.Controls.Add(lblEnd, 3, row);
                tblLends.Controls.Add(lblReturn, 4, row);
                tblLends.Controls.Add(lblUser, 5, row);
                tblLends.Controls.Add(btnShow, 6, row);

                row++;
            }
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Lend lend)
            {
                btnReturn.Enabled = false;
                btnAccept.Enabled = false;
                btnReject.Enabled = false;

                selectedLend = lend;
                txtSelectedLend.Text = $"ID: {lend.Id}\r\n" +
                                       $"Statut: {lend.Status}\r\n" +
                                       $"Date début: {lend.StartDate}\r\n" +
                                       $"Date fin: {lend.EndDate}\r\n" +
                                       $"Date de retour: {lend.ReturnDate}\r\n" +
                                       $"Date demande: {lend.RequestDate}\r\n" +
                                       $"Utilisateur: {lend.UserId}\r\n" +
                                       $"Équipement: {lend.EquipmentId}";

                btnReturn.Enabled = lend.Status == "accepté";

                if(lend.Status == "en attente")
                {
                    btnAccept.Enabled = true;
                    btnReject.Enabled = true;
                }
            }
        }
        private void BtnAccept_Click(object sender, EventArgs e)
        {
            if (selectedLend != null)
            {
                // Vérifier si le matériel est déjà réservé pendant les dates demandées
                List<(DateTime start, DateTime end)> reservedDates = Lend.GetReservedDates(selectedLend.EquipmentId);

                foreach (var range in reservedDates)
                {
                    // Vérifier si les dates se chevauchent
                    if ((selectedLend.StartDate >= range.start && selectedLend.StartDate <= range.end) ||
                        (selectedLend.EndDate >= range.start && selectedLend.EndDate <= range.end) ||
                        (selectedLend.StartDate <= range.start && selectedLend.EndDate >= range.end))
                    {
                        // Si une réservation existe déjà pour ces dates, afficher un message d'erreur
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
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (selectedLend != null)
            {
                selectedLend.Status = "refusé";
                selectedLend.Update(); // idem

                LoadLends();
                txtSelectedLend.Text = "Demande refusée.";
                selectedLend = null;
            }
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (selectedLend != null && selectedLend.Status == "accepté")
            {
                selectedLend.Status = "retourné";
                selectedLend.EndDate = DateTime.Now;
                selectedLend.Update();

                LoadLends();
                txtSelectedLend.Text = "Matériel marqué comme retourné.";
                selectedLend = null;
            }
        }

    }
}
