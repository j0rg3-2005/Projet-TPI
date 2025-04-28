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

        // Méthode pour récupérer tous les équipements
        public static List<Equipment> GetAll()
        {
            List<Equipment> equipmentList = new List<Equipment>();

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
            return equipmentList;
        }
    }
}
