using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace IncendiosAlgoritmo
{
    class Coordenadas
    {
        private Double lat;
        private Double lon;
        public const int Raio_Terra = 6371; 

        public Coordenadas()
        {
            this.lat = 0;
            this.lon = 0;
        }

        public Coordenadas(Double lat, Double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

        public Double getLatitude()
        {
            return this.lat;
        }

        public Double getLongitude()
        {
            return this.lon;
        }
        
        public void altSinal()
        {
            this.lon = this.lon * -1;
        } 


        public double convertSexagesinal2decimal (String coord)
        {
            
            double d = 0;
            double m = 0;
            double s = 0;
            Regex rgx = new Regex(@"^(40|41|42|8|08|7|07|6|06)\:\d+\:\d+(\.\d+)?(\'\')?$");
            Regex rgx2 = new Regex(@"^(40|41|42|8|08|7|07|6|06)º\d+\'\d+(\.\d+)?(\'\')?$");
            string[] parse;
            try
            {
                if (rgx.IsMatch(coord))
                {

                    parse = coord.Split(':');
                    parse[2] = parse[2].Replace('\'', '0');
                    parse[2] = parse[2].Replace('\'', '0');
                    parse[2] = parse[2].Replace('.', ',');

                    s = Convert.ToDouble(parse[2]);
                    d = Convert.ToDouble(parse[0]);
                    m = Convert.ToDouble(parse[1]);

                }
                else if (rgx2.IsMatch(coord))
                {
                    parse = coord.Split('º');
                    String[] parse1 = parse[1].Split('\'');
                    parse1[1] = parse1[1].Replace('.', ',');
                    s = Convert.ToDouble(parse1[1]);
                    d = Convert.ToDouble(parse[0]);
                    m = Convert.ToDouble(parse1[0]);
                }
                else return 0;
            }
            catch (Exception) {
                Console.WriteLine("Exception on Tranforming Coordinates\n");
                return 0;
            }
            return (d + (m / 60) + (s / 3600));
        }

        
    }
}
