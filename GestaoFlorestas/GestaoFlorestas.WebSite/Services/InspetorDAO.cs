using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using System.Data.SqlClient;

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

        public void put(Inspetor i)
        {
            String query;
            if (contains(i.getUsername()))
            {
                query = "UPDATE Inspetor SET password=@password,nome=@nome WHERE username=@username ;";
            }
            else
            {
                query = "INSERT INTO Inspetor VALUES(@username,@password,@nome);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", i.getUsername());
            cmd.Parameters.AddWithValue("@password", i.getPassword());
            cmd.Parameters.AddWithValue("@nome", i.getNome());

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
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

                    if (this.OpenConnection() == true)
                    {
                        int r = cmd.ExecuteNonQuery();
                        this.CloseConnection();
                    }
                }

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
                    
                }
                this.CloseConnection();
            }

            query = "Select idTerreno from Inspecao " +
                               "where idInspetor=@username and estadoInspecao='Em espera';";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenosAInspecionar.Add((int)reader[0]);

                    }
                }
                this.CloseConnection();
            }

            return new Inspetor(username, password, nome, terrenosAInspecionar);


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
            }

            return insp;


        }
    }
}

