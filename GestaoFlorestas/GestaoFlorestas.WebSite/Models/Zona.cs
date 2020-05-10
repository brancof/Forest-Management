using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Zona
    {
        public Double nivelCritico { get; set; }
        public Double area { get; set; }
        public String codigo_Postal { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public String nomeFreguesia { get; set; }
        private LocalidadeDAO freguesia;

        public Zona()
        {
            this.nivelCritico = 0;
            this.area = 0;
            this.codigo_Postal = "";
            this.latitude = 0;
            this.longitude = 0;
            this.nomeFreguesia = "";
        }

        public Zona(Double nivelCritico, Double area, String codigo_Postal, Double latitude, Double longitude, String freguesia)
        {
            this.nivelCritico = nivelCritico;
            this.area = area;
            this.codigo_Postal = codigo_Postal;
            this.latitude = latitude;
            this.longitude = longitude;
            this.nomeFreguesia = freguesia;
            this.freguesia = new LocalidadeDAO();
        }

        public Zona(Zona Z)
        {
            this.nivelCritico = Z.getNivelCritico();
            this.area = Z.getArea();
            this.codigo_Postal = Z.getCodigo_Postal();
            this.latitude = Z.getLatitude();
            this.longitude = Z.getLongitude();
        }



        public Double getNivelCritico() { return this.nivelCritico; }

        public String getCodigo_Postal() { return this.codigo_Postal; }

        public Double getArea() { return this.area; }

        public Double getLatitude() { return this.latitude; }

        public Double getLongitude() { return this.longitude; }

        public String getFreguesia() { return this.nomeFreguesia; }

        public void setNivelCritico(Double nivelCritico) { this.nivelCritico = nivelCritico; }

        public void setCodigo_Postal(String codigo_Postal) { this.codigo_Postal = codigo_Postal; }

        public void setArea(Double area) { this.area = area; }

        public void setLatitude(Double latitude) { this.latitude = latitude; }

        public void setLongitude(Double longitude) { this.longitude = longitude; }

        public void setNomeFreguesia(String nome) { this.nomeFreguesia = nome; }

        public Freguesia getFreguesiaObject()
        {
            return this.freguesia.getFreguesia(this.nomeFreguesia);
        }

        public string getConcelho()
        {
            return this.freguesia.getFreguesia(this.nomeFreguesia).getConcelho();
        }

        public string getInspetor()
        {
            return this.freguesia.getFreguesia(this.nomeFreguesia).getCodInsp();
        }


        public int getNivelCriticoReal()
        {
            if (this.nivelCritico <= 0.02) return 1;
            if (this.nivelCritico <= 0.07) return 2;
            if (this.nivelCritico <= 0.15) return 3;
            return 4;
        }
    }
}
