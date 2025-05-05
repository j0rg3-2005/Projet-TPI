using TPI.AdminTabs;
using TPI.Tables;
using System;
using System.Windows.Forms;

namespace TPI
{
    public partial class frmAdministrator : Form
    {
        private Button btnBack;
        private Button btnRequests;
        private Button btnLends;
        private Button btnEquipment;
        private Button btnConsumables;
        private Button btnCategories;
        private Button btnUsers;
        private FlowLayoutPanel flpButtons;
        private FlowLayoutPanel flpMain;

        private const int paddingMargin = 20;
        private Label lblRole;

        private Requests requests;

        public frmAdministrator()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void frmAdministrator_Load(object sender, EventArgs e)
        {

        }

        private void InitializeUI()
        {
            this.Controls.Clear();
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(paddingMargin);

            flpMain = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(paddingMargin, paddingMargin * 2, paddingMargin, paddingMargin),
                AutoScroll = true
            };
            this.Controls.Add(flpMain);

            Requests hey = new AdminTabs.Requests();

            flpMain.Controls.Add(hey);

            requests = new Requests();
            requests.Hide();
            flpMain.Controls.Add(requests);

            flpButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(paddingMargin)
            };
            this.Controls.Add(flpButtons);

            btnBack = new Button
            {
                Text = "Retour",
                Width = 100,
                Margin = new Padding(0, 0, paddingMargin * 3, 0)
            };
            btnBack.Click += BtnBack_Click;
            flpButtons.Controls.Add(btnBack);

            // Bouton Demandes
            btnRequests = new Button
            {
                Text = "Demandes",
                Width = 100
            };
            btnRequests.Click += (s, ev) =>
            {
                ShowTab(requests);  // Afficher l'onglet des demandes
            };
            flpButtons.Controls.Add(btnRequests);

            // Bouton Prêts
            btnLends = new Button
            {
                Text = "Prêts",
                Width = 100
            };
            btnLends.Click += (s, ev) =>
            {
               // ShowTab(new Lends());  // Afficher l'onglet des prêts
            };
            flpButtons.Controls.Add(btnLends);

            // Bouton Matériel
            btnEquipment = new Button
            {
                Text = "Matériel",
                Width = 100
            };
            btnEquipment.Click += (s, ev) =>
            {
               // ShowTab(new Equipment());  // Afficher l'onglet matériel
            };
            flpButtons.Controls.Add(btnEquipment);

            // Bouton Consommables
            btnConsumables = new Button
            {
                Text = "Consommables",
                Width = 100
            };
            btnConsumables.Click += (s, ev) =>
            {
               // ShowTab(new Consumables());  // Afficher l'onglet consommables
            };
            flpButtons.Controls.Add(btnConsumables);

            // Bouton Catégories
            btnCategories = new Button
            {
                Text = "Catégories",
                Width = 100
            };
            btnCategories.Click += (s, ev) =>
            {
               // ShowTab(new Categories());  // Afficher l'onglet catégories
            };
            flpButtons.Controls.Add(btnCategories);

            // Bouton Utilisateurs
            btnUsers = new Button
            {
                Text = "Utilisateurs",
                Width = 100
            };
            btnUsers.Click += (s, ev) =>
            {
                //ShowTab(new Users());  // Afficher l'onglet utilisateurs
            };
            flpButtons.Controls.Add(btnUsers);
        }

        private void ShowTab(Control tab)
        {
            // Vider l'onglet principal
            flpMain.Controls.Clear();

            // Ajouter le nouveau contrôle
            flpMain.Controls.Add(tab);
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
