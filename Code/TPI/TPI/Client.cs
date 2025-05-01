using Microsoft.VisualBasic.ApplicationServices;
using TPI.Tables;
using System.Drawing;
using System.Windows.Forms;

namespace TPI
{
    public partial class frmClient : Form
    {
        TextBox txtSelectedEquipmentId;
        TableLayoutPanel tblEquipment;
        TableLayoutPanel tblCart;
        FlowLayoutPanel flpMain;
        Label lblRole;
        Button btnShowEquipment;
        Button btnShowConsummables;
        Panel pnlAddEquipment;
        int paddingMargin = 15;
        TextBox txtSearch;
        Button btnSearch;

        ComboBox cmbModel;
        ComboBox cmbCategory;
        MonthCalendar monthCalendar;

        private List<(string model, int id, DateTime start, DateTime end)> EquipmentCartItems = new List<(string,int, DateTime, DateTime)>();
        private List<(string model, int id, int quantity)> ConsumableCartItems = new List<(string, int, int)>();

        private enum ViewMode { Equipment, Consumables }
        private ViewMode currentView = ViewMode.Equipment;

        public frmClient()
        {
            InitializeComponent();
        }

        public class ComboBoxItem
        {
            public int Id { get; set; }
            public string DisplayText { get; set; }

            public ComboBoxItem(int id, string displayText)
            {
                Id = id;
                DisplayText = displayText;
            }
            public override string ToString() => DisplayText;
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            InitializeClientUI();
        }

        private void InitializeClientUI()
        {
            this.Controls.Clear();
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(paddingMargin);

            Equipment.verifyState();

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
            pnlAddEquipment.AutoSize = false;

            InitializeEquipmentLendPanel();
            List<Equipment> equipments = Equipment.GetAllAvailableEquipment();
            AddEquipmentsToTable(equipments);

        }

