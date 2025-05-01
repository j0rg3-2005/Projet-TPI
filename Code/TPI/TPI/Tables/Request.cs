using MySqlConnector;
using System;
using System.Collections.Generic;

namespace TPI.Tables
{
    public class Request
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public int ConsumableQuantity { get; set; }
        public int UserId { get; set; }
        public int ConsumableId { get; set; }

        public static List<Request> GetAll()
        {
            List<Request> requests = new List<Request>();

            try
            {
                string query = "SELECT * FROM request";
                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Request request = new Request
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Status = reader["status"].ToString(),
                                RequestDate = Convert.ToDateTime(reader["requestDate"]),
                                ConsumableQuantity = Convert.ToInt32(reader["consumableQuantity"]),
                                UserId = Convert.ToInt32(reader["userId"]),
                                ConsumableId = Convert.ToInt32(reader["consumableId"])
                            };

                            requests.Add(request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des demandes : {ex.Message}");
            }

            return requests;
        }
        public static bool Add(DateTime requestDate, int userId, int consumableId, int quantity)
        {
            try
            {
                string query = @"
        INSERT INTO request (status, requestDate, consumableQuantity, userId, consumableId)
        VALUES ('en attente', @requestDate, @quantity, @userId, @consumableId);";

                using (MySqlCommand cmd = new MySqlCommand(query, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@requestDate", requestDate);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@consumableId", consumableId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout de la demande : {ex.Message}");
                return false;
            }
        }
    }
}
