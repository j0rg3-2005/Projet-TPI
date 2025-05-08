using TPI.Tables;

namespace TPI.AdminTabs
{
    public partial class Categories : UserControl
    {
        private TableLayoutPanel tblCategories;
        private List<Category> categoryList;
        private Category selectedCategory;
        private TextBox txtName;
        private ComboBox cmbType;
        private Button btnSave;
        private Button btnCancel;
        private Button btnDelete;

        public Categories()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponents();
            LoadCategories();
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

            tblCategories = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            mainLayout.Controls.Add(tblCategories, 0, 0);

            var rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
            };
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            rightPanel.Controls.Add(new Label { Text = "Nom", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            txtName = new TextBox { Width = 200 };
            rightPanel.Controls.Add(txtName, 1, 0);

            rightPanel.Controls.Add(new Label { Text = "Type", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            cmbType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200
            };
            cmbType.Items.Add("matériel");
            cmbType.Items.Add("consommable");
            rightPanel.Controls.Add(cmbType, 1, 1);

            btnSave = new Button
            {
                Text = "Enregistrer",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
            };
            btnSave.Click += BtnSave_Click;
            rightPanel.Controls.Add(btnSave, 1, 2);

            btnCancel = new Button
            {
                Text = "Annuler",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
            };
            btnCancel.Click += BtnCancel_Click;
            rightPanel.Controls.Add(btnCancel, 1, 3);

            btnDelete = new Button
            {
                Text = "Supprimer",
                Width = 200,
                Height = 30,
                Margin = new Padding(10),
                Enabled = false
            };
            btnDelete.Click += BtnDelete_Click;
            rightPanel.Controls.Add(btnDelete, 1, 4);
            mainLayout.Controls.Add(rightPanel, 1, 0);
        }
        private void LoadCategories()
        {
            selectedCategory = null;
            categoryList = Category.GetAll();
            tblCategories.Controls.Clear();
            tblCategories.RowCount = 1;

            string[] headers = { "Nom", "Type", "Afficher infos" };
            for (int i = 0; i < headers.Length; i++)
            {
                tblCategories.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / headers.Length));
                tblCategories.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }

            int row = 1;
            foreach (var cat in categoryList)
            {
                tblCategories.RowCount++;

                var lblName = new Label { Text = cat.Name, AutoSize = true};
                var lblType = new Label { Text = cat.Type };
                var btnSelect = new Button
                {
                    Text = "Sélectionner",
                    Tag = cat,
                    Width = 100,
                    Height = 25
                };
                btnSelect.Click += BtnSelect_Click;

                tblCategories.Controls.Add(lblName, 0, row);
                tblCategories.Controls.Add(lblType, 1, row);
                tblCategories.Controls.Add(btnSelect, 2, row);

                row++;
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Category cat)
            {
                selectedCategory = cat;
                txtName.Text = cat.Name;
                cmbType.SelectedItem = cat.Type;
                btnDelete.Enabled = true;
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || cmbType.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }
            if (selectedCategory == null)
            {
                Category.Insert(txtName.Text, cmbType.SelectedItem.ToString());
            }
            else
            {
                Category.Update(selectedCategory.Id, txtName.Text, cmbType.SelectedItem.ToString());
            }
            ClearForm();
            LoadCategories();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCategory == null)
                return;

            var confirm = MessageBox.Show("Voulez-vous vraiment supprimer cette catégorie ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                Category.Delete(selectedCategory.Id);
                ClearForm();
                LoadCategories();
            }
        }
        private void ClearForm()
        {
            selectedCategory = null;
            txtName.Clear();
            cmbType.SelectedIndex = -1;
            btnDelete.Enabled = false;
        }
    }
}