        private void InitializeEquipmentLendPanel()
        {
            pnlAddEquipment.Controls.Clear();

            Label lblModel = new Label() { Text = "Modèle:", Left = 10, Top = 50, AutoSize = true };
            cmbModel = new ComboBox() { Left = 120, Top = 50, Width = 200 };
            cmbModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModel.Enabled = false;

            Label lblCategory = new Label() { Text = "Catégorie :", Left = 10, Top = 20, AutoSize = true };
            cmbCategory = new ComboBox() { Left = 120, Top = 20, Width = 200 };
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            List<Category> categories = Category.GetAllEquipment();
            foreach (var category in categories)
            {
                cmbCategory.Items.Add((category.Name));
            }

            Label lblDate = new Label() { Text = "Date de début/fin:", Left = 10, Top = 80, AutoSize = true };
            monthCalendar = new MonthCalendar();
            monthCalendar.Location = new Point(120, 80);
            monthCalendar.MaxSelectionCount = 14;
            monthCalendar.ShowToday = true;
            monthCalendar.ShowTodayCircle = true;


            Button btnSubmit = new Button() { Text = "Soumettre", Left = 120, Top = 250 };

            cmbCategory.SelectedIndexChanged += (s, ev) =>
                    {
                        cmbModel.Enabled = true;
                        cmbModel.Items.Clear();
                        if (cmbCategory.SelectedIndex != -1)
                        {
                            List<Equipment> equipmentByCategory = Equipment.Search(cmbCategory.SelectedItem.ToString());
                            foreach (var equipment in equipmentByCategory)
                            {
                                cmbModel.Items.Add(new ComboBoxItem(equipment.Id, equipment.Model));
                            }
                            UpdateReservedDates();
                        }
                    };

            cmbModel.SelectedIndexChanged += (s, ev) => UpdateReservedDates();

            btnSubmit.Click += (sender, e) =>
            {
                var selectedItem = cmbModel.SelectedItem as ComboBoxItem;
                int itemId = selectedItem?.Id ?? 0;

                if (cmbCategory.SelectedItem == null || selectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une catégorie et un matériel.");
                    return;
                }

                DateTime startDate = monthCalendar.SelectionStart.Date;
                DateTime endDate = monthCalendar.SelectionEnd.Date;

                List<(DateTime start, DateTime end)> reservedDates = Lends.GetReservedDates(itemId);
                foreach (var range in reservedDates)
                {
                    if ((startDate >= range.start && startDate <= range.end) ||
                        (endDate >= range.start && endDate <= range.end) ||
                        (startDate <= range.start && endDate >= range.end))
                    {
                        MessageBox.Show("La période sélectionnée chevauche une réservation existante.");
                        return;
                    }
                }

                // Juste ajouter au panier
                EquipmentCartItems.Add((selectedItem.DisplayText, startDate, endDate));
                UpdateCartTable();

                cmbModel.SelectedIndex = -1;
                cmbCategory.SelectedIndex = -1;
                monthCalendar.SetSelectionRange(DateTime.Today, DateTime.Today.AddDays(1));
            };

            tblCart = new TableLayoutPanel();
            tblCart.Dock = DockStyle.Fill;
            tblCart.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tblCart.Left = 60;
            tblCart.Top = 500;
            tblCart.Width = (int)(pnlAddEquipment.Width * 0.75);
            tblCart.Anchor = AnchorStyles.None;
            tblCart.AutoScroll = true;
            tblCart.AutoSize = true;

            Button btnValidateEquipment = new Button()
            {
                Text = "Valider la demande",
                Left = tblCart.Left,
                Top = tblCart.Bottom + 10,
                Width = tblCart.Width,
                Height = 30
            };
            btnValidateEquipment.Click += (s, e) =>
            {
                if (EquipmentCartItems.Count == 0)
                {
                    MessageBox.Show("Le panier est vide.");
                    return;
                }

                foreach (var item in EquipmentCartItems)
                {
                    Equipment equip = Equipment.GetAllAvailableEquipment().FirstOrDefault(eq => eq.Model == item.model);

                    if (equip == null)
                    {
                        MessageBox.Show($"Le modèle {item.model} est introuvable.");
                        continue;
                    }

                    Lends.Add(item.start, item.end, DateTime.Today, Session.UserId, equip.Id);
                }

                MessageBox.Show("La demande a été validée avec succès !");
                EquipmentCartItems.Clear();
                UpdateCartTable();
            };

            pnlAddEquipment.Controls.Add(lblModel);
            pnlAddEquipment.Controls.Add(lblDate);
            pnlAddEquipment.Controls.Add(cmbModel);
            pnlAddEquipment.Controls.Add(lblCategory);
            pnlAddEquipment.Controls.Add(cmbCategory);
            pnlAddEquipment.Controls.Add(monthCalendar);
            pnlAddEquipment.Controls.Add(btnSubmit);
            pnlAddEquipment.Controls.Add(tblCart);
            pnlAddEquipment.Controls.Add(btnValidateEquipment);
        }
        private void InitializeConsumableRequestPanel()
        {
            pnlAddEquipment.Controls.Clear();

            Label lblCategory = new Label() { Text = "Catégorie :", Left = 10, Top = 20, AutoSize = true };
            ComboBox cmbCategoryCons = new ComboBox() { Left = 120, Top = 20, Width = 200 };
            cmbCategoryCons.DropDownStyle = ComboBoxStyle.DropDownList;
            List<Category> categories = Category.GetAllConsumables();
            foreach (var category in categories)
            {
                cmbCategoryCons.Items.Add(category.Name);
            }

            Label lblModel = new Label() { Text = "Modèle :", Left = 10, Top = 60, AutoSize = true };
            ComboBox cmbModelCons = new ComboBox() { Left = 120, Top = 60, Width = 200 };
            cmbModelCons.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelCons.Enabled = false;

            Label lblQuantity = new Label() { Text = "Quantité :", Left = 10, Top = 100, AutoSize = true };
            NumericUpDown nudQuantity = new NumericUpDown() { Left = 120, Top = 100, Width = 100, Minimum = 1 };

            Button btnAddToCart = new Button() { Text = "Ajouter au panier", Left = 120, Top = 150 };

            cmbCategoryCons.SelectedIndexChanged += (s, e) =>
            {
                cmbModelCons.Enabled = true;
                cmbModelCons.Items.Clear();
                if (cmbCategoryCons.SelectedIndex != -1)
                {
                    List<Consumables> cons = Consumables.Search(cmbCategoryCons.SelectedItem.ToString());
                    foreach (var c in cons)
                    {
                        cmbModelCons.Items.Add(new ComboBoxItem(c.Id, $"{c.Id} - {c.Model}"));
                    }
                }
            };

            cmbModelCons.SelectedIndexChanged += (s, e) =>
            {
                if (cmbModelCons.SelectedItem is ComboBoxItem selectedItem)
                {
                    Consumables selectedConsumable = Consumables.GetById(selectedItem.Id);
                    if (selectedConsumable != null)
                    {
                        nudQuantity.Maximum = selectedConsumable.Stock;
                        nudQuantity.Value = Math.Min(1, selectedConsumable.Stock);
                    }
                }
            };

            btnAddToCart.Click += (s, e) =>
            {
                if (cmbCategoryCons.SelectedItem == null || cmbModelCons.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une catégorie et un modèle.");
                    return;
                }

                var selected = cmbModelCons.SelectedItem as ComboBoxItem;
                string model = selected.DisplayText.Split('-')[1].Trim();
                int quantity = (int)nudQuantity.Value;

                ConsumableCartItems.Add((model, quantity));
                UpdateCartTable();

                cmbModelCons.SelectedIndex = -1;
                cmbCategoryCons.SelectedIndex = -1;
                nudQuantity.Value = 1;
            };

            tblCart = new TableLayoutPanel();
            tblCart.ColumnCount = 2;
            tblCart.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tblCart.Left = 60;
            tblCart.Top = 200;
            tblCart.Width = (int)(pnlAddEquipment.Width * 0.75);
            tblCart.Anchor = AnchorStyles.None;
            tblCart.AutoScroll = true;

            Button btnValidateConsumable = new Button()
            {
                Text = "Valider la demande",
                Left = tblCart.Left,
                Top = tblCart.Bottom + 10,
                Width = tblCart.Width,
                Height = 30
            };
            btnValidateConsumable.Click += (s, e) =>
            {
                if (ConsumableCartItems.Count == 0)
                {
                    MessageBox.Show("Le panier est vide.");
                    return;
                }

                foreach (var item in ConsumableCartItems)
                {
                    int consumableId = int.Parse(item.Id.Trim().Split('-')[0]);
                    Request.Add(DateTime.Today, Session.UserId, consumableId, item.quantity);
                }

                MessageBox.Show("Demande de consommables validée avec succès !");
                ConsumableCartItems.Clear();
                UpdateCartTable();
            };

            pnlAddEquipment.Controls.Add(lblCategory);
            pnlAddEquipment.Controls.Add(cmbCategoryCons);
            pnlAddEquipment.Controls.Add(lblModel);
            pnlAddEquipment.Controls.Add(cmbModelCons);
            pnlAddEquipment.Controls.Add(lblQuantity);
            pnlAddEquipment.Controls.Add(nudQuantity);
            pnlAddEquipment.Controls.Add(btnAddToCart);
            pnlAddEquipment.Controls.Add(tblCart);
            pnlAddEquipment.Controls.Add(btnValidateConsumable);
        }

