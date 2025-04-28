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

        // Nouveau bouton pour afficher les consommables et les équipements
        Button btnShowEquipments;
        Button btnShowConsumables;

        public frmClient()
        {
            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(paddingMargin);
            this.Controls.Add(btnBack);

            // Boutons pour basculer entre Equipements et Consommables
            btnShowEquipments = new Button();
            btnShowEquipments.Text = "Equipements";
            btnShowEquipments.AutoSize = true;
            btnShowEquipments.Padding = new Padding(5);
            btnShowEquipments.Click += BtnShowEquipments_Click;
            btnShowEquipments.Location = new Point(this.ClientSize.Width - btnShowEquipments.Width - paddingMargin * 2, paddingMargin);
            flpMain.Controls.Add(btnShowEquipments);

            btnShowConsumables = new Button();
            btnShowConsumables.Text = "Consommables";
            btnShowConsumables.AutoSize = true;
            btnShowConsumables.Padding = new Padding(5);
            btnShowConsumables.Click += BtnShowConsumables_Click;
            btnShowConsumables.Location = new Point(this.ClientSize.Width - btnShowConsumables.Width - paddingMargin, paddingMargin * 2 + btnShowEquipments.Height);
            this.Controls.Add(btnShowConsumables);

            // Champs de recherche
            txtSearch = new TextBox();
            txtSearch.Width = 250;
            txtSearch.PlaceholderText = "Rechercher...";
            txtSearch.Top = paddingMargin * 3 + btnShowEquipments.Height + btnShowConsumables.Height;
            txtSearch.Left = (this.ClientSize.Width - txtSearch.Width - 100 - paddingMargin) / 2;
            this.Controls.Add(txtSearch);

            btnSearch = new Button();
            btnSearch.Text = "Rechercher";
            btnSearch.Width = 100;
            btnSearch.Top = txtSearch.Top;
            btnSearch.Left = txtSearch.Right + paddingMargin;
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            // Panel principal qui contiendra les équipements ou les consommables
            flpMain = new FlowLayoutPanel();
            flpMain.Dock = DockStyle.Fill;
            flpMain.FlowDirection = FlowDirection.LeftToRight;
            flpMain.Padding = new Padding(paddingMargin, paddingMargin * 2, paddingMargin, paddingMargin);
            flpMain.AutoScroll = true;
            this.Controls.Add(flpMain);

            // Zone de texte pour afficher les informations détaillées
            txtSelectedEquipmentId = new TextBox();
            txtSelectedEquipmentId.Text = "Veuillez sélectionner un équipement ou un consommable pour voir ses informations.";
            txtSelectedEquipmentId.ReadOnly = true;
            txtSelectedEquipmentId.Multiline = true;
            txtSelectedEquipmentId.WordWrap = true;
            txtSelectedEquipmentId.ScrollBars = ScrollBars.Vertical;
            txtSelectedEquipmentId.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            txtSelectedEquipmentId.Height = flpMain.Height - (4 * paddingMargin);
            flpMain.Controls.Add(txtSelectedEquipmentId);

            // Table pour afficher les équipements ou les consommables
            tblEquipment = new TableLayoutPanel();
            tblEquipment.Dock = DockStyle.Fill;
            tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tblEquipment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tblEquipment.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tblEquipment.Width = (int)(flpMain.Width * 0.3) - (2 * paddingMargin);
            tblEquipment.Height = flpMain.Height - (4 * paddingMargin);
            tblEquipment.RowCount = 0;
            tblEquipment.AutoScroll = true;

            // Par défaut, afficher la liste des équipements
            DisplayEquipments();
        }

        // Méthode pour afficher les équipements
        private void DisplayEquipments()
        {
            List<Equipment> equipments = Equipment.GetAll();
            AddItemsToTable(equipments, "Équipement");
        }

        // Méthode pour afficher les consommables
        private void DisplayConsumables()
        {
            List<Consumables> consumables = Consumables.GetAll();
            AddItemsToTable(consumables, "Consommable");
        }

        // Méthode pour ajouter des éléments (équipements ou consommables) à la table
        private void AddItemsToTable<T>(List<T> items, string itemType)
        {
            flpMain.Controls.Remove(tblEquipment);
            tblEquipment.Controls.Clear();
            tblEquipment.RowCount = 0;

            foreach (var item in items)
            {
                Label lblItem = new Label();
                string itemDescription = "";
                if (item is Equipment equipment)
                {
                    itemDescription = $"{equipment.Model} - {equipment.InventoryNumber}";
                }
                
                lblItem.Text = itemDescription;
                lblItem.Anchor = AnchorStyles.None;
                lblItem.Padding = new Padding(5);
                lblItem.AutoSize = true;

                Button btnShowItemInfo = new Button();
                btnShowItemInfo.Text = "Afficher infos";
                btnShowItemInfo.AutoSize = true;
                btnShowItemInfo.Padding = new Padding(5);
                btnShowItemInfo.Anchor = AnchorStyles.None;
                btnShowItemInfo.Click += (s, ev) =>
                {
                    txtSelectedEquipmentId.Text = $"{itemType} INFOS" +
                    "\r\n\r\n- Description : " + itemDescription;
                };

                tblEquipment.RowCount++;
                tblEquipment.Controls.Add(lblItem, 0, tblEquipment.RowCount);
                tblEquipment.Controls.Add(btnShowItemInfo, 1, tblEquipment.RowCount);
            }
            tblEquipment.RowCount++;
            tblEquipment.Controls.Add(new Label(), 0, tblEquipment.RowCount);
            tblEquipment.Controls.Add(new Label(), 1, tblEquipment.RowCount);

            flpMain.Controls.Add(txtSelectedEquipmentId);
            flpMain.Controls.Add(tblEquipment);
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
            AddItemsToTable(filteredEquipments, "Équipement");
        }

        // Méthodes pour les boutons "Equipements" et "Consommables"
        private void BtnShowEquipments_Click(object sender, EventArgs e)
        {
            DisplayEquipments();
        }

        private void BtnShowConsumables_Click(object sender, EventArgs e)
        {
            DisplayConsumables();
        }
    }
}
