using MySqlConnector;

namespace TPI
{
    internal static class Program
    {
        public static MySqlConnection conn;

        [STAThread]
        static void Main(string[] args)
        {
            string server = "localhost";
            string database = "tpi";
            string username = "root";
            string password = "Pa$$w0rd";
            string constring = "SERVER=" + server + ";" + "DATABASE=" + database + ";" +
                "UID=" + username + ";" + "PASSWORD=" + password + ";";

            conn = new MySqlConnection(constring);
            conn.Open();

            ApplicationConfiguration.Initialize();
            Application.Run(new frmconnection());
        }
    }
}