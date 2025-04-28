using MySqlConnector;
using System;
using System.Collections.Generic;

namespace TPI.Tables
{
    public class Consumables
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public int CategoryId { get; set; }

        public static List<Consumables> GetAll()
        {
            List<Consumables> consumableList = new List<Consumables>();

            string query = "SELECT * FROM consumables";
            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Consumables consumable = new Consumables
                {
                    Id = reader["id"] != DBNull.Value ? (int)reader["id"] : 0,
                    Model = reader["model"] != DBNull.Value ? (string)reader["model"] : string.Empty,
                    Stock = reader["stock"] != DBNull.Value ? (int)reader["stock"] : 0,
                    MinStock = reader["minStock"] != DBNull.Value ? (int)reader["minStock"] : 0,
                    CategoryId = reader["categoryId"] != DBNull.Value ? (int)reader["categoryId"] : 0
                };
                consumableList.Add(consumable);
            }
            reader.Close();
            return consumableList;
        }
    }
}
