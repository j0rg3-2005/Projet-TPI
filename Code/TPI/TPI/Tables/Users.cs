using MySqlConnector;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TPI.Tables
{
    public class Users
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
                // Gestion des erreurs SQL
                return $"Une erreur s'est produite lors de l'inscription : {ex.Message}";
            }
            catch (Exception ex)
            {
                // Gestion des erreurs générales
                return $"Une erreur s'est produite : {ex.Message}";
            }
        }

        // Fonction de connexion réussie (qui était déjà dans votre code)
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

        public static Users GetUser(string email, string password)
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
                    return new Users
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

        // Récupérer tous les utilisateurs
        public static List<Users> GetAll(string roleFilter = null)
        {
            List<Users> utilisateurs = new List<Users>();

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
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Users user = new Users
                {
                    Id = reader[""] != DBNull.Value ? (int)reader["ID"] : 0,
                    LastName = reader["lastName"] != DBNull.Value ? (string)reader["nom"] : string.Empty,
                    FirstName = reader["firstName"] != DBNull.Value ? (string)reader["prenom"] : string.Empty,
                    Email = reader["email"] != DBNull.Value ? (string)reader["email"] : string.Empty,
                    Password = reader["password"] != DBNull.Value ? (string)reader["motDePasse"] : string.Empty,
                    Role = reader["role"] != DBNull.Value ? (string)reader["role"] : string.Empty
                };
                utilisateurs.Add(user);
            }
            reader.Close();
            return utilisateurs;
        }
    }
}
