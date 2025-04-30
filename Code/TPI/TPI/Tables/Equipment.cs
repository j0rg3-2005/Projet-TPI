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

        public static List<Equipment> GetAllAvailableEquipment()
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

        public static List<Equipment> Search(string searchTerm)
        {
            List<Equipment> filteredEquipments = new List<Equipment>();
            List<Equipment> allEquipments = GetAll();
            List<Category> allCategories = Category.GetAll();

            // Cherche si le searchTerm correspond à une catégorie
            int? matchedCategoryId = allCategories
                .FirstOrDefault(c => c.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))?.Id;

            foreach (var equipment in allEquipments)
            {
                if (equipment.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.InventoryNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.SerialNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    equipment.CategoryId.ToString().Contains(searchTerm) ||
                    (matchedCategoryId.HasValue && equipment.CategoryId == matchedCategoryId.Value))
                {
                    filteredEquipments.Add(equipment);
                }
            }

            return filteredEquipments;
        }


        // Méthode pour ajouter un équipement dans la base de données
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

        public static void DisplayEquipments(List<Equipment> equipments, TableLayoutPanel tblEquipment)
        {
            tblEquipment.Controls.Clear();
            tblEquipment.RowCount = 0;

            foreach (var equipment in equipments)
            {
                Label lblEquipment = new Label
                {
                    Text = equipment.Model + " - " + equipment.InventoryNumber,
                    Anchor = AnchorStyles.None,
                    Padding = new Padding(5),
                    AutoSize = true
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
                tblEquipment.RowCount++;
                tblEquipment.Controls.Add(lblEquipment, 0, tblEquipment.RowCount);
                tblEquipment.Controls.Add(btnShowEquipmentInfo, 1, tblEquipment.RowCount);
            }

            tblEquipment.RowCount++;
            tblEquipment.Controls.Add(new Label(), 0, tblEquipment.RowCount);
            tblEquipment.Controls.Add(new Label(), 1, tblEquipment.RowCount);
        }

        public static void AddEquipment(TextBox txtModel, TextBox txtInventoryNumber, TextBox txtSerialNumber, TextBox txtCategory)
        {
            string model = txtModel.Text.Trim();
            string inventoryNumber = txtInventoryNumber.Text.Trim();
            string serialNumber = txtSerialNumber.Text.Trim();
            int categoryId = int.TryParse(txtCategory.Text.Trim(), out int result) ? result : 0;

            // Validation des champs obligatoires
            if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(inventoryNumber) || string.IsNullOrEmpty(serialNumber))
            {
                MessageBox.Show("Veuillez remplir tous les champs du formulaire avant de soumettre.",
                                "Champs manquants",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Insertion des données dans la base de données
            string query = "INSERT INTO equipment (model, inventoryNumber, serialNumber, categoryId) VALUES (@Model, @InventoryNumber, @SerialNumber, @CategoryId);";
            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            cmd.Parameters.AddWithValue("@Model", model);
            cmd.Parameters.AddWithValue("@InventoryNumber", inventoryNumber);
            cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Les informations ont été enregistrées dans la base de données.",
                                "Succès",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l'équipement : {ex.Message}",
                                "Erreur",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

    }
}
