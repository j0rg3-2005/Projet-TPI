using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

        public static bool DeleteById(int id)
        {
            try
            {
                string query = "DELETE FROM equipment WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = cmd.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de l’équipement : {ex.Message}");
                return false;
            }
        }

        public static void AddEquipment(string model, string inventoryNumber, string serialNumber, int categoryId)
        {
            try
            {
                string query = "INSERT INTO equipment (model, inventoryNumber, serialNumber, categoryId) VALUES (@Model, @InventoryNumber, @SerialNumber, @CategoryId);";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@InventoryNumber", inventoryNumber);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout d'un équipement : {ex.Message}");
            }
        }

        public static void UpdateEquipment(int id, string model, string inventoryNumber, string serialNumber, int categoryId, bool available)
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
                Console.WriteLine($"Erreur lors de la mise à jour d'un équipement : {ex.Message}");
            }
        }

        public static List<Equipment> GetAllAvailableEquipment()
        {
            List<Equipment> equipmentList = new List<Equipment>();

            try
            {
                string query = "SELECT * FROM equipment WHERE available = true";
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

        public static void verifyState()
        {
            try
            {
                string queryLends = @"SELECT DISTINCT equipmentId FROM lends
                WHERE status IN ('en cours')
                AND DATE(startDate) <= CURDATE()
                AND DATE(endDate) >= CURDATE();";
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
            List<Equipment> allEquipments = GetAllAvailableEquipment();
            List<Category> allCategories = Category.GetAll();

            int? matchedCategoryId = allCategories
                .FirstOrDefault(c => c.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))?.Id;

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

        public static void DisplayEquipments(List<Equipment> equipments, TableLayoutPanel tblEquipment)
        {
            tblEquipment.Controls.Clear();
            tblEquipment.RowCount = 0;

            // Crée les en-têtes des colonnes
            tblEquipment.ColumnCount = 3;
            tblEquipment.RowCount++;

            tblEquipment.Controls.Add(new Label() { Text = "Modèle", AutoSize = true, Padding = new Padding(5), TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Numéro d'inventaire", AutoSize = true, Padding = new Padding(5), TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            tblEquipment.Controls.Add(new Label() { Text = "Actions", AutoSize = true, Padding = new Padding(5), TextAlign = ContentAlignment.MiddleCenter }, 2, 0);

            foreach (var equipment in equipments)
            {
                tblEquipment.RowCount++;

                Label lblModel = new Label
                {
                    Text = equipment.Model,
                    Anchor = AnchorStyles.None,
                    Padding = new Padding(5),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label lblInventoryNumber = new Label
                {
                    Text = equipment.InventoryNumber,
                    Anchor = AnchorStyles.None,
                    Padding = new Padding(5),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Button btnShowEquipmentInfo = new Button
                {
                    Text = "Afficher infos",
                    AutoSize = true,
                    Padding = new Padding(5),
                    Anchor = AnchorStyles.None
                };

                btnShowEquipmentInfo.Click += (s, ev) =>
                {
                    string equipmentDetails = $"INFOS DE L'ÉQUIPEMENT" +
                    $"\r\n\r\n- ID : {equipment.Id}" +
                    $"\r\n\r\n- Modèle : {equipment.Model}" +
                    $"\r\n\r\n- Numéro d'inventaire : {equipment.InventoryNumber}" +
                    $"\r\n\r\n- Numéro de série : {equipment.SerialNumber}" +
                    $"\r\n\r\n- Catégorie ID : {equipment.CategoryId}" +
                    $"\r\n\r\n- Disponible : {(equipment.Available ? "Oui" : "Non")}";
                    MessageBox.Show(equipmentDetails, "Détails de l'équipement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                tblEquipment.Controls.Add(lblModel, 0, tblEquipment.RowCount);
                tblEquipment.Controls.Add(lblInventoryNumber, 1, tblEquipment.RowCount);
                tblEquipment.Controls.Add(btnShowEquipmentInfo, 2, tblEquipment.RowCount);
            }

            // Ajouter une ligne vide à la fin pour plus de clarté (optionnel)
            tblEquipment.RowCount++;
            tblEquipment.Controls.Add(new Label(), 0, tblEquipment.RowCount);
            tblEquipment.Controls.Add(new Label(), 1, tblEquipment.RowCount);
            tblEquipment.Controls.Add(new Label(), 2, tblEquipment.RowCount);
        }

    }
}
