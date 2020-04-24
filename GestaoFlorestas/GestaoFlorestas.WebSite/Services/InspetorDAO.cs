using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace GestaoFlorestas.WebSite.Services
{
    public class InspetorDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public InspetorDAO()
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
            string query = "Select password,salt from inspetor " +
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


        public void put(Inspetor i)
        {
            int j;
            String password = "";
            String query;
            String salt = "";
            if (contains(i.getUsername()))
            {
                j = 0;
                query = "UPDATE Inspetor SET nome=@nome,email=@email WHERE username=@username ;";
            }
            else
            {
                j = 1;
                query = "INSERT INTO Inspetor VALUES(@username,@password,@nome,@email,@salt);";
                salt = Convert.ToBase64String(createSalt());
                password = creatHash(i.getPassword(), salt);
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", i.getUsername());
            cmd.Parameters.AddWithValue("@email", i.getEmail());
            if (j == 1) cmd.Parameters.AddWithValue("@password", password);
            if (j == 1) cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@nome", i.getNome());

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();

                foreach (int a in (List<int>)i.getTerrenosAInspecionar())
                {
                    if (!containsInspecao(i.getUsername(), a))
                    {
                        query = "INSERT INTO Inspecao VALUES(@idTerreno,@idInspetor,@resultado,@relatorio,@estado);";

                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@idTerreno", a);
                        cmd.Parameters.AddWithValue("@idInspetor", i.getUsername());
                        cmd.Parameters.AddWithValue("@resultado", null);
                        cmd.Parameters.AddWithValue("@relatorio", null);
                        cmd.Parameters.AddWithValue("@estado", "Em espera");

                        r = cmd.ExecuteNonQuery();
                    }

                }
            this.CloseConnection();
            }
        }

        public void putInspecaoNova(Inspecao i)
        {
            String query;
            if (!containsInspecao(i.getInspetor(), i.getTerreno()))
            {
                query = "INSERT INTO Inspecao VALUES(@idTerreno,@idInspetor,@resultado,@relatorio,@estado);";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idTerreno", i.getTerreno());
                cmd.Parameters.AddWithValue("@idInspetor", i.getInspetor());
                cmd.Parameters.AddWithValue("@resultado", i.getResultado());
                cmd.Parameters.AddWithValue("@relatorio", i.getRelatorio());
                cmd.Parameters.AddWithValue("@estado", "A espera");

                if (this.OpenConnection() == true)
                {
                    int r = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }

        }

        public void putInspecaoRealizada(Inspecao i)
        {
            String query;
            if (!containsInspecao(i.getInspetor(), i.getTerreno()))
            {
                query = "Update Inspecao Set resultado=@resultado,relatorio=@relatorio,estadoInspecao='Realizada');";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@resultado", i.getResultado());
                cmd.Parameters.AddWithValue("@relatorio", i.getRelatorio());

                if (this.OpenConnection() == true)
                {
                    int r = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }

        }


        public bool contains(String i)
        {
            bool r = false;
            string query = "Select username from Inspetor " +
                           "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", i);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public bool containsInspecao(String inspetor, int id_Terreno)
        {
            bool r = false;
            string query = "Select * from Inspecao " +
                           "where idInspetor=@username and idTerreno=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", inspetor);
            cmd.Parameters.AddWithValue("@id", id_Terreno);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }



        public Inspetor get(String user)
        {
            String username = user;
            String password = "";
            String nome = "";
            String email = "";
            List<int> terrenosAInspecionar = new List<int>();

            string query = "Select * from Inspetor " +
                               "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    password = (String)reader[1];
                    nome = (String)reader[2];
                    email = (String)reader[3];

                }

                query = "Select idTerreno from Inspecao " +
                                   "where idInspetor=@username and estadoInspecao='Em espera';";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", user);


                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenosAInspecionar.Add((int)reader[0]);

                    }
                }
                this.CloseConnection();
                return new Inspetor(nome, username, email, password, terrenosAInspecionar);
            }
            return null;
        }

        public List<Inspecao> getInspecoes(String inspetor, int terreno)
        {
            List<Inspecao> insp = new List<Inspecao>();
            
            string query = "Select relatorio,resultado,datahora from Inspecao " +
                               "where idTerreno=@terreno,idInspetor=@inspetor ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@terreno", terreno);
            cmd.Parameters.AddWithValue("@inspetor", inspetor);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        
                        insp.Add(new Inspecao(terreno, inspetor, (int)reader[1], (string)reader[0], (DateTime)reader[2]));

                    }
                }
                this.CloseConnection();
                return insp;
            }
            return null;
        }
    }
}

