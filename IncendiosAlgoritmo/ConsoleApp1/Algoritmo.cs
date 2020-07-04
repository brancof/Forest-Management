using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;


namespace IncendiosAlgoritmo
{
    class Algoritmo
    {
        private String WebString;
        private Connection con;


        public Algoritmo()
        {
            this.WebString = "";
            this.con = new Connection();
        }

        public Algoritmo(String X)
        {
            this.WebString = X;
        }

        public String getWebString()
        {
            return this.WebString;
        }

        public Connection getConnection()
        {
            return this.con;
        }

        //Return String file
        public String downloadFile()
        {
            DateTime utcTime = DateTime.UtcNow;
            WebClient Client = new WebClient();
            string storage = Path.Combine(Environment.CurrentDirectory,"" + utcTime.ToString("dd-MM-yyyyTH.mm.ss") + ".txt");
            Client.DownloadFile("http://www.prociv.pt/pt-PT/Paginas/export.aspx?ex=1&l=0&d=&c=&f=&t=0&n=0&e=1", storage);
            return storage;
        }

        public int loadIncendiosDiarios(String path)
        {
            int i = 0;
            int count = 0;
            List<Incendio>[] conj = new List<Incendio>[14];
            String[] concelhos = { "Esposende", "Fafe", "Guimarães", "Póvoa de Lanhoso", "Terras de Bouro", "Vieira do Minho", "Vila Nova de Famalicão", "Vila Verde", "Vizela", "Amares", "Barcelos", "Braga", "Cabeceiras de Basto", "Celorico de Basto" };
            List<String> opcoes = new List<String> { " Incêndios Rurais / Agrícola", " Incêndios Rurais / Consolidação de Rescaldo", " Incêndios Rurais / Gestão de Combustível", " Incêndios Rurais / Mato", " Incêndios Rurais / Povoamento Florestal", " Incêndios Rurais / Queima", " Incêndios Urbanos ou em Área Urbanizável / Habitacional", " Incêndios Urbanos ou em Área Urbanizável / Parque Escolar" };
            for (int j = 0; j < 14; j++)
            {
                conj[j] = new List<Incendio>();
            }

            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                using ( sr = File.OpenText(path))
                {
                    string s;
                    sr.ReadLine();
                    while ((s = sr.ReadLine()) != null)
                    {
                        try
                        {
                            String[] parse = s.Split('|');
                            //Procura pelos tipos do incidente
                            String[] tipo = parse[2].Split('/');

                            //Pode dar OutOfBound
                            String aux = tipo[1] + "/" + tipo[2];
                            

                            Regex rgx = new Regex(@"^\ Inc.ndios\ (Rurais|Urbanos ou em .rea Urbaniz.vel)\ .*$");
                            if (rgx.IsMatch(aux))
                            {
                                
                                int tam = parse.Length - 1;
                                
                                //id unico
                                Guid gg = Guid.NewGuid();
                                Coordenadas cord = new Coordenadas(Convert.ToDouble(parse[8]), Convert.ToDouble(parse[9]));
                                DateTime dateAlerta = DateTime.Now;
                                try
                                {
                                    dateAlerta = DateTime.ParseExact(parse[1], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                }catch (FormatException e) { }
                                
                                Incendio inc = new Incendio(gg.ToString(), dateAlerta, DateTime.Now, tipo[2], 0, null, cord);
                                int nConcelho = 0;
                                if (!(new Regex(@"^(?i:conclus.o)$").IsMatch(parse[3])) && new Regex(@"^(?i:braga)$").IsMatch(parse[4]))
                                {
                                    if (new Regex(@"^(?i:esposende)$").IsMatch(parse[5])) nConcelho = 0;
                                    else if (new Regex(@"^(?i:fafe)$").IsMatch(parse[5])) nConcelho = 1;
                                    else if (new Regex(@"^(?i:guimar.es)$").IsMatch(parse[5])) nConcelho = 2;
                                    else if (new Regex(@"^(?i:p.voa de lanhoso)$").IsMatch(parse[5])) nConcelho = 3;
                                    else if (new Regex(@"^(?i:terras de bouro)$").IsMatch(parse[5])) nConcelho = 4;
                                    else if (new Regex(@"^(?i:vieira do minho)$").IsMatch(parse[5])) nConcelho = 5;
                                    else if (new Regex(@"^(?i:vila nova de famalic.o)$").IsMatch(parse[5])) nConcelho = 6;
                                    else if (new Regex(@"^(?i:vila verde)$").IsMatch(parse[5])) nConcelho = 7;
                                    else if (new Regex(@"^(?i:vizela)$").IsMatch(parse[5])) nConcelho = 8;
                                    else if (new Regex(@"^(?i:amares)$").IsMatch(parse[5])) nConcelho = 9;
                                    else if (new Regex(@"^(?i:barcelos)$").IsMatch(parse[5])) nConcelho = 10;
                                    else if (new Regex(@"^(?i:braga)$").IsMatch(parse[5])) nConcelho = 11;
                                    else if (new Regex(@"^(?i:cabeceiras de basto)$").IsMatch(parse[5])) nConcelho = 12;
                                    else if (new Regex(@"^(?i:celorico de basto)$").IsMatch(parse[5])) nConcelho = 13;

                                    conj[nConcelho].Add(inc);
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }
                for (int j = 0; j < 14; j++)
                {
                    if (conj[j].Count != 0)
                    {
                        List<Zona> zc = this.con.getZonaConcelho(concelhos[j]);
                        conj[j] = procura(zc, conj[j]);
                    }
                }
                //insert na bd
                for (int j = 0; j < 14; j++)
                {
                    if (conj[j].Count != 0)
                    {
                        count += conj[j].Count;
                        this.con.putIncendios(conj[j]);
                    }
                }
                
            }
            return count;
        }


        public void loadIncendiosAntigos(String path)
        {
            int i = 100000000;
            List<Incendio>[] conj = new List<Incendio> [14];
            String[] concelhos = { "Esposende","Fafe", "Guimarães", "Póvoa de Lanhoso", "Terras de Bouro", "Vieira do Minho", "Vila Nova de Famalicão", "Vila Verde","Vizela","Amares","Barcelos","Braga","Cabeceiras de Basto","Celorico de Basto"};
            //Nao sei se é necessária esta inicialização
            for(int j = 0; j<14; j++)
            {
                conj[j] = new List<Incendio>();
            }

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    sr.ReadLine();
                    while ((s = sr.ReadLine()) != null)
                    {
                        String[] parse = s.Split(',');
                        if (!parse[1].Equals("Falso Alarme"))
                        {
                            int tam = parse.Length - 1;

                            //Transforma as coordenadas em decimal
                            Coordenadas cord = new Coordenadas();
                            Double lat = cord.convertSexagesinal2decimal(parse[tam - 13]);
                            Double lo = cord.convertSexagesinal2decimal(parse[tam - 12]);
                            //Se correr tudo bem na transformação entra, senão ignora
                            if (lat != 0 && lo != 0)
                            {
                                cord = new Coordenadas(lat, lo);
                                cord.altSinal();
                                String area = parse[tam - 7].Replace('.', ',');
                                Guid gg = Guid.NewGuid();
                                DateTime dateAlerta = DateTime.Now;
                                DateTime datextincao = DateTime.Now;
                                try
                                {
                                    dateAlerta = DateTime.ParseExact(parse[tam - 11] + " " + parse[tam - 10], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                }catch(FormatException e){}
                                try
                                {
                                    datextincao = DateTime.ParseExact(parse[tam - 9] + " " + parse[tam - 8], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                                catch (FormatException e) { }

                                Incendio inc = new Incendio(gg.ToString(),dateAlerta, datextincao, parse[1], Convert.ToDouble(area), null, cord);
                               
                                int nConcelho = 0;

                                if (parse[3].Equals("Esposende") || parse[3].Equals("Esposende".ToUpper())) nConcelho = 0;
                                else if (parse[3].Equals("Fafe") || parse[3].Equals("Fafe".ToUpper())) nConcelho = 1;
                                else if (parse[3].Equals("Guimarães") || parse[3].Equals("Guimarães".ToUpper())) nConcelho = 2;
                                else if (parse[3].Equals("Póvoa de Lanhoso") || parse[3].Equals("Póvoa de Lanhoso".ToUpper()) || parse[3].Equals("Póvoa De Lanhoso")) nConcelho = 3;
                                else if (parse[3].Equals("Terras de Bouro") || parse[3].Equals("Terras de Bouro".ToUpper()) || parse[3].Equals("Terras De Bouro")) nConcelho = 4;
                                else if (parse[3].Equals("Vieira do Minho") || parse[3].Equals("Vieira do Minho".ToUpper()) || parse[3].Equals("Vieira Do Minho")) nConcelho = 5;
                                else if (parse[3].Equals("Vila Nova de Famalicão") || parse[3].Equals("Vila Nova de Famalicão".ToUpper()) || parse[3].Equals("Vila Nova De Famalicão")) nConcelho = 6;
                                else if (parse[3].Equals("Vila Verde") || parse[3].Equals("Vila Verde".ToUpper())) nConcelho = 7;
                                else if (parse[3].Equals("Vizela".ToUpper()) || parse[3].Equals("Vizela")) nConcelho = 8;
                                else if (parse[3].Equals("Amares") || parse[3].Equals("Amares".ToUpper())) nConcelho = 9;
                                else if (parse[3].Equals("Barcelos") || parse[3].Equals("Barcelos".ToUpper())) nConcelho = 10;
                                else if (parse[3].Equals("Braga") || parse[3].Equals("Braga".ToUpper())) nConcelho = 11;
                                else if (parse[3].Equals("Cabeceiras de Basto") || parse[3].Equals("Cabeceiras de Basto".ToUpper()) || parse[3].Equals("Cabeceiras De Basto")) nConcelho = 12;
                                else if (parse[3].Equals("Celorico de Basto") || parse[3].Equals("Celorico de Basto".ToUpper()) || parse[3].Equals("Celorico De Basto")) nConcelho = 13;

                                conj[nConcelho].Add(inc);
                                i++;
                               
                            }
                        }
                    }
                }
                for (int j = 0; j < 14; j++)
                {
                    if( conj[j].Count != 0)
                    {
                         List<Zona> zc = this.con.getZonaConcelho(concelhos[j]);
                         conj[j] = procura(zc, conj[j]);
                    }
                }

                //insert na bd
                for (int j = 0; j < 14; j++)
                {
                    if (conj[j].Count != 0)
                    {
                        this.con.putIncendios(conj[j]);
                    }
                }
            }
        }


        public double distancia(Coordenadas c1, Coordenadas c2)
        {
            Double result;
            Double l1 = c1.getLatitude() * Math.PI / 180;
            Double l2 = c2.getLatitude() * Math.PI / 180;
            Double lo1 = c1.getLongitude() * Math.PI / 180;
            Double lo2 = c2.getLongitude() * Math.PI / 180;

            result = 6371 * Math.Acos(Math.Cos(l1) * Math.Cos(l2) * Math.Cos(lo2 - lo1) + Math.Sin(l1) * Math.Sin(l2));
            return result;
        }


        public List<Incendio> procura(List<Zona> z, List<Incendio> inc)
        {
            for(int i = 0; i < z.Count; i++)
            {
                String cod = z[i].getCodPostal();
                Coordenadas cz = new Coordenadas(z[i].getLatitude(), z[i].getLongitude());
                for (int j = 0; j < inc.Count; j++)
                {
                    double r = 0;
                    Coordenadas cInc = inc[j].getCoordenadas();
                    Double min = inc[j].getDistancia();
                    if ((r = distancia(cz,cInc)) < min)
                    {
                        inc[j].setDistancia(r);
                        inc[j].setZona(cod);
                    }
                }
            }
            return inc;
        }

        public String List2InsertIncendios(List<Incendio> i)
        {
            String ins = "INSERT INTO incendio VALUES ";

            for(int j = 0; j < i.Count; j++)
            {
                String lat = Convert.ToString(i[j].getCoordenadas().getLatitude()).Replace(',','.');
                String lo = Convert.ToString(i[j].getCoordenadas().getLongitude()).Replace(',', '.');
                ins += "('" + i[j].getId() + "'," + i[j].getDataAlerta() + "," + i[j].getDataExtincao() + ",'" + i[j].getTipo() + "'," + i[j].getArea() + ",'" + i[j].getZona() + "'," + lat + "," + lo + i[j].getDistancia() + "),\n ";
            }

            return ins;
        }

        public void alteraNivelCritico()
        {
            con.atualizaNivelCritico();
        }
    }
}
