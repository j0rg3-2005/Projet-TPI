using MySqlConnector;

namespace TPI.Tables
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        // Méthode pour récupérer toutes les catégories
        public static List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string query = "SELECT * FROM categories";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Category cat = new Category
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                        Type = reader["type"] != DBNull.Value ? reader["type"].ToString() : string.Empty
                    };

                    categories.Add(cat);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des catégories : {ex.Message}");
            }

            return categories;
        }

        public static List<Category> GetAllEquipment()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string query = "SELECT * FROM categories WHERE type = \"matériel\"";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Category cat = new Category
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                        Type = reader["type"] != DBNull.Value ? reader["type"].ToString() : string.Empty
                    };

                    categories.Add(cat);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des catégories : {ex.Message}");
            }

            return categories;
        }

        public static List<Category> GetAllConsumables()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string query = "SELECT * FROM categories WHERE type = \"consommable\"";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Category cat = new Category
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                        Type = reader["type"] != DBNull.Value ? reader["type"].ToString() : string.Empty
                    };
                    categories.Add(cat);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des catégories : {ex.Message}");
            }

            return categories;
        }
    }
}
