using TPI.Tables;
namespace TPI.AdminTabs
{
    public partial class Users : UserControl
    {
        private TableLayoutPanel tblUsers;
        private List<User> userList;
        private User selectedUser;
        private TextBox txtLastName;
        private TextBox txtFirstName;
        private TextBox txtEmail;
        private ComboBox cmbRole;
        private TableLayoutPanel mainLayout;
        private Button btnSave;
        private Button btnCancel;
        private Button btnDelete;

        public Users()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponents();
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
                Padding = new Padding(15),
                AutoSize = true
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            tblUsers = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            string[] headers = { "Nom", "Prénom", "Email", "Rôle", "Afficher infos" };
            foreach (var h in headers)
                tblUsers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / headers.Length));

            for (int i = 0; i < headers.Length; i++)
            {
                tblUsers.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }

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
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            rightPanel.Controls.Add(new Label { Text = "Nom", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            txtLastName = new TextBox { Width = 200 };
            rightPanel.Controls.Add(txtLastName, 1, 0);

            rightPanel.Controls.Add(new Label { Text = "Prénom", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            txtFirstName = new TextBox { Width = 200 };
            rightPanel.Controls.Add(txtFirstName, 1, 1);

            rightPanel.Controls.Add(new Label { Text = "Email", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            txtEmail = new TextBox { Width = 200 };
            rightPanel.Controls.Add(txtEmail, 1, 2);

            rightPanel.Controls.Add(new Label { Text = "Rôle", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            cmbRole = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200
            };
            cmbRole.Items.Add("admin");
            cmbRole.Items.Add("client");
            rightPanel.Controls.Add(cmbRole, 1, 3);

            btnSave = new Button
            {
                Text = "Modifier",
                Width = 200,
                Height = 30,
                Margin = new Padding(10)
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
            this.Controls.Add(mainLayout);
            rightPanel.Controls.Add(btnDelete, 1, 6);
            mainLayout.Controls.Add(rightPanel, 1, 0);
            LoadUsers();
            mainLayout.Controls.Add(tblUsers, 0, 0);
        }
        private void LoadUsers()
        {
            selectedUser = null;
            userList = User.GetAll();
            tblUsers.Controls.Clear();
            tblUsers.RowCount = 1;

            string[] headers = { "Nom", "Prénom", "Email", "Rôle", "Actions" };
            for (int i = 0; i < headers.Length; i++)
            {
                tblUsers.Controls.Add(new Label()
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }, i, 0);
            }

            int row = 1;
            foreach (var user in userList)
            {
                tblUsers.RowCount++;

                var lblLastName = new Label { Text = user.LastName, AutoSize = true};
                var lblFirstName = new Label { Text = user.FirstName, AutoSize = true };
                var lblEmail = new Label { Text = user.Email, AutoSize = true };
                var lblRole = new Label { Text = user.Role, AutoSize = true };

                var btnSelect = new Button
                {
                    Text = "Sélectionner",
                    Tag = user,
                    Width = 100,
                    Height = 25
                };
                btnSelect.Click += BtnSelect_Click;

                tblUsers.Controls.Add(lblLastName, 0, row);
                tblUsers.Controls.Add(lblFirstName, 1, row);
                tblUsers.Controls.Add(lblEmail, 2, row);
                tblUsers.Controls.Add(lblRole, 3, row);
                tblUsers.Controls.Add(btnSelect, 4, row);

                row++;
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is User user)
            {
                selectedUser = user;
                txtLastName.Text = user.LastName;
                txtFirstName.Text = user.FirstName;
                txtEmail.Text = user.Email;
                cmbRole.SelectedItem = user.Role;
                btnDelete.Enabled = true;
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedUser == null)
                return;

            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }
            User.Update(selectedUser.Id,
                        txtLastName.Text,
                        txtFirstName.Text,
                        txtEmail.Text,
                        cmbRole.SelectedItem.ToString());
            ClearForm();
            LoadUsers();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUser == null)
                return;

            var confirm = MessageBox.Show("Voulez-vous vraiment supprimer cet utilisateur ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                User.Delete(selectedUser.Id);
                ClearForm();
                LoadUsers();
            }
        }
        private void ClearForm()
        {
            selectedUser = null;
            txtLastName.Clear();
            txtFirstName.Clear();
            txtEmail.Clear();
            cmbRole.SelectedIndex = -1;
            btnDelete.Enabled = false;
        }
    }
}