        private void UpdateCartTable()
        {
            tblCart.Controls.Clear();
            tblCart.RowStyles.Clear();
            tblCart.RowCount = 0;

            // En-têtes
            if (currentView == ViewMode.Equipment)
            {
                tblCart.ColumnCount = 3;
                tblCart.Controls.Add(new Label() { Text = "Modèle", Font = new Font(Font, FontStyle.Bold) }, 0, 0);
                tblCart.Controls.Add(new Label() { Text = "Début", Font = new Font(Font, FontStyle.Bold) }, 1, 0);
                tblCart.Controls.Add(new Label() { Text = "Fin", Font = new Font(Font, FontStyle.Bold) }, 2, 0);

                for (int i = 0; i < EquipmentCartItems.Count; i++)
                {
                    tblCart.Controls.Add(new Label() { Text = EquipmentCartItems[i].model }, 0, i + 1);
                    tblCart.Controls.Add(new Label() { Text = EquipmentCartItems[i].start.ToShortDateString() }, 1, i + 1);
                    tblCart.Controls.Add(new Label() { Text = EquipmentCartItems[i].end.ToShortDateString() }, 2, i + 1);
                }
            }
            else
            {
                tblCart.ColumnCount = 2;
                tblCart.Controls.Add(new Label() { Text = "Modèle", Font = new Font(Font, FontStyle.Bold) }, 0, 0);
                tblCart.Controls.Add(new Label() { Text = "Quantité", Font = new Font(Font, FontStyle.Bold) }, 1, 0);

                for (int i = 0; i < ConsumableCartItems.Count; i++)
                {
                    tblCart.Controls.Add(new Label() { Text = ConsumableCartItems[i].model }, 0, i + 1);
                    tblCart.Controls.Add(new Label() { Text = ConsumableCartItems[i].quantity.ToString() }, 1, i + 1);
                }
            }
        }


