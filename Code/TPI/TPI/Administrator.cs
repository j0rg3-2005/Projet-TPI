using TPI.AdminTabs;
using TPI.Tables;
using System;
using System.Windows.Forms;

namespace TPI
{
    public partial class frmAdministrator : Form
    {
        private TabControl tabMain;

        private const int paddingMargin = 10;
        private Label lblRole;

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



            // TabControl
            tabMain = new TabControl
            {
                Dock = DockStyle.Fill,
            };
            MessageBox.Show(this.ClientSize.Width.ToString());

            ShowTab("Demandes", new Requests());
            ShowTab("Prêts", new Lends());
            //ShowTab("Matériel", new Equipments());
            //ShowTab("Consommables", new Consumables());
            //ShowTab("Catégories", new Categories());
            //ShowTab("Utilisateurs", new Users());
        }
        private void ShowTab(string tabTitle, Control content)
        {
            // Vérifie si l'onglet existe déjà
            foreach (TabPage tab in tabMain.TabPages)
            {
                if (tab.Text == tabTitle)
                {
                    tabMain.SelectedTab = tab;
                    return;
                }
            }
            var newTab = new TabPage(tabTitle);
            content.Dock = DockStyle.Fill;
            newTab.Controls.Add(content);
            tabMain.TabPages.Add(newTab);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmClient clientForm = new frmClient();
            clientForm.InitializeClientUI();
            clientForm.Show();
        }
    }
}
