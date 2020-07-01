using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using GestaoFlorestas.WebSite.Models;
using System.Security.Cryptography;

namespace GestaoFlorestas.WebSite.Services
{
    public class ProprietarioDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public ProprietarioDAO()
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

        private byte[] createSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;
        }

        private byte[] creatHash(String password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            return hash;
        }

        private String creatHash(String password, String salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }

        private bool PassEquals(byte[] hashDb, byte[] hashVerificar)
        {
            for (int i = 0; i < 20; i++)
                if (hashDb[i] != hashVerificar[i])
                    return false;
            return true;
        }

        public bool verificarPassword(String pass, String user)
        {
            String passDb = null;
            byte[] salt = null;
            string query = "Select password,salt from proprietario " +
                           "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();
                    passDb = (String)reader[0];
                    salt = Convert.FromBase64String((String)reader[1]);
                }
                this.CloseConnection();
            }
            if (passDb != null && salt != null)
            {
                byte[] hashDb = Convert.FromBase64String(passDb);

                var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000);
                byte[] hashVerificar = pbkdf2.GetBytes(20);
                return PassEquals(hashDb, hashVerificar);
            }
            else return false; //User inexistente
        }


        public void put(Proprietario p)
        {
            int i;
            String password = "";
            String query;
            String salt = "";
            if (contains(p.getUsername()))
            {
                i = 0;
                query = "UPDATE proprietario SET nif=@nif,email=@email,nome=@nome WHERE username=@username ;";
            }
            else
            {
                i = 1;
                query = "INSERT INTO proprietario (username,password,nif,email,nome,salt) VALUES(@username,@password,@nif,@email,@nome,@salt);";
                salt = Convert.ToBase64String(createSalt());
                password = creatHash(p.getPassword(), salt);
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", p.getUsername());
            if (i == 1) cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@nif", Int32.Parse(p.getNif()));
            cmd.Parameters.AddWithValue("@email", p.getMail());
            cmd.Parameters.AddWithValue("@nome", p.getNome());
            if (i == 1) cmd.Parameters.AddWithValue("@salt", salt);
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }




        public bool contains(String p)
        {
            bool r = false;
            string query = "Select username from proprietario " +
                           "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", p);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public bool containsByNif(String nif)
        {
            bool r = false;
            string query = "Select username from proprietario " +
                           "where nif=@nif ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nif", nif);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }


        public Proprietario get(String user)
        {
            String username = "";
            String password = "";
            String nif = "";
            String mail = "";
            String nome = "";
            List<int> terrenos = new List<int>();
            string query = "Select * from proprietario " +
                               "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    username = (String)reader[0];
                    password = (String)reader[1];
                    nif = "" + ((int)reader[2]);
                    mail = ((String)reader[3]);
                    nome = ((String)reader[4]);

                }

                query = "Select idTerreno from terreno " +
                                   "where nifProprietario=@nif ;";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nif", nif);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenos.Add((int)reader[0]);
                    }
                }
                int count = 0;
                query = "Select count(*) from Notificacao " +
                                   "where usernameUser=@username AND tipoUser=@tipo AND Visualizacao=0 ;";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", user);
                cmd.Parameters.AddWithValue("@tipo", "Proprietario");



                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    count = (int)reader[0];
                }
                this.CloseConnection();

                return new Proprietario(nome, mail, nif, password, username, count, terrenos);
            }
            return null;
        }





        public Proprietario getByNif(String usenif)
        {
            int usernif = Int32.Parse(usenif);
            String username = "";
            String password = "";
            String nif = "";
            String mail = "";
            String nome = "";
            string query = "Select * from proprietario " +
                               "where nif=@nif ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nif", usernif);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    username = (String)reader[0];
                    password = (String)reader[1];
                    nif = "" + ((int)reader[2]);
                    mail = ((String)reader[3]);
                    nome = ((String)reader[4]);

                }

                List<int> terrenos = new List<int>();

                query = "Select idTerreno from terreno " +
                                   "where nifProprietario=@nif ;";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nif", usernif);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenos.Add((int)reader[0]);
                    }
                }

                int count = 0;
                query = "Select count(*) from Notificacao " +
                                   "where usernameUser=@username AND tipoUser=@tipo AND Visualizacao=0 ;";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@tipo", "Proprietario");


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    count = (int)reader[0];
                }

                this.CloseConnection();

                return new Proprietario(nome, mail, nif, password, username, count, terrenos);
            }

            return null;


        }

        public String getSalt(String user)
        {
            String salt = "";

            string query = "Select salt from proprietario " +
                               "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    salt = ((String)reader[0]);

                }
                this.CloseConnection();
                return salt;
            }
            return null;
        }


        public int updatePassword(String username, String password)
        {
            String query;
            String salt = "";

            query = "UPDATE proprietario SET password=@pass WHERE username=@username ;";
            salt = getSalt(username);
            if (salt != null)
            {
                String passHashed = creatHash(password, salt);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pass", passHashed);

                if (this.OpenConnection() == true)
                {
                    int r = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    return 1;
                }
            }
            return 0;
        }


        


    }
}
