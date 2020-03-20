using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using System.Data.SqlClient;

namespace GestaoFlorestas.WebSite.Services
{
    public class ZonaDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public ZonaDAO()
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

        public void put(Zona z)
        {
            String query;
            if (contains(z.getCodigo_Postal()))
            {
                query = "UPDATE Zona SET Area=@area,latitude=@lat,longitude=@lon,nomeFreguesia=@freg,nivelCritico=@nivel WHERE Cod_Postal=@cod ;";
            }
            else
            {
                query = "INSERT INTO Zona VALUES(@cod,@area,@lat,@lon,@freg,@nivel);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", z.getCodigo_Postal());
            cmd.Parameters.AddWithValue("@area", z.getArea());
            cmd.Parameters.AddWithValue("@lat", z.getLatitude());
            cmd.Parameters.AddWithValue("@lon", z.getLongitude());
            cmd.Parameters.AddWithValue("@freg", z.getFreguesia());
            cmd.Parameters.AddWithValue("@nivel", z.getNivelCritico());
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }

        }

        public bool contains(String cod)
        {
            bool r = false;
            string query = "Select Cod_Postal from Zona " +
                           "where Cod_Postal=@cod ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", cod);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }


        public Zona get(String codigoP)
        {
            String codigoPostal = codigoP;
            int area = 0;
            Double latitude = 0;
            Double longitude = 0;
            String nomeFreguesia = "";
            Double nivelCritico = 0;
            string query = "Select * from Zona " +
                               "where Cod_Postal=@cod ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", codigoP);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    area = (int)reader[1];
                    latitude = ((Double)reader[2]);
                    longitude = ((Double)reader[3]);
                    nomeFreguesia = ((String)reader[4]);
                    nivelCritico = ((Double)reader[5]);

                }
                this.CloseConnection();
            }

           return new Zona(nivelCritico, area, codigoPostal, latitude, longitude, nomeFreguesia);

        }

    }
}
