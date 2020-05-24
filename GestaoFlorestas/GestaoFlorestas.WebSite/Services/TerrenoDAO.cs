using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using System.Data.SqlClient;

namespace GestaoFlorestas.WebSite.Services
{
    public class TerrenoDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public TerrenoDAO()
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

        public void put(Terreno t)
        {
            String query;
            if (contains(t.getId_Terreno()))
            {
                query = "UPDATE Terreno SET estado=@estado,Area=@area,Proprietario=@pro,latitude=@lat,longitude=@lon,nifProprietario=@nif WHERE idTerreno=@id ;";
            }
            else
            {
                query = "INSERT INTO Terreno VALUES(@id,@estado,@area,@cod,@pro,@lat,@lon,@nif);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            if(t.getEstadoLimpeza()) cmd.Parameters.AddWithValue("@estado", 1);
            else cmd.Parameters.AddWithValue("@estado", 0);

            cmd.Parameters.AddWithValue("@area", t.getArea());
            cmd.Parameters.AddWithValue("@nif", Int32.Parse(t.getNif()));
            cmd.Parameters.AddWithValue("@pro", t.getProprietario());
            cmd.Parameters.AddWithValue("@lat", t.getLatitude());
            cmd.Parameters.AddWithValue("@lon", t.getLongitude());
            cmd.Parameters.AddWithValue("@id", t.getId_Terreno());
            cmd.Parameters.AddWithValue("@cod", t.getCod_Postal());
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }



        }

        public bool contains(int t)
        {
            bool r = false;
            string query = "Select idTerreno from Terreno " +
                           "where idTerreno=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", t);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public Terreno get(int id)
        {
            Boolean estadoLimpeza = false;
            int id_Terreno = 0;
            Double area = 0;
            Decimal latitude = 0;
            Decimal longitude = 0;
            String proprietario = "";
            String cod_postal = "";
            String nif = "";
            String morada = "";
            List<Inspecao> inspecoes = new List<Inspecao>();
          

            string query = "Select T.idTerreno,T.estado,T.Area,T.Cod_Postal,T.Proprietario,T.latitude,T.longitude,T.nifProprietario,F.nomeFreguesia, C.nomeConcelho, C.nomeDistrito from Terreno As T "
                            +"JOIN Zona as Z on Z.Cod_Postal = T.Cod_Postal "
                            +"JOIN Freguesia AS F on F.nomeFreguesia = Z.nomeFreguesia "
                            +"Join Concelho AS C on C.nomeConcelho = F.nomeConcelho " 
                            + "where idTerreno=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    id_Terreno = (int)reader[0];
                    estadoLimpeza = ((int)reader[1]) != 0;
                    area = ((int)reader[2]);
                    cod_postal = ((String)reader[3]);
                    proprietario = ((String)reader[4]);
                    latitude = (Decimal)reader[5];
                    longitude = (Decimal)reader[6];
                    nif = "" + ((int)reader[7]);
                    morada = ((String)reader[8]) + ", " + ((String)reader[9]) + ", " + ((String)reader[10]);




                }

                query = "Select idInspetor,resultado,relatorio,dataHora from Inspecao " +
                                   "where idTerreno=@id and estadoInspecao='Realizada' ;";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id_Terreno);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        inspecoes.Add(new Inspecao(id, (String)reader[0], (int)reader[1], (String)reader[2], (DateTime)reader[3]));

                    }
                }
                this.CloseConnection();
                return new Terreno(estadoLimpeza, id_Terreno, area, Decimal.ToDouble(latitude), Decimal.ToDouble(longitude), proprietario, cod_postal, nif, morada, inspecoes);

            }
            return null;

        }


        public List<Terreno> getTerrenosNifConcelho(int Nif, string Concelho)
        {
            Boolean estadoLimpeza = false;
            int id_Terreno = 0;
            Double area = 0;
            Decimal latitude = 0;
            Decimal longitude = 0;
            String proprietario = "";
            String cod_postal = "";
            String nif = "";
            String morada = "";
            List<Terreno> res = new List<Terreno>();
            List<Inspecao> inspecoes = new List<Inspecao>();


            string query = "Select T.idTerreno,T.estado,T.Area,T.Cod_Postal,T.Proprietario,T.latitude,T.longitude,T.nifProprietario,F.nomeFreguesia, C.nomeConcelho, C.nomeDistrito from Terreno As T "
                            + "JOIN Zona as Z on Z.Cod_Postal = T.Cod_Postal "
                            + "JOIN Freguesia AS F on F.nomeFreguesia = Z.nomeFreguesia "
                            + "Join Concelho AS C on C.nomeConcelho = F.nomeConcelho "
                            + "where nifProprietario = @nif AND C.nomeConcelho = @conc ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nif", Nif);
            cmd.Parameters.AddWithValue("@conc", Concelho);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        id_Terreno = (int)reader[0];
                        estadoLimpeza = ((int)reader[1]) != 0;
                        area = ((int)reader[2]);
                        cod_postal = ((String)reader[3]);
                        proprietario = ((String)reader[4]);
                        latitude = (Decimal)reader[5];
                        longitude = (Decimal)reader[6];
                        nif = "" + ((int)reader[7]);
                        morada = ((String)reader[8]) + ", " + ((String)reader[9]) + ", " + ((String)reader[10]);
                        res.Add(new Terreno(estadoLimpeza, id_Terreno, area, Decimal.ToDouble(latitude), Decimal.ToDouble(longitude), proprietario, cod_postal, nif, morada, inspecoes));
                    }

                }

                this.CloseConnection();
                return res;

            }
            return null;

        }


        public void limpezaTerreno (int id, int estado)
        {
            String query = "UPDATE Terreno SET estado=@estado WHERE idTerreno=@id ;";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@estado", estado);

            cmd.Parameters.AddWithValue("@id", id);
            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        
    }
}
