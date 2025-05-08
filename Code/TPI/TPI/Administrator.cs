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
        private Dictionary<string, Func<Control>> tabFactories;
        public frmAdministrator()
        {
            InitializeComponent();
            InitializeUI();
        }
        private void InitializeUI()
        {
            this.Controls.Clear();
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(paddingMargin);

            tabMain = new TabControl
            {
                Dock = DockStyle.Fill,
            };

            ShowTab("Demandes de consommables", new Requests());
            ShowTab("Demandes de prêt", new Lends());
            ShowTab("Matériel", new Equipments());
            ShowTab("Consommables", new Consumables());
            ShowTab("Catégories", new Categories());
            ShowTab("Utilisateurs", new Users());

            this.Controls.Add(tabMain);

            tabFactories = new Dictionary<string, Func<Control>>
            {
                { "Demandes de consommables", () => new Requests() },
                { "Demandes de prêt", () => new Lends() },
                { "Matériel", () => new Equipments() },
                { "Consommables", () => new Consumables() },
                { "Catégories",() => new Categories() },
                { "Utilisateurs", () => new Users() }
            };

            tabMain.SelectedIndexChanged += TabMain_SelectedIndexChanged;

            foreach (var pair in tabFactories)
            {
                ShowTab(pair.Key, pair.Value());
            }
        }
        private void ShowTab(string tabTitle, Control content)
        {
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
        private void TabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabMain.SelectedTab;
            string title = selectedTab.Text;

            if (tabFactories.TryGetValue(title, out var factory))
            {
                selectedTab.Controls.Clear();
                var newControl = factory();
                newControl.Dock = DockStyle.Fill;
                selectedTab.Controls.Add(newControl);
            }
        }
    }
}
