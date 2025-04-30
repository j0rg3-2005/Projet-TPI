using MySqlConnector;

namespace TPI.Tables
{
    public class Lends
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public int UserId { get; set; }
        public int EquipmentId { get; set; }

        public static List<Lends> GetAll()
        {
            List<Lends> lendsList = new List<Lends>();

            try
            {
                string query = "SELECT * FROM lends";
                MySqlCommand cmd = new MySqlCommand(query, Program.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Lends lend = new Lends
                    {
                        Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                        Status = reader["status"] != DBNull.Value ? reader["status"].ToString() : string.Empty,
                        StartDate = reader["startDate"] != DBNull.Value ? Convert.ToDateTime(reader["startDate"]) : (DateTime?)null,
                        EndDate = reader["endDate"] != DBNull.Value ? Convert.ToDateTime(reader["endDate"]) : (DateTime?)null,
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

        public static bool Add(string status, DateTime? startDate, DateTime? endDate, DateTime? requestDate, int userId, int equipmentId)
        {
            try
            {
                string query = @"
            INSERT INTO lends (status, startDate, endDate, requestDate, userId, equipmentId)
            VALUES (, @startDate, @endDate, @requestDate, @userId, @equipmentId)";

                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@startDate", startDate.HasValue ? startDate.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@endDate", endDate.HasValue ? endDate.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@requestDate", requestDate.HasValue ? requestDate.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@equipmentId", equipmentId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'insertion d'un prêt : {ex.Message}");
                return false;
            }
        }
    }
}

