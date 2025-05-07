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

        public static Category GetById(int id)
        {
            Category cat = null;

            try
            {
                string query = "SELECT * FROM categories WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cat = new Category
                            {
                                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                                Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                                Type = reader["type"] != DBNull.Value ? reader["type"].ToString() : string.Empty
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de la catégorie : {ex.Message}");
            }

            return cat;
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

        public static void Insert(string name, string type)
        {
            try
            {
                string query = "INSERT INTO categories (name, type) VALUES (@name, @type)";
                using (var cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion de la catégorie : " + ex.Message);
            }
        }

        public static void Update(int id, string name, string type)
        {
            try
            {
                string query = "UPDATE categories SET name = @name, type = @type WHERE id = @id";
                using (var cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour de la catégorie : " + ex.Message);
            }
        }

        public static void Delete(int id)
        {
            try
            {
                string query = "DELETE FROM categories WHERE id = @id";
                using (var cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de la catégorie : " + ex.Message);
            }
        }
    }
}
