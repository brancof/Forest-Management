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
                query = "UPDATE Inspetor SET nome=@nome,email=@email, latitude=@lat, longitude=@long WHERE username=@username ;";
            }
            else
            {
                j = 1;
                query = "INSERT INTO Inspetor VALUES(@username,@password,@nome,@email,@salt,@lat,@long);";
                salt = Convert.ToBase64String(createSalt());
                password = creatHash(i.getPassword(), salt);
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", i.getUsername());
            cmd.Parameters.AddWithValue("@email", i.getMail());
            if (j == 1) cmd.Parameters.AddWithValue("@password", password);
            if (j == 1) cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@nome", i.getNome());
            cmd.Parameters.AddWithValue("@lat", i.getLatitude());
            cmd.Parameters.AddWithValue("@long", i.getLongitude());

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

        public void AtualizarCoordenadas(String username, Double latitude, Double longitude)
        {
            String query;

            query = "UPDATE Inspetor SET latitude=@lat, longitude=@long WHERE username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@lat", latitude);
            cmd.Parameters.AddWithValue("@long",longitude);
            cmd.Parameters.AddWithValue("@username", username);


            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
            }


        }


        public void putInspecaoNova(Inspecao i)
        {
            String query;
            if (!containsInspecao(i.getInspetor(), i.getTerreno()))
            {
                query = "INSERT INTO Inspecao VALUES(@idTerreno,@idInspetor,@resultado,@relatorio,@estado,@data);";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idTerreno", i.getTerreno());
                cmd.Parameters.AddWithValue("@idInspetor", i.getInspetor());
                cmd.Parameters.AddWithValue("@resultado", i.getResultado());
                cmd.Parameters.AddWithValue("@relatorio", i.getRelatorio());
                cmd.Parameters.AddWithValue("@estado", "Em espera");
                cmd.Parameters.AddWithValue("@data", i.getDate());

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

            query = "Update Inspecao Set resultado=@resultado,relatorio=@relatorio,estadoInspecao='Realizada',dataHora = @data where idInspetor = @idI AND idTerreno = @idT;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@resultado", i.getResultado());
            cmd.Parameters.AddWithValue("@relatorio", i.getRelatorio());
            cmd.Parameters.AddWithValue("@data", i.getDate());
            cmd.Parameters.AddWithValue("@idI", i.getInspetor());
            cmd.Parameters.AddWithValue("@idT", i.getTerreno());



            if (this.OpenConnection() == true)
            {
                int r = cmd.ExecuteNonQuery();
                this.CloseConnection();
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
                           "where idInspetor=@username and idTerreno=@id and estadoInspecao='Em espera';";

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
            Decimal latitude = 0;
            Decimal longitude = 0;
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
                    latitude = (Decimal)reader[5];
                    longitude = (Decimal)reader[6];

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
                int count = 0;
                query = "Select count(*) from Notificacao " +
                                   "where usernameUser=@username AND tipoUser=@tipo AND Visualizacao=0 ;";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", user);
                cmd.Parameters.AddWithValue("@tipo", "Inspetor");



                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    count = (int)reader[0];
                }
                this.CloseConnection();
                return new Inspetor(nome, username, email, password, count, Decimal.ToDouble(latitude), Decimal.ToDouble(longitude),  terrenosAInspecionar);
            }
            return null;
        }

        public List<Inspecao> getInspecoes(String inspetor, int terreno)
        {
            List<Inspecao> insp = new List<Inspecao>();
            
            string query = "Select relatorio,resultado,datahora from Inspecao " +
                               "where idTerreno=@terreno AND idInspetor=@inspetor ;";

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

        public List<Terreno> getTerrenosPendentes(String inspetor)
        {
            List<Terreno> insp = new List<Terreno>();

            Boolean estadoLimpeza = false;
            int id_Terreno = 0;
            Double area = 0;
            Decimal latitude = 0;
            Decimal longitude = 0;
            String proprietario = "";
            String cod_postal = "";
            String nif = "";
            String morada = "";
            int nivelPrioridade = 0;


            string query = "Select T.idTerreno, T.estado, T.area, T.Cod_Postal, T.Proprietario, T.latitude, T.longitude, T.nifProprietario,F.nomeFreguesia, C.nomeConcelho, C.nomeDistrito from Inspecao As I" +
                            " Join Terreno As T on T.idTerreno= I.idTerreno" +
                            " JOIN Zona as Z on Z.Cod_Postal = T.Cod_Postal " +
                            " JOIN Freguesia AS F on F.nomeFreguesia = Z.nomeFreguesia " +
                            " Join Concelho AS C on C.nomeConcelho = F.nomeConcelho " +
                            " where estadoInspecao= 'Em espera' AND idInspetor = @inspetor ;";


           SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@inspetor", inspetor);

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
                        proprietario = reader[4].ToString();
                        latitude = (Decimal)reader[5];
                        longitude = (Decimal)reader[6];
                        nif = "" + ((int)reader[7]);
                        morada = ((String)reader[8]) + ", " + ((String)reader[9]) + ", " + ((String)reader[10]);

                        insp.Add(new Terreno(estadoLimpeza, id_Terreno, area, Decimal.ToDouble(latitude), Decimal.ToDouble(longitude), proprietario, cod_postal, nif, morada, nivelPrioridade));

                    }
                }
                this.CloseConnection();
                return insp;
            }
            return null;
        }

        public String getSalt(String user)
        {
            String salt = "";

            string query = "Select salt from Inspetor " +
                               "where username=@username ;";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", user);

            if (this.OpenConnection() == true)
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    reader.Read();

                    salt = ((String)reader[0]);

                }
                this.CloseConnection();
                return salt;
            }
            return null;
        }


        public int updatePassword(String username, String password)
        {
            String query;
            String salt = "";

            query = "UPDATE Inspetor SET password=@pass WHERE username=@username ;";
            salt = getSalt(username);
            if (salt != null)
            {
                String passHashed = creatHash(password, salt);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pass", passHashed);

                if (this.OpenConnection() == true)
                {
                    int r = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    return 1;
                }
            }
            return 0;
        }
    }
}

