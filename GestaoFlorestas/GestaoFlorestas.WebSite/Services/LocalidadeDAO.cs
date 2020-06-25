using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GestaoFlorestas.WebSite.Models;

namespace GestaoFlorestas.WebSite.Services
{
    public class LocalidadeDAO
    {
        private String server;
        private String database;
        private String userId;
        private String pass;
        private SqlConnection con;

        public LocalidadeDAO()
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

        public void putDistrito(Distrito d)
        {
            String query;
            if (containsDistrito(d.getNome()))
            {
                query = "UPDATE Distrito SET codDistrito=@cod,Area=@area,habitantes=@hab WHERE nomeDistrito=@nome ;";
            }
            else
            {
                query = "INSERT INTO Distrito VALUES(@cod,@nome,@area,@hab);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", d.getCodigo());
            cmd.Parameters.AddWithValue("@area", d.getArea());
            cmd.Parameters.AddWithValue("@nome", d.getNome());
            cmd.Parameters.AddWithValue("@hab", d.getHabitantes());

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void putConcelho(Concelho c)
        {
            String query;
            if (containsConcelho(c.getNome()))
            {
                query = "UPDATE Concelho SET codConcelho=@cod,Area=@area,nomeDistrito=@distrito,nif=@nif WHERE nomeConcelho=@nome ;";
            }
            else
            {
                query = "INSERT INTO Concelho VALUES(@cod,@nome,@area,@distrito,@nif);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", c.getCodigo());
            cmd.Parameters.AddWithValue("@area", c.getArea());
            cmd.Parameters.AddWithValue("@nome", c.getNome());
            //cmd.Parameters.AddWithValue("@distrito", c.getDistrito());
            cmd.Parameters.AddWithValue("@nif", c.getNif());

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void putFreguesia(Freguesia f)
        {
            String query;
            if (containsFreguesia(f.getNome()))
            {
                query = "UPDATE Freguesia SET codFreguesia=@cod,Area=@area,nomeConcelho=@concelho,Inspetor=@insp WHERE nomeFreguesia=@nome ;";
            }
            else
            {
                query = "INSERT INTO Freguesia VALUES(@cod,@nome,@area,@concelho,@insp);";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cod", f.getCodigo());
            cmd.Parameters.AddWithValue("@area", f.getArea());
            cmd.Parameters.AddWithValue("@nome", f.getNome());
            cmd.Parameters.AddWithValue("@concelho", f.getConcelho());
            cmd.Parameters.AddWithValue("@insp", f.getCodInsp());

            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }


        public bool containsDistrito(String nomeDistrito)
        {
            bool r = false;
            string query = "Select nomeDistrito from Distrito " +
                           "where nomeDistrito=@distrito ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@distrito", nomeDistrito);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public bool containsConcelho(String nomeConcelho)
        {
            bool r = false;
            string query = "Select nomeConcelho from Concelho " +
                           "where nomeConcelho=@concelho ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@concelho", nomeConcelho);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }

        public bool containsFreguesia(String nomeFreguesia)
        {
            bool r = false;
            string query = "Select nomeFreguesia from Freguesia " +
                           "where nomeFreguesia=@freguesia ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@freguesia", nomeFreguesia);

            if (this.OpenConnection() == true)
            {
                var value = cmd.ExecuteScalar();
                if (value != null) r = true;
                else r = false;
                this.CloseConnection();
            }
            return r;
        }



        public Distrito getDistrito(String distrito)
        {
            String nome = distrito;
            int codDistrito = 0;
            int area = 0;
            int habitantes = 0;
       
            string query = "Select * from Distrito " +
                               "where nomeDistrito=@dis ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@dis", distrito);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();
                    
                    codDistrito = (int)reader[0];
                    area = (int)reader[2];
                    habitantes = (int)reader[3];

                }
                this.CloseConnection();
                return new Distrito(codDistrito, nome, area, habitantes);
            }
            return null;
            
        }

        public Concelho getConcelho(String concelho)
        {
            String nome = concelho;
            Decimal longitude = 0;
            Decimal latitude = 0;
            int codConcelho = 0;
            int area = 0;
            String nomeDistrito = "";
            int nif = 0;
            List<int> terrenosCamara = new List<int>();
            string query = "Select * from Concelho " +
                               "where nomeConcelho=@con ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@con", concelho);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    codConcelho = (int)reader[0];
                    area = (int)reader[2];
                    nomeDistrito = (String)reader[3];
                    nif = (int)reader[4];
                    latitude = (Decimal)reader[6];
                    longitude = (Decimal)reader[7];

                }

              

                query = "Select idTerreno from Terreno " +
                                   "where Proprietario=@con;";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@con", concelho);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        terrenosCamara.Add((int)reader[0]);

                    }
                }
                this.CloseConnection();
                return new Concelho(codConcelho, nome, area, nif, Decimal.ToDouble(latitude), Decimal.ToDouble(longitude), terrenosCamara);
            }
            return null;

        }


        public Freguesia getFreguesia(String freguesia)
        {
            String nome = freguesia;
            int codFreguesia = 0;
            int area = 0;
            String nomeConcelho = "";
            List<String> zonas = new List<String>();
            String inspetor = "";

            string query = "Select * from Freguesia " +
                               "where nomeFreguesia=@fre ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@fre", freguesia);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    codFreguesia = (int)reader[0];
                    area = (int)reader[2];
                    nomeConcelho = (String)reader[3];
                    inspetor = (String)reader[4];

                }

                query = "Select Cod_Postal from Zona " +
                                   "where nomeFreguesia=@fre;";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@fre", freguesia);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        zonas.Add((String)reader[0]);

                    }
                }
                this.CloseConnection();
                return new Freguesia(codFreguesia, area, nome, nomeConcelho, zonas, inspetor);
            }
            return null;
        }


        public int numeroDeTerrenosPorLimpar(String concelho)
        {
            int res = 0;

            string query = "Select count(*) from Terreno As T"
                           + " JOIN Zona as Z on Z.Cod_Postal = T.Cod_Postal"
                           + " JOIN Freguesia AS F on F.nomeFreguesia = Z.nomeFreguesia"
                           + " where T.estado = 0 AND F.nomeConcelho = @conc ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@conc", concelho);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();
                    res = (int)reader[0];

                }
                this.CloseConnection();
            }

            return res;
        }


        public List<Zona> zonasConcelho(String concelho)
        {
            List<Zona> res = new List<Zona>();
            List<Zona> aux = new List<Zona>();

            string query = "SELECT z.Cod_Postal, z.Area, z.latitude, z.longitude, z.nomeFreguesia, z.nivelCritico, z.nIncendios FROM Zona as z"
                            + " JOIN Freguesia AS f on f.nomeFreguesia = z.nomeFreguesia"
                            + " where nomeConcelho = @conc"
                            + " order by z.nivelCritico desc;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@conc", concelho);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    { 
                        Zona z = new Zona(Decimal.ToDouble((Decimal)reader[5]), (int)reader[1], (String) reader[0],
                                         Decimal.ToDouble((Decimal)reader[2]), Decimal.ToDouble((Decimal)reader[3]), (String) reader[4]);
                        res.Add(z);
                    }

                }

                query = "SELECT z.Cod_Postal, z.Area, z.latitude, z.longitude, z.nomeFreguesia, z.nivelCritico, z.nIncendios FROM Zona as z"
                        + " JOIN Freguesia AS f on f.nomeFreguesia = z.nomeFreguesia"
                        + " JOIN Terreno AS t on t.Cod_Postal = z.Cod_Postal"
                        + " JOIN Inspecao AS i on t.idTerreno = i.idTerreno"
                        + " where nomeConcelho = @conc AND i.estadoInspecao = 'Em espera';";
                SqlCommand c = new SqlCommand(query, con);
                c.Parameters.AddWithValue("@conc", concelho);
                using (SqlDataReader reader = c.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Zona z = new Zona(Decimal.ToDouble((Decimal)reader[5]), (int)reader[1], (String)reader[0],
                                         Decimal.ToDouble((Decimal)reader[2]), Decimal.ToDouble((Decimal)reader[3]), (String)reader[4]);
                        aux.Add(z);
                    }
                }

                this.CloseConnection();

                for (int i = 0; i<res.Count(); i++)
                {
                    Boolean b = false;
                    for (int j = 0; j < aux.Count() && b==false ; j++)
                    {
                        if (res[i].getCodigo_Postal() == aux[j].getCodigo_Postal()) b = true;
                    }
                    res[i].setEmInspecao(b);
                }
                    
            }

            return res;
        }

        public List<Trabalhador_da_Camara> trabalhadoresConcelho(String concelho)
        {
            List<Trabalhador_da_Camara> res = new List<Trabalhador_da_Camara>();
            List<String> aux = new List<String>();
            String username = "";
            String password = "";
            String nomeConcelho = "";
            String nome = "";
            String email = "";
            Decimal lat = 0;
            Decimal longi = 0;

            string query = "SELECT * FROM Trabalhador as T"
                            + " where nomeConcelho = @conc;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@conc", concelho);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        username = (String)reader[0];
                        password = (String)reader[1];
                        nomeConcelho = ((String)reader[3]);
                        nome = ((String)reader[2]);
                        email = (String)reader[4];
                        lat = (Decimal)reader[6];
                        longi = (Decimal)reader[7];

                        Trabalhador_da_Camara tc = new Trabalhador_da_Camara(nome, username, email, password, nomeConcelho, -1,Decimal.ToDouble(lat), Decimal.ToDouble(longi), new List<int>());
                        res.Add(tc);
                    }

                }

                query = "SELECT T.username FROM Trabalhador as T"
                        + " JOIN LimpezasPendentes as L on L.Trabalhador = T.username"
                        + " where nomeConcelho = @conc;";
                
                SqlCommand c = new SqlCommand(query, con);
                c.Parameters.AddWithValue("@conc", concelho);
                using (SqlDataReader reader = c.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        username = (String)reader[0];
                        aux.Add(username);
                    }

                }
                for (int i = 0; i < res.Count(); i++)
                {
                    for (int j = 0; j < aux.Count(); j++)
                    {
                        if (res[i].getUsername() == aux[j])
                        {
                            res[i].idsLimpezasPendentes.Add(1);
                            aux.RemoveAt(j); j--;
                        }
                    }
                }


                this.CloseConnection();
            }

            return res;
        }


    }
}