        private void UpdateEquipmentCartTable()
        {
            tblCart.Controls.Clear();
            tblCart.RowStyles.Clear();
            tblCart.RowCount = 0;

            // En-têtes
            tblCart.Controls.Add(new Label() { Text = "Modèle", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true }, 0, 0);
            tblCart.Controls.Add(new Label() { Text = "Début", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true }, 1, 0);
            tblCart.Controls.Add(new Label() { Text = "Fin", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true }, 2, 0);

            int row = 1;

            foreach (var item in EquipmentCartItems)
            {
                tblCart.Controls.Add(new Label() { Text = item.model, AutoSize = true }, 0, row);
                tblCart.Controls.Add(new Label() { Text = item.start.ToShortDateString(), AutoSize = true }, 1, row);
                tblCart.Controls.Add(new Label() { Text = item.end.ToShortDateString(), AutoSize = true }, 2, row);
                row++;
            }

            tblCart.RowCount = row + 1;
        }


        private void UpdateReservedDates()
        {
            if (cmbModel.SelectedItem is ComboBoxItem selectedItem)
            {
                List<(DateTime start, DateTime end)> reservedDates = Lends.GetReservedDates(selectedItem.Id);
                monthCalendar.RemoveAllBoldedDates();

                foreach (var range in reservedDates)
                {
                    DateTime current = range.start;
                    while (current <= range.end)
                    {
                        monthCalendar.AddBoldedDate(current);
                        current = current.AddDays(1);
                    }
                }
                monthCalendar.UpdateBoldedDates();

                monthCalendar.DateSelected += (s, e) =>
                {
                    DateTime selectedStart = monthCalendar.SelectionStart;
                    DateTime selectedEnd = monthCalendar.SelectionEnd;

                    foreach (var range in reservedDates)
                    {
                        if ((selectedStart >= range.start && selectedStart <= range.end) ||
                            (selectedEnd >= range.start && selectedEnd <= range.end) ||
                            (selectedStart <= range.start && selectedEnd >= range.end))
                        {
                            MessageBox.Show("Cette période contient des dates déjà réservées.",
                                          "Période indisponible",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                            monthCalendar.SetSelectionRange(DateTime.Today, DateTime.Today.AddDays(1));
                            break;
                        }
                    }
                };
            }
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
                lblConsumable.Text = $"ID: {consumable.Id} - Modèle: {consumable.Model}"; // Affiche ID et modèle
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
            InitializeEquipmentLendPanel();
            AddEquipmentsToTable(equipments);
        }

        private void btnShowConsummables_Click(object sender, EventArgs e)
        {
            currentView = ViewMode.Consumables;
            List<Consumables> consumables = Consumables.GetAll();
            flpMain.Controls.Clear();
            InitializeConsumableRequestPanel();
            AddConsumablesToTable(consumables);
        }
    }
}