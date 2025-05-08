using MySqlConnector;
namespace TPI.Tables
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string InventoryNumber { get; set; }
        public bool Available { get; set; }
        public string SerialNumber { get; set; }
        public int CategoryId { get; set; }

        public static List<Equipment> GetAll()
        {
            List<Equipment> equipmentList = new List<Equipment>();

            try
            {
                string query = "SELECT * FROM equipment";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Equipment equipment = new Equipment
                    {
                        Id = reader["id"] != DBNull.Value ? (int)reader["id"] : 0,
                        Model = reader["model"] != DBNull.Value ? (string)reader["model"] : string.Empty,
                        InventoryNumber = reader["inventoryNumber"] != DBNull.Value ? (string)reader["inventoryNumber"] : string.Empty,
                        Available = reader["available"] != DBNull.Value ? (bool)reader["available"] : true,
                        SerialNumber = reader["serialNumber"] != DBNull.Value ? (string)reader["serialNumber"] : string.Empty,
                        CategoryId = reader["categoryId"] != DBNull.Value ? (int)reader["categoryId"] : 0
                    };
                    equipmentList.Add(equipment);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des équipements : {ex.Message}");
            }
            return equipmentList;
        }
        public static Equipment GetById(int id)
        {
            string query = "SELECT * FROM equipment WHERE id = @Id";

            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Equipment
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Model = reader["model"].ToString(),
                        InventoryNumber = reader["inventoryNumber"] != DBNull.Value ? reader["inventoryNumber"].ToString() : string.Empty,
                        Available = Convert.ToBoolean(reader["available"]),
                        SerialNumber = reader["serialNumber"] != DBNull.Value ? reader["serialNumber"].ToString() : string.Empty,
                        CategoryId = reader["categoryId"] != DBNull.Value ? Convert.ToInt32(reader["categoryId"]) : 0
                    };
                }
            }
            return null;
        }
        public static void verifyState()
        {
            try
            {
                string queryLends = @"SELECT DISTINCT equipmentId FROM lends WHERE status IN ('en cours') 
                AND DATE(startDate) <= CURDATE() AND DATE(endDate) >= CURDATE();";
                HashSet<int> lentEquipments = new HashSet<int>();

                using (MySqlCommand cmd = new MySqlCommand(queryLends, Program.conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lentEquipments.Add(reader.GetInt32("equipmentId"));
                    }
                }
                string getAllEquipmentsQuery = "SELECT id FROM equipment";
                List<int> allEquipments = new List<int>();

                using (MySqlCommand cmd = new MySqlCommand(getAllEquipmentsQuery, Program.conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allEquipments.Add(reader.GetInt32("id"));
                    }
                }
                using (var transaction = Program.conn.BeginTransaction())
                {
                    try
                    {
                        foreach (int equipmentId in allEquipments)
                        {
                            bool isLent = lentEquipments.Contains(equipmentId);
                            string updateQuery = "UPDATE equipment SET available = @available WHERE id = @id";

                            using (MySqlCommand cmd = new MySqlCommand(updateQuery, Program.conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@available", isLent ? 0 : 1);
                                cmd.Parameters.AddWithValue("@id", equipmentId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification des états : {ex.Message}");
            }
        }
        public static void SetAvailability(int equipmentId, bool available)
        {
            try
            {
                string query = "UPDATE equipment SET available = @available WHERE id = @id";
                using (var cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@available", available);
                    cmd.Parameters.AddWithValue("@id", equipmentId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de la disponibilité : {ex.Message}");
            }
        }
        public static void Update(int id, string model, string inventoryNumber, string serialNumber, int categoryId, bool available)
        {
            try
            {
                string query = "UPDATE equipment SET model = @Model, inventoryNumber = @InventoryNumber, serialNumber = @SerialNumber, categoryId = @CategoryId, available = @Available WHERE id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@InventoryNumber", inventoryNumber);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Available", available);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de l'équipement : {ex.Message}");
            }
        }
        public static void Insert(string model, string inventoryNumber, string serialNumber, int categoryId, bool available)
        {
            try
            {
                string query = "INSERT INTO equipment (model, inventoryNumber, serialNumber, categoryId, available) VALUES (@Model, @InventoryNumber, @SerialNumber, @CategoryId, @Available);";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@InventoryNumber", inventoryNumber);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Available", available);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout de l'équipement : {ex.Message}");
            }
        }
        public static void Delete(int id)
        {
            try
            {
                string query = "DELETE FROM equipment WHERE id = @Id;";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de l'équipement : {ex.Message}");
            }
        }
        public static List<Equipment> Search(string searchTerm)
        {
            List<Equipment> filteredEquipments = new List<Equipment>();
            List<Equipment> allEquipments = GetAll();
            List<Category> allCategories = Category.GetAll();

            int? matchedCategoryId = allCategories.FirstOrDefault(c => c.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))?.Id;

            foreach (var equipment in allEquipments)
            {
                if ((equipment.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.InventoryNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.SerialNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.CategoryId.ToString().Contains(searchTerm) ||
                    (matchedCategoryId.HasValue && equipment.CategoryId == matchedCategoryId.Value)) && equipment.Available == true)
                {
                    filteredEquipments.Add(equipment);
                }
            }
            return filteredEquipments;
        }
    }
}
