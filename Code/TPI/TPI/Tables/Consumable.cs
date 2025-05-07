using MySqlConnector;

namespace TPI.Tables
{
    public class Consumable
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public int CategoryId { get; set; }

        // Récupérer tous les consommables
        public static List<Consumable> GetAll()
        {
            List<Consumable> consumablesList = new List<Consumable>();

            try
            {
                string query = "SELECT * FROM consumables";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Consumable cons = new Consumable
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Model = reader["model"] != DBNull.Value ? reader["model"].ToString() : string.Empty,
                        Stock = reader["stock"] != DBNull.Value ? Convert.ToInt32(reader["stock"]) : 0,
                        MinStock = reader["minStock"] != DBNull.Value ? Convert.ToInt32(reader["minStock"]) : 0,
                        CategoryId = reader["categoryId"] != DBNull.Value ? Convert.ToInt32(reader["categoryId"]) : 0
                    };
                    consumablesList.Add(cons);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des consommables : {ex.Message}");
            }

            return consumablesList;
        }

        // Rechercher des consommables par modèle, stock ou catégorie
        public static List<Consumable> Search(string searchTerm)
        {
            List<Consumable> result = new List<Consumable>();
            List<Consumable> all = GetAll();

            foreach (var cons in all)
            {
                if (cons.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    cons.Stock.ToString().Contains(searchTerm) ||
                    cons.MinStock.ToString().Contains(searchTerm) ||
                    cons.CategoryId.ToString().Contains(searchTerm))
                {
                    result.Add(cons);
                }
            }

            return result;
        }

        // Ajouter un consommable dans la base de données
        public static void AddConsumable(string model, int stock, int minStock, int categoryId)
        {
            try
            {
                string query = "INSERT INTO consumables (model, stock, minStock, categoryId) VALUES (@Model, @Stock, @MinStock, @CategoryId)";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@Stock", stock);
                cmd.Parameters.AddWithValue("@MinStock", minStock);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du consommable : {ex.Message}");
            }
        }

        public static Consumable GetById(int id)
        {
            Consumable consumable = null;

            try
            {
                string query = "SELECT * FROM consumables WHERE id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    consumable = new Consumable
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Model = reader["model"] != DBNull.Value ? reader["model"].ToString() : string.Empty,
                        Stock = reader["stock"] != DBNull.Value ? Convert.ToInt32(reader["stock"]) : 0,
                        MinStock = reader["minStock"] != DBNull.Value ? Convert.ToInt32(reader["minStock"]) : 0,
                        CategoryId = reader["categoryId"] != DBNull.Value ? Convert.ToInt32(reader["categoryId"]) : 0
                    };
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du consommable par ID : {ex.Message}");
            }

            return consumable;
        }
        
        public static void DecreaseQuantity(int consumableId, int quantity)
        {
            try
            {
                string query = "UPDATE consumables SET stock = stock - @qty WHERE id = @id";
                using (var cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@qty", quantity);
                    cmd.Parameters.AddWithValue("@id", consumableId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour de la quantité : " + ex.Message);
            }
        }
        public static void Insert(string model, int stock, int minStock, int categoryId)
        {
            try
            {
                string query = "INSERT INTO consumables (model, stock, minStock, categoryId) VALUES (@model, @stock, @minStock, @categoryId)";
                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@minStock", minStock);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion du consommable : " + ex.Message);
            }
        }
        public static void Update(int id, string model, int stock, int minStock, int categoryId)
        {
            try
            {
                string query = "UPDATE consumables SET model = @model, stock = @stock, minStock = @minStock, categoryId = @categoryId WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@minStock", minStock);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour du consommable : " + ex.Message);
            }
        }
        public static void Delete(int id)
        {
            try
            {
                string query = "DELETE FROM consumables WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du consommable : " + ex.Message);
            }
        }
    }
}