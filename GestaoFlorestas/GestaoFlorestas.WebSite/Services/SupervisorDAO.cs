using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using System.Data.SqlClient;

namespace GestaoFlorestas.WebSite.Services
{
    public class SupervisorDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public SupervisorDAO()
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

        public void put(Supervisor_Concelho s)
        {
            String query;
            if (contains(s.getUsername()))
            {
                query = "UPDATE Supervisor SET password=@password,nome=@nome,Concelho=@con WHERE username=@username ;";
            }
            else
            {
                query = "INSERT INTO Supervisor (username,password,nome,Concelho) VALUES(@username,@password,@nome,@con);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", s.getUsername());
            cmd.Parameters.AddWithValue("@password", s.getPassword());
            cmd.Parameters.AddWithValue("@con", s.getConcelho());
            cmd.Parameters.AddWithValue("@nome", s.getNome());
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }

        }

        public bool contains(String p)
        {
            bool r = false;
            string query = "Select username from Supervisor " +
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


        public Supervisor_Concelho get(String user)
        {
            String username = user;
            String password = "";
            String concelho = "";
            String nome = "";
            string query = "Select * from Supervisor " +
                               "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();
                    
                    password = (String)reader[1];
                    concelho = ((String)reader[3]);
                    nome = ((String)reader[2]);
                    
                }
                this.CloseConnection();
            }

            
            return new Supervisor_Concelho(nome, username, password, concelho);

        }

    }
}
