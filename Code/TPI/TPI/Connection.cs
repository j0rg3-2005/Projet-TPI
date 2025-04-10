using System;
using System.Drawing;
using System.Windows.Forms;

namespace TPI
{
    public partial class frmconnection : Form
    {
        public frmconnection()
        {
            InitializeComponent();
        }

        private void frmconnection_Load(object sender, EventArgs e)
        {
            this.Text = "Connexion";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlConnection = new Panel
            {
                Size = new Size(300, 370),
                Location = new Point((this.ClientSize.Width - 300) / 2, (this.ClientSize.Height - 370) / 2),
                Anchor = AnchorStyles.None
            };
            this.Controls.Add(pnlConnection);

            Label lblWelcome = new Label
            {
                Text = "Bienvenue ! \r\nVeuillez vous connecter ou vous inscrire.",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 0)
            };
            pnlConnection.Controls.Add(lblWelcome);

            Label lblEmail = new Label
            {
                Text = "Email",
                AutoSize = true,
                Location = new Point(10, 50)
            };
            pnlConnection.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox
            {
                Width = 200,
                Location = new Point(90, 50)
            };
            pnlConnection.Controls.Add(txtEmail);

            Label lblPassword = new Label
            {
                Text = "Mot de passe",
                AutoSize = true,
                Location = new Point(10, 90)
            };
            pnlConnection.Controls.Add(lblPassword);

            TextBox txtPassword = new TextBox
            {
                Width = 200,
                Location = new Point(90, 90),
                UseSystemPasswordChar = true
            };
            pnlConnection.Controls.Add(txtPassword);

            Button btnLogin = new Button
            {
                Text = "Se connecter",
                Width = 250,
                Location = new Point(10, 130)
            };
            pnlConnection.Controls.Add(btnLogin);

            Label lblSignUp = new Label
            {
                Text = "Pas encore de compte ? Créez-en un ci-dessous.",
                AutoSize = true,
                Location = new Point(10, 170)
            };
            pnlConnection.Controls.Add(lblSignUp);

            Button btnSignUp = new Button
            {
                Text = "S'inscrire",
                Width = 250,
                Location = new Point(10, 200),
            };
            btnSignUp.Click += btnSignUp_Click;
            pnlConnection.Controls.Add(btnSignUp);
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            frmSignUp frmSignUp = new frmSignUp();
            frmSignUp.Show();
            this.Hide();
            frmSignUp.FormClosed += (s, args) => this.Show();
        }
    }
}
