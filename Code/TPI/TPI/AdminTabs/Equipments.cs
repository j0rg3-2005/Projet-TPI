using TPI.Tables;
namespace TPI.AdminTabs
{
    public partial class Equipments : UserControl
    {
        private TableLayoutPanel tblEquipments;
        private List<Equipment> equipmentList;
        private Equipment selectedEquipment;

        private Button btnSave;
        private Button btnCancel;
        private Button btnDelete;

        private TextBox txtModel;
        private TextBox txtInventory;
        private TextBox txtSerial;
        private CheckBox chkAvailable;
        private ComboBox cmbCategory;

        public Equipments()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponents();
            LoadEquipments();
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

            tblEquipments = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            mainLayout.Controls.Add(tblEquipments, 0, 0);

            var rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
            };

            rightPanel.Controls.Add(new Label { Text = "Modèle", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            txtModel = new TextBox();
            rightPanel.Controls.Add(txtModel, 1, 0);

            rightPanel.Controls.Add(new Label { Text = "N° inventaire", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            txtInventory = new TextBox();
            rightPanel.Controls.Add(txtInventory, 1, 1);

            rightPanel.Controls.Add(new Label { Text = "N° série", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            txtSerial = new TextBox();
            rightPanel.Controls.Add(txtSerial, 1, 2);

            rightPanel.Controls.Add(new Label { Text = "Disponible", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            chkAvailable = new CheckBox();
            rightPanel.Controls.Add(chkAvailable, 1, 3);

            rightPanel.Controls.Add(new Label { Text = "Catégorie", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 4);
            cmbCategory = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Margin = new Padding(10)
            };

            var categories = Category.GetAllEquipment();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";

            rightPanel.Controls.Add(cmbCategory, 1, 4);

            btnSave = new Button
            {
                Text = "Enregistrer",
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
                Enabled = true
            };
            btnSave.Click += BtnSave_Click;
            rightPanel.Controls.Add(btnSave, 1, 5);

            btnCancel = new Button
            {
                Text = "Annuler",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
            };
            btnCancel.Click += BtnCancel_Click;
            rightPanel.Controls.Add(btnCancel, 1, 6);

            btnDelete = new Button
            {
                Text = "Supprimer",
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
                Enabled = false
            };
            btnDelete.Click += BtnDelete_Click;
            rightPanel.Controls.Add(btnDelete, 1, 7);

            mainLayout.Controls.Add(rightPanel, 1, 0);
        }
        private void LoadEquipments()
        {
            selectedEquipment = null;

            equipmentList = Equipment.GetAll();

            tblEquipments.Controls.Clear();
            tblEquipments.RowCount = 1;

            string[] headers = { "Model", "Numéro d'inventaire", "Disponible", "Numéro de serie", "Catégorie", "Afficher infos" };
            for (int i = 0; i < headers.Length; i++)
            {
                tblEquipments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / headers.Length));
                tblEquipments.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true
                }, i, 0);
            }
            int row = 1;
            foreach (var eq in equipmentList)
            {
                tblEquipments.RowCount++;

                var lblModel = new Label { Text = eq.Model , AutoSize = true};
                var lblInv = new Label { Text = eq.InventoryNumber };
                var lblAvail = new Label { Text = eq.Available ? "Oui" : "Non" };
                var lblSerial = new Label { Text = eq.SerialNumber };
                var lblCategory = new Label { Text = Category.GetById(eq.CategoryId).Name };
                var btnSelect = new Button
                {
                    Text = "Sélectionner",
                    Tag = eq,
                    Width = 100,
                    Height = 25
                };
                btnSelect.Click += BtnSelect_Click;

                tblEquipments.Controls.Add(lblModel, 0, row);
                tblEquipments.Controls.Add(lblInv, 1, row);
                tblEquipments.Controls.Add(lblAvail, 2, row);
                tblEquipments.Controls.Add(lblSerial, 3, row);
                tblEquipments.Controls.Add(lblCategory, 4, row);
                tblEquipments.Controls.Add(btnSelect, 5, row);

                row++;
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Equipment eq)
            {
                selectedEquipment = eq;
                btnSave.Enabled = true;
                txtModel.Text = eq.Model;
                txtInventory.Text = eq.InventoryNumber;
                txtSerial.Text = eq.SerialNumber;
                cmbCategory.SelectedValue = eq.CategoryId;
                chkAvailable.Checked = eq.Available;
                btnDelete.Enabled = true;
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            int categoryId = (int)cmbCategory.SelectedValue;

            if (selectedEquipment == null)
            {
                if (txtModel.Text != "" && txtInventory.Text != "" && txtSerial.Text != "" && categoryId != null)
                {
                    Equipment.Insert(txtModel.Text, txtInventory.Text, txtSerial.Text, categoryId, chkAvailable.Checked);
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs !");
                    return;
                }
            }
            else
            {
                Equipment.Update(selectedEquipment.Id, txtModel.Text, txtInventory.Text, txtSerial.Text, categoryId, chkAvailable.Checked);
            }

            txtModel.Text = "";
            txtInventory.Text = "";
            txtSerial.Text = "";
            chkAvailable.Checked = false;
            cmbCategory.SelectedIndex = -1;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            LoadEquipments();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            selectedEquipment = null;
            txtModel.Clear();
            txtInventory.Clear();
            txtSerial.Clear();
            chkAvailable.Checked = false;
            btnSave.Enabled = true;
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedEquipment == null)
                return;

            if (!selectedEquipment.Available)
            {
                MessageBox.Show("Ce matériel est actuellement en prêt et ne peut pas être supprimé.", "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var confirm = MessageBox.Show("Voulez-vous vraiment supprimer ce matériel ainsi que tous les prêts associés ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                Equipment.Delete(selectedEquipment.Id);

                selectedEquipment = null;
                txtModel.Clear();
                txtInventory.Clear();
                txtSerial.Clear();
                chkAvailable.Checked = false;
                cmbCategory.SelectedIndex = -1;
                btnSave.Enabled = true;
                btnDelete.Enabled = false;

                LoadEquipments();
            }
        }
    }
}