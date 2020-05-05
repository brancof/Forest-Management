using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GestaoFlorestas.WebSite.Models;
using System.Security.Cryptography;

namespace GestaoFlorestas.WebSite.Services
{
    public class TrabalhadorCamDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public TrabalhadorCamDAO()
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
            string query = "Select password,salt from trabalhador " +
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

        public void put(Trabalhador_da_Camara tp)
        {
            int i;
            String password = "";
            String query;
            String salt = "";
            if (containsTrabalhador(tp.getUsername()))
            {
                i = 0;
                query = "UPDATE Trabalhador SET nome=@nome,nomeConcelho=@con,email=@email WHERE username=@username ;";
            }
            else
            {
                i = 1;
                query = "INSERT INTO Trabalhador (username,password,nome,nomeConcelho,email,salt) VALUES(@username,@password,@nome,@con,@email,@salt);";
                salt = Convert.ToBase64String(createSalt());
                password = creatHash(tp.getPassword(), salt);
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", tp.getUsername());
            if (i == 1) cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@con", tp.getConcelho());
            cmd.Parameters.AddWithValue("@email", tp.getEmail());
            cmd.Parameters.AddWithValue("@nome", tp.getNome());
            if (i == 1) cmd.Parameters.AddWithValue("@salt", salt);
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();


                foreach (int a in (List<int>)tp.getTerrenosPendentes())
                {
                    if (!containsLimpeza(a, tp.getUsername()))
                    {
                        query = "INSERT INTO LimpezaPendentes VALUES(@idTerreno,@trabalhador);";

                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@idTerreno", a);
                        cmd.Parameters.AddWithValue("@trabalhador", tp.getUsername());

                        r = cmd.ExecuteNonQuery();
   
                    }

                }
                this.CloseConnection();
            }

        }


        public void putLimpezas(int id, String trabalhador)
        {
            if (this.OpenConnection() == true)
            {
                String query;
                if (!containsLimpeza(id, trabalhador))
                {
                    query = "INSERT INTO LimpezaPendentes VALUES(@idTerreno,@trabalhador);";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@idTerreno", id);
                    cmd.Parameters.AddWithValue("@trabalhador", trabalhador);

                    int r = cmd.ExecuteNonQuery();

                }
                this.CloseConnection();
            }
        }
        

        
        
        public bool containsTrabalhador(String p)
        {
            bool r = false;
            string query = "Select username from Trabalhador " +
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

        public bool containsLimpeza(int terreno, String trabalhador)
        {
            bool r = false;
            string query = "Select * from Trabalhador " +
                           "where Trabalhador=@username AND idTerreno=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", trabalhador);
            cmd.Parameters.AddWithValue("@id", terreno);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public Trabalhador_da_Camara get(String user)
        {
            String username = "";
            String password = "";
            String nomeConcelho = "";
            String nome = "";
            String email = "";
            string query = "Select * from Trabalhador " +
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
                    nomeConcelho = ((String)reader[3]);
                    nome = ((String)reader[2]);
                    email = (String)reader[4];

                }

                List<int> terrenos = new List<int>();

                query = "Select idTerreno from LimpezaPendentes " +
                                   "where Trabalhador=@tp ;";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@tp", user);

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
                cmd.Parameters.AddWithValue("@tipo", "Trabalhador");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    count = (int)reader[0];
                }
                this.CloseConnection();
                return new Trabalhador_da_Camara(nome, username, email, password, nomeConcelho, count, terrenos);
            }
            return null;
        }

        public bool LimpezaRealizada(int terreno, String trabalhador)
        {
            if (containsLimpeza(terreno, trabalhador))
            {
                String query = "DELETE FROM LimpezaPendentes WHERE idTerreno=@idTerreno,Trabalhador=@trabalhador;";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idTerreno", terreno);
                cmd.Parameters.AddWithValue("@trabalhador", trabalhador);

                if (this.OpenConnection() == true)
                {
                    int r = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }

            }
            else return false;
            return true;
        }

    }
}
