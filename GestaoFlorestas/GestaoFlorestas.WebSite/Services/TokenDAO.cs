using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using GestaoFlorestas.WebSite.Models;
using System.Security.Cryptography;

namespace GestaoFlorestas.WebSite.Services
{
    public class TokenDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public TokenDAO()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.server = "1920li4.database.windows.net";
            this.database = "GestaoFlorestal";
            this.userId = "li4_1920";
            this.pass = "Grupo3li";
            String connectionString = "Server=" + server + "; Database=" + database + "; User Id=" + userId + "; Password=" + pass + ";";
            this.con = new SqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                con.Open();
                return true;
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password.");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                con.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private String hashToken(string token)
        {
            using (SHA256 algorHash = SHA256.Create())
            {
                byte[] hash = algorHash.ComputeHash(Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(hash);
            }

        }

        private byte[] hashTokenByte(string token)
        {
            using (SHA256 algorHash = SHA256.Create())
            {
                byte[] hash = algorHash.ComputeHash(Encoding.UTF8.GetBytes(token));
                return hash;
            }

        }

        public Boolean verificarToken(string tokenDado, String tokBD)
        {
            byte[] hashBD = Convert.FromBase64String(tokBD);
            byte[] hashDado = hashTokenByte(tokenDado);

            for (int i = 0; i < hashBD.Length; i++)
                if (hashBD[i] != hashDado[i])
                    return false;
            return true;

        }

        public bool contains(String username, String tipo)
        {
            bool r = false;
            string query = "Select usernameUser from tokens " +
                           "where usernameUser=@username AND tipoUser=@tipo;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }


        public void insertToken(string username, string tipo, string token)
        {
            DateTime time = DateTime.UtcNow;
            string query = "INSERT INTO tokens (usernameUser, tipoUser, token, dataEmissao) VALUES(@username,@tipo,@tok,@date);";

            String tok = hashToken(token);

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@tok", tok);
            cmd.Parameters.AddWithValue("@date", time);

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public Token getToken(string username, string tipo)
        {
            if (contains(username, tipo))
            {
                DateTime data = new DateTime();
                String token = null;
                string query = "Select token, dataEmissao from tokens where usernameUser=@username AND tipoUser=@tipo ;";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                if (this.OpenConnection() == true)
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        reader.Read();

                        token = (String)reader[0];
                        data = (DateTime)reader[1];

                    }
                    this.CloseConnection();
                    return new Token(username, tipo, token, data);
                }
            }
            return null;
        }

        public void DeleteToken(string username, string tipo)
        {

            string query = "Delete from tokens where usernameUser=@username AND tipoUser=@tipo ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

    }
}
