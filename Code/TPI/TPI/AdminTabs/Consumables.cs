using TPI.Tables;
namespace TPI.AdminTabs
{
    public partial class Consumables : UserControl
    {
        private TableLayoutPanel tblConsumables;
        private List<Consumable> consumableList;
        private Consumable selectedConsumable;
        private TextBox txtModel;
        private TextBox txtStock;
        private TextBox txtMinStock;
        private ComboBox cmbCategory;
        private Button btnSave;
        private Button btnCancel;
        private Button btnDelete;

        public Consumables()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponents();
            LoadConsumables();
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

            tblConsumables = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            mainLayout.Controls.Add(tblConsumables, 0, 0);

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

            rightPanel.Controls.Add(new Label { Text = "Stock", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            txtStock = new TextBox();
            rightPanel.Controls.Add(txtStock, 1, 1);

            rightPanel.Controls.Add(new Label { Text = "Stock Min", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            txtMinStock = new TextBox();
            rightPanel.Controls.Add(txtMinStock, 1, 2);

            rightPanel.Controls.Add(new Label { Text = "Catégorie", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            cmbCategory = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Margin = new Padding(10)
            };
            rightPanel.Controls.Add(cmbCategory, 1, 3);
            var categories = Category.GetAllConsumables(); // Suppose que cette méthode existe pour obtenir les catégories
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";

            btnSave = new Button
            {
                Text = "Enregistrer",
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
            };
            btnSave.Click += BtnSave_Click;
            rightPanel.Controls.Add(btnSave, 1, 4);

            btnCancel = new Button
            {
                Text = "Annuler",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
            };
            btnCancel.Click += BtnCancel_Click;
            rightPanel.Controls.Add(btnCancel, 1, 5);

            btnDelete = new Button
            {
                Text = "Supprimer",
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
                Enabled = false
            };
            btnDelete.Click += BtnDelete_Click;
            rightPanel.Controls.Add(btnDelete, 1, 6);
            mainLayout.Controls.Add(rightPanel, 1, 0);
        }
        private void LoadConsumables()
        {
            selectedConsumable = null;
            consumableList = Consumable.GetAll();

            tblConsumables.Controls.Clear();
            tblConsumables.RowCount = 1;

            string[] headers = { "Model", "Stock", "Min Stock", "Categorie", "Afficher infos" };
            for (int i = 0; i < headers.Length; i++)
            {
                tblConsumables.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }
            int row = 1;
            foreach (var cons in consumableList)
            {
                tblConsumables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / headers.Length));
                tblConsumables.RowCount++;

                var lblModel = new Label { Text = cons.Model };
                var lblStock = new Label { Text = cons.Stock.ToString() };
                var lblMinStock = new Label { Text = cons.MinStock.ToString() };
                var lblCategory = new Label { Text = Category.GetById(cons.CategoryId).Name };
                var btnSelect = new Button
                {
                    Text = "Sélectionner",
                    Tag = cons,
                    AutoSize = true,
                };
                btnSelect.Click += BtnSelect_Click;

                tblConsumables.Controls.Add(lblModel, 0, row);
                tblConsumables.Controls.Add(lblStock, 1, row);
                tblConsumables.Controls.Add(lblMinStock, 2, row);
                tblConsumables.Controls.Add(lblCategory, 3, row);
                tblConsumables.Controls.Add(btnSelect, 4, row);

                row++;
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Consumable cons)
            {
                selectedConsumable = cons;
                txtModel.Text = cons.Model;
                txtStock.Text = cons.Stock.ToString();
                txtMinStock.Text = cons.MinStock.ToString();
                cmbCategory.SelectedValue = cons.CategoryId;
                btnDelete.Enabled = true;
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            int categoryId = (int)cmbCategory.SelectedValue;

            if (selectedConsumable == null)
            {
                if (txtModel.Text != "" && txtStock.Text != "" && txtMinStock.Text != "" && cmbCategory.SelectedValue != null)
                {
                    Consumable.Insert(txtModel.Text, int.Parse(txtStock.Text), int.Parse(txtMinStock.Text), categoryId);
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs !");
                    return;
                }
            }
            else
            {
                Consumable.Update(selectedConsumable.Id, txtModel.Text, int.Parse(txtStock.Text), int.Parse(txtMinStock.Text), categoryId);
            }

            txtModel.Clear();
            txtStock.Clear();
            txtMinStock.Clear();
            cmbCategory.SelectedIndex = -1;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            LoadConsumables();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            selectedConsumable = null;
            txtModel.Clear();
            txtStock.Clear();
            txtMinStock.Clear();
            cmbCategory.SelectedIndex = -1;
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedConsumable == null)
                return;

            var confirm = MessageBox.Show("Voulez-vous vraiment supprimer ce consommable ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                Consumable.Delete(selectedConsumable.Id);

                selectedConsumable = null;
                txtModel.Clear();
                txtStock.Clear();
                txtMinStock.Clear();
                cmbCategory.SelectedIndex = -1;
                btnDelete.Enabled = false;

                LoadConsumables();
            }
        }
    }
}
