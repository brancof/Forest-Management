using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using GestaoFlorestas.WebSite.Models;

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

            public void put(Proprietario p)
            {
                String query;
                if (contains(p.getUsername()))
                {
                    query = "UPDATE proprietario SET password=@password,nif=@nif,email=@email,nome=@nome WHERE username=@username ;";
                }
                else
                {
                    query = "INSERT INTO proprietario (username,password,nif,email,nome) VALUES(@username,@password,@nif,@email,@nome);";
                }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", p.getUsername());
                cmd.Parameters.AddWithValue("@password", p.getPassword());
                cmd.Parameters.AddWithValue("@nif", Int32.Parse(p.getNif()));
                cmd.Parameters.AddWithValue("@email", p.getMail());
                cmd.Parameters.AddWithValue("@nome", p.getNome());
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


        public Proprietario get(String user)
        {
            String username="";
            String password="";
            String nif = "";
            String mail="";
            String nome="";
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
                        nif = ""+((int)reader[2]);
                        mail = ((String)reader[3]);
                        nome = ((String)reader[4]);
                    
                }
                this.CloseConnection();
            }

            List<int> terrenos = new List<int>();

            query = "Select idTerreno from terreno " +
                               "where nifProprietario=@nif ;";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nif", nif);
            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenos.Add((int)reader[0]);
                    }
                }
                this.CloseConnection();
            }
            return new Proprietario(nome,mail,nif,password,username,terrenos);

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
                this.CloseConnection();
            }

            List<int> terrenos = new List<int>();

            query = "Select idTerreno from terreno " +
                               "where nifProprietario=@nif ;";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nif", nif);
            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenos.Add((int)reader[0]);
                    }
                }
                this.CloseConnection();
            }
            return new Proprietario(nome, mail, nif, password, username, terrenos);

        }

    }
}
