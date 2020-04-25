﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using GestaoFlorestas.WebSite.Models;
using System.Security.Cryptography;

namespace GestaoFlorestas.WebSite.Services
{
    public class NotificacaoDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public NotificacaoDAO()
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

        public void put(Notificacao n)
        {
            int i;
            String query;
            if (contains(n.getId()))
            {
                i = 0;
                query = "UPDATE notificaco SET conteudo=@conteudo,visualizacao=@vis,tipoUser=@tipo WHERE idNotificacao=@id ;";
            }
            else
            {
                i = 1;
                query = "INSERT INTO notificacao (idNotificacao, conteudo, Visualizacao, usernameUser, TipoUser) VALUES(@id,@conteudo,@vis,@user,@tipo);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", n.getId());
            if (i == 1) cmd.Parameters.AddWithValue("@user", n.getUsername());
            cmd.Parameters.AddWithValue("@conteudo",n.getConteudo());
            cmd.Parameters.AddWithValue("@vis", n.getVisualizacao()?1:0);
            cmd.Parameters.AddWithValue("@tipo", n.getTipoUser());
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void putList(List<Notificacao> ln)
        {
            if (this.OpenConnection() == true)
            {
                foreach (Notificacao n in ln)
                {
                    int i;
                    String query;
                    if (containsSConnection(n.getId()))
                    {
                        i = 0;
                        query = "UPDATE notificaco SET conteudo=@conteudo,visualizacao=@vis,tipoUser=@tipo WHERE idNotificacao=@id ;";
                    }
                    else
                    {
                        i = 1;
                        query = "INSERT INTO notificacao (idNotificacao, conteudo, Visualizacao, usernameUser, TipoUser) VALUES(@id,@conteudo,@vis,@user,@tipo); ";
                    }
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", n.getId());
                    if (i == 1) cmd.Parameters.AddWithValue("@user", n.getUsername());
                    cmd.Parameters.AddWithValue("@conteudo", n.getConteudo());
                    cmd.Parameters.AddWithValue("@vis", n.getVisualizacao() ? 1 : 0);
                    cmd.Parameters.AddWithValue("@tipo", n.getTipoUser());

                    int r = cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
            }
        }


        public bool contains(String id)
        {
            bool r = false;
            string query = "Select idNotificacao from notificacao " +
                           "where idNotificacao=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public bool containsSConnection(String id)
        {
            bool r = false;
            string query = "Select idNotificacao from notificacao " +
                           "where idNotificacao=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            var value = cmd.ExecuteScalar();
            if (value != null) r = true;
            else r = false;

            return r;
        }

        public List<Notificacao> get(String user, String tipoUser)
        {
            String id = "";
            String conteudo = "";
            Boolean Visualizacao = false;
            
            List<Notificacao> l = new List<Notificacao>();
            string query = "Select idNotificacao, conteudo, visualizacao from Notificacao " +
                               "where usernameUser=@username AND tipoUser=@tipo ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);
            cmd.Parameters.AddWithValue("@tipo", tipoUser);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        id = (String)reader[0];
                        conteudo = (String)reader[1];
                        Visualizacao = ((int)reader[2]) != 0;
                        l.Add(new Notificacao(id, conteudo, Visualizacao, user, tipoUser));
                    }

                }
                this.CloseConnection();
                return l;
            }
            return null;
        }

        public int countNVisualizadas(String user, String tipoUser)
        {
            int count = 0;
            string query = "Select count(*) from Notificacao " +
                               "where usernameUser=@username AND tipoUser=@tipo AND Visualizacao=0 ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);
            cmd.Parameters.AddWithValue("@tipo", tipoUser);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    count = (int)reader[0];
                }
                this.CloseConnection();
                
            }
            return count;
        }

    }
}
