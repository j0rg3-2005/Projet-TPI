using Microsoft.Extensions.Configuration;
using MySqlConnector;
namespace TPI
{
    internal static class Program
    {
        public static MySqlConnection conn;

        [STAThread]
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var server = config["Database:Server"];
            var database = config["Database:Database"];
            var username = config["Database:Username"];
            var password = config["Database:Password"];

            string constring = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={password};";
            conn = new MySqlConnection(constring);

            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur de connexion à la base de données :\n\n{ex.Message}\n\n Veuillez contacter votre administrateur IT.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new frmconnection());
        }
    }
}
