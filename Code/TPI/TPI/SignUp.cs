using TPI.Tables;

namespace TPI
{
    public partial class frmSignUp : Form
    {
        private TextBox txtPrenom;
        private TextBox txtNom;
        private TextBox txtEmail;
        private TextBox txtPassword;

        public frmSignUp()
        {
            InitializeComponent();
        }

        private void frmSignUp_Load(object sender, EventArgs e)
        {
            this.Text = "Inscription";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlInscription = new Panel
            {
                Size = new Size(300, 400),
                Location = new Point((this.ClientSize.Width - 300) / 2, (this.ClientSize.Height - 400) / 2),
                Anchor = AnchorStyles.None
            };
            this.Controls.Add(pnlInscription);

            Label lblWelcome = new Label
            {
                Text = "Créer un compte\nVeuillez remplir le formulaire ci-dessous.",
                Size = new Size(280, 40),
                Location = new Point(10, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlInscription.Controls.Add(lblWelcome);

            Label lblPrenom = new Label
            {
                Text = "Prénom",
                AutoSize = true,
                Location = new Point(10, 50)
            };
            pnlInscription.Controls.Add(lblPrenom);

            txtPrenom = new TextBox
            {
                Width = 250,
                Location = new Point(10, 70)
            };
            pnlInscription.Controls.Add(txtPrenom);

            Label lblNom = new Label
            {
                Text = "Nom",
                AutoSize = true,
                Location = new Point(10, 110)
            };
            pnlInscription.Controls.Add(lblNom);

            txtNom = new TextBox
            {
                Width = 250,
                Location = new Point(10, 130)
            };
            pnlInscription.Controls.Add(txtNom);

            Label lblEmail = new Label
            {
                Text = "Adresse mail",
                AutoSize = true,
                Location = new Point(10, 170)
            };
            pnlInscription.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Width = 250,
                Location = new Point(10, 190)
            };
            pnlInscription.Controls.Add(txtEmail);

            Label lblPassword = new Label
            {
                Text = "Mot de passe",
                AutoSize = true,
                Location = new Point(10, 230)
            };
            pnlInscription.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Width = 250,
                Location = new Point(10, 250),
                UseSystemPasswordChar = true
            };
            pnlInscription.Controls.Add(txtPassword);

            Button btnRegister = new Button
            {
                Text = "S'inscrire",
                Width = 250,
                Location = new Point(10, 290)
            };
            btnRegister.Click += BtnRegister_Click;
            pnlInscription.Controls.Add(btnRegister);

            Label lblLoginMsg = new Label
            {
                Text = "Déjà un compte ? Connectez-vous ci-dessous.",
                Size = new Size(280, 30),
                Location = new Point(10, 330),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlInscription.Controls.Add(lblLoginMsg);

            Button btnLogin = new Button
            {
                Text = "Se connecter",
                Width = 250,
                Location = new Point(10, 360)
            };
            btnLogin.Click += btnLogin_Click;
            pnlInscription.Controls.Add(btnLogin);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmconnection frmconnection = new frmconnection();
            frmconnection.Show();
            this.Hide();
            frmconnection.FormClosed += (s, args) => this.Show();
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string prenom = txtPrenom.Text;
            string nom = txtNom.Text;
            string email = txtEmail.Text;
            string motDePasse = txtPassword.Text;

            string resultMessage = User.RegisterUser(prenom, nom, email, motDePasse);

            frmconnection frmconnection = new frmconnection();
            frmconnection.Show();
            this.Hide();
            frmconnection.FormClosed += (s, args) => this.Show();
        }
    }
}
