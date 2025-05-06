using MySqlConnector;

namespace TPI.Tables
{
    public class Lend
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public int UserId { get; set; }
        public int EquipmentId { get; set; }

        public static List<Lend> GetAll()
        {
            List<Lend> lendsList = new List<Lend>();

            try
            {
                string query = "SELECT * FROM lends";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Lend lend = new Lend
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Status = reader["status"] != DBNull.Value ? reader["status"].ToString() : string.Empty,
                        StartDate = reader["startDate"] != DBNull.Value ? Convert.ToDateTime(reader["startDate"]) : (DateTime?)null,
                        EndDate = reader["endDate"] != DBNull.Value ? Convert.ToDateTime(reader["endDate"]) : (DateTime?)null,
                        ReturnDate = reader["returnDate"] != DBNull.Value ? Convert.ToDateTime(reader["endDate"]) : (DateTime?)null,
                        RequestDate = reader["requestDate"] != DBNull.Value ? Convert.ToDateTime(reader["requestDate"]) : (DateTime?)null,
                        UserId = reader["userId"] != DBNull.Value ? Convert.ToInt32(reader["userId"]) : 0,
                        EquipmentId = reader["equipmentId"] != DBNull.Value ? Convert.ToInt32(reader["equipmentId"]) : 0
                    };

                    lendsList.Add(lend);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des prêts : {ex.Message}");
            }

            return lendsList;
        }
        public void Update()
        {
            try
            {
                string query = "UPDATE lends SET status = @status, startDate = @start, returnDate = @return, endDate = @end WHERE id = @id";
                using (var cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@status", this.Status);
                    cmd.Parameters.AddWithValue("@start", (object?)this.StartDate ?? DBNull.Value);
                    if (this.Status == "retourné")
                    {
                        cmd.Parameters.AddWithValue("@return", DateTime.Now);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@return", null);
                    }
                    cmd.Parameters.AddWithValue("@end", (object?)this.EndDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", this.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour du prêt : {ex.Message}");
            }
        }

        public static List<(DateTime start, DateTime end)> GetReservedDates(int equipmentId)
        {
            List<(DateTime start, DateTime end)> reservedRanges = new();

            string query = @"SELECT startDate, endDate FROM lends 
                     WHERE equipmentId = @equipmentId AND status = 'accepté'";

            using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
            {
                cmd.Parameters.AddWithValue("@equipmentId", equipmentId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime start = reader.GetDateTime("startDate");
                        DateTime end = reader.GetDateTime("endDate");
                        reservedRanges.Add((start, end));
                    }
                }
            }
            return reservedRanges;
        }

        public static void Add(Lend lend)
        {
            string query = @"INSERT INTO lends (status, startDate, endDate, returnDate, requestDate, userId, equipmentId)
                     VALUES (@status, @start, @end, null, @request, @user, @equip)";

            using (var cmd = new MySqlConnector.MySqlCommand(query, Program.conn))
            {
                cmd.Parameters.AddWithValue("@status", lend.Status);
                cmd.Parameters.AddWithValue("@start", lend.StartDate);
                cmd.Parameters.AddWithValue("@end", lend.EndDate);
                cmd.Parameters.AddWithValue("@request", lend.RequestDate);
                cmd.Parameters.AddWithValue("@user", lend.UserId);
                cmd.Parameters.AddWithValue("@equip", lend.EquipmentId);

                cmd.ExecuteNonQuery();
            }
        }


    }
}

