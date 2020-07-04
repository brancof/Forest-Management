using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace IncendiosAlgoritmo
{
    class Connection
    {

        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public Connection()
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



        public bool containsConcelho(String cod)
        {
            bool r = false;
            string query = "Select *  from Concelho " +
                           "where nomeConcelho=@cod ;";

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

        

        public List<Zona> getZonaConcelho(String concelho)
        {

            List<Zona> lz = new List<Zona>();

            string query = "Select Cod_Postal, latitude, longitude, Z.nIncendios from [dbo].[Zona] As Z "
                           + "join [dbo].[Freguesia] AS F on F.nomeFreguesia = Z.nomeFreguesia "
                           + "join [dbo].[Concelho] AS C on F.nomeConcelho = C.nomeConcelho "
                           + "where C.nomeConcelho = @conc";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@conc", concelho);

            if (this.OpenConnection() == true)
            {
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Decimal lat = (decimal)reader[1];
                            Decimal lo = (decimal)reader[2];

                            Zona z = new Zona((String)reader[0], Decimal.ToDouble(lat), Decimal.ToDouble(lo), (int)reader[3]);
                            lz.Add(z);
                        }
                    }
                }
                catch (SqlException e) { this.CloseConnection(); return null; };
                this.CloseConnection();
            }
            return lz;
        }

        public Double getNivelcritico(String Cod)
        {

            Double nivel = 0;

            string query = "Select nivelCritico from [dbo].[Zona] As Z "
                           + "where Z.Cod_Postal = @cod";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", Cod);

            if (this.OpenConnection() == true)
            {

               using (SqlDataReader reader = cmd.ExecuteReader())
               {

                   reader.Read();
                   nivel = (Double)reader[0];
                        
               }
                this.CloseConnection();
            }
            return nivel;
        }


        public Double updateNivelcritico(String Cod, Double nivel)
        {

            string query = "update Zona set nivelCritico=@nivel "
                           + "where Cod_Postal = @cod";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", Cod);
            cmd.Parameters.AddWithValue("@nivel", nivel);

            if (this.OpenConnection() == true)
            {
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
            return nivel;
        }


        public bool containsIncendio(String id)
        {
            bool r = false;
            string query = "Select *  from incendio " +
                           "where idIncendio=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            var value = cmd.ExecuteScalar();
            if (value != null) r = true;
            else r = false;
            
            return r;
        }


        public void putIncendios(List<Incendio> inc)
        {
            if (this.OpenConnection() == true)
            {
                for (int i = 0; i < inc.Count; i++)
                {
                    String query;
                    if (containsIncendio(inc[i].getId()))
                    {
                        query = "UPDATE incendio SET datahora_alerta=@dAlerta, datahora_extincao=@dextincao, tipo=@tipo, Area=@area, Cod_Postal=@cod, latitude=@lat, longitude=@lo where idIncendio=@id ;";
                    }
                    else
                    {
                        query = "INSERT INTO incendio VALUES(@id, @dAlerta, @dextincao, @tipo, @area, @cod, @lat, @lo);";
                    }
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", inc[i].getId());
                    
                    cmd.Parameters.AddWithValue("@dAlerta", inc[i].getDataAlerta());
                    cmd.Parameters.AddWithValue("@dextincao", inc[i].getDataExtincao());
                    cmd.Parameters.AddWithValue("@tipo", inc[i].getTipo());
                    cmd.Parameters.AddWithValue("@area", inc[i].getArea());
                    cmd.Parameters.AddWithValue("@cod", inc[i].getZona());
                    String lat = Convert.ToString(inc[i].getCoordenadas().getLatitude()).Replace(',', '.');
                    String lo = Convert.ToString(inc[i].getCoordenadas().getLongitude()).Replace(',', '.');
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lo", lo);


                    int r = cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
            }

        }

        
        public void atualizaNivelCritico()
        {
            List<Zona> lz = new List<Zona>();

            string query = "SELECT cod_Postal, Z.nIncendios, C.nIncendios FROM Zona AS Z " +
                            "JOIN Freguesia AS F on F.nomeFreguesia = Z.nomeFreguesia " +
                            "JOIN Concelho AS C on C.nomeConcelho = F.nomeConcelho ";

            SqlCommand cmd = new SqlCommand(query, con);
            if (this.OpenConnection() == true)
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int ZonaInc = (int)reader[1];
                        int ConcelhoInc = (int)reader[2];
                        if (ConcelhoInc != 0 && ZonaInc != 0)
                        {
                            Double nivel = (Double)ZonaInc / (Double)ConcelhoInc;
                            Zona z = new Zona((String)reader[0], 0, 0, nivel, (int)reader[1]);
                            lz.Add(z);
                        }
                    }
                }
                query = "UPDATE zona Set nivelCritico=@nivel where Cod_Postal=@cod";
                for (int i = 0; i < lz.Count; i++)
                {
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@nivel", lz[i].getNivel());
                    cmd.Parameters.AddWithValue("@cod", lz[i].getCodPostal());
                    int r = cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
            }
            
        }




        
    }
}
