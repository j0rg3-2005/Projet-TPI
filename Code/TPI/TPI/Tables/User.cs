using MySqlConnector;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TPI.Tables
{
    public class User
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // Fonction pour hacher le mot de passe
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Méthode pour inscrire un utilisateur
        public static string RegisterUser(string firstName, string lastName, string email, string password)
        {
            // Vérification si les champs sont remplis
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return "Veuillez remplir tous les champs.";
            }

            // Hachage du mot de passe
            string hashedPassword = HashPassword(password);

            try
            {
                // Vérification si l'email est déjà utilisé
                string checkEmailQuery = "SELECT COUNT(*) FROM users WHERE email = @Email";
                MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, Program.conn);
                checkEmailCmd.Parameters.AddWithValue("@Email", email);
                int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                if (emailCount > 0)
                {
                    return "Cet email est déjà utilisé.";
                }

                string insertQuery = "INSERT INTO users (firstname, lastname, email, password, role) VALUES (@FirstName, @LastName, @Email, @Password, 'client')";
                MySqlCommand cmd = new MySqlCommand(insertQuery, Program.conn);

                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);


                cmd.ExecuteNonQuery();
                return "Utilisateur inscrit avec succès !";
            }
            catch (MySqlException ex)
            {
                return $"Une erreur s'est produite lors de l'inscription : {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Une erreur s'est produite : {ex.Message}";
            }
        }

        public static bool ConnectionSuccessfull(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND password = @Password";
            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);

            object result = cmd.ExecuteScalar();

            return result != null && Convert.ToInt32(result) > 0;
        }

        public static User GetUser(string email, string password)
        {
            string hashedPassword = HashPassword(password);
            string query = "SELECT * FROM users WHERE email = @Email AND password = @Password";

            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        FirstName = reader["firstname"].ToString(),
                        LastName = reader["lastname"].ToString(),
                        Email = reader["email"].ToString(),
                        Password = reader["password"].ToString(),
                        Role = reader["role"].ToString()
                    };
                }
            }

            return null;
        }

        public static List<User> GetAll(string roleFilter = null)
        {
            List<User> utilisateurs = new List<User>();
            string query = "SELECT * FROM users";

            if (!string.IsNullOrEmpty(roleFilter))
            {
                query += " WHERE role = @Role";
            }

            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            if (!string.IsNullOrEmpty(roleFilter))
            {
                cmd.Parameters.AddWithValue("@Role", roleFilter);
            }

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    User user = new User
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        LastName = reader["lastname"].ToString(),
                        FirstName = reader["firstname"].ToString(),
                        Email = reader["email"].ToString(),
                        Role = reader["role"].ToString()
                    };
                    utilisateurs.Add(user);
                }
            }

            return utilisateurs;
        }
        public static void Update(int id, string lastName, string firstName, string email, string role)
        {
            string query = @"UPDATE users 
                     SET lastname = @LastName, firstname = @FirstName, email = @Email, role = @Role WHERE id = @Id";

            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
        public static void Delete(int id)
        {
            string query = "DELETE FROM users WHERE id = @Id";
            MySqlCommand cmd = new MySqlCommand(query, Program.conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }

    }
}
