using MySqlConnector;

namespace TPI.Tables
{
    public class Consumables
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public int CategoryId { get; set; }

        // Récupérer tous les consommables
        public static List<Consumables> GetAll()
        {
            List<Consumables> consumablesList = new List<Consumables>();

            try
            {
                string query = "SELECT * FROM consumables";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Consumables cons = new Consumables
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
        public static List<Consumables> Search(string searchTerm)
        {
            List<Consumables> result = new List<Consumables>();
            List<Consumables> all = GetAll();

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

        // Afficher les consommables dans un tableau
        public static void DisplayConsumables(List<Consumables> consumables, TableLayoutPanel tbl)
        {
            tbl.Controls.Clear();
            tbl.RowCount = 0;

            foreach (var cons in consumables)
            {
                Label lbl = new Label
                {
                    Text = cons.Model + " - ID: " + cons.Id,
                    AutoSize = true,
                    Padding = new Padding(5),
                    Anchor = AnchorStyles.None
                };

                Button btnShow = new Button
                {
                    Text = "Afficher infos",
                    AutoSize = true,
                    Padding = new Padding(5),
                    Anchor = AnchorStyles.None
                };

                btnShow.Click += (s, e) =>
                {
                    string info = $"INFOS DU CONSOMMABLE" +
                                  $"\r\n\r\n- ID : {cons.Id}" +
                                  $"\r\n\r\n- Modèle : {cons.Model}" +
                                  $"\r\n\r\n- Stock : {cons.Stock}" +
                                  $"\r\n\r\n- Min Stock : {cons.MinStock}" +
                                  $"\r\n\r\n- Catégorie ID : {cons.CategoryId}";
                    MessageBox.Show(info, "Détails du consommable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                tbl.RowCount++;
                tbl.Controls.Add(lbl, 0, tbl.RowCount);
                tbl.Controls.Add(btnShow, 1, tbl.RowCount);
            }

            tbl.RowCount++;
            tbl.Controls.Add(new Label(), 0, tbl.RowCount);
            tbl.Controls.Add(new Label(), 1, tbl.RowCount);
        }
    }
}