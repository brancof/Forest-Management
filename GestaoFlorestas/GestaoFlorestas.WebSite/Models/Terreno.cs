using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Terreno
    {
        private Boolean estadoLimpeza;
        private int id_Terreno;
        private Double area;
        private Double latitude;
        private Double longitude;
        private String proprietario;
        private String cod_postal;
        private ZonaDAO zona;
        private List<Inspecao> inspecoes;
        private String nif;

        public Terreno()
        {
            this.id_Terreno = 0;
            this.area = 0;
            this.latitude = 0;
            this.longitude = 0;
            this.proprietario = "";
            this.cod_postal = "";
            this.nif = "";
            this.inspecoes = null;
            this.zona = new ZonaDAO();
        }

        public Terreno(Boolean estadoLimpeza, int id_Terreno, Double area, Double latitude, Double longitude, String pro, String codPostal, String nif, List<Inspecao>inspecs)
        {
            this.estadoLimpeza = estadoLimpeza;
            this.id_Terreno = id_Terreno;
            this.area = area;
            this.latitude = latitude;
            this.longitude = longitude;
            this.cod_postal = codPostal;
            this.inspecoes = inspecs;
            this.nif = nif;
            this.zona = new ZonaDAO();
            this.proprietario = pro;
        }

        public Terreno(Terreno t)
        {
            this.estadoLimpeza = t.getEstadoLimpeza();
            this.id_Terreno = t.getId_Terreno();
            this.area = t.getArea();
            this.latitude = t.getLatitude();
            this.longitude = t.getLongitude();
            this.cod_postal = t.getCod_Postal();
            this.inspecoes = t.getInspecoes();
            this.nif = t.getNif();
            this.zona = new ZonaDAO();
            this.proprietario = t.getProprietario();
        }


        public Boolean getEstadoLimpeza() { return this.estadoLimpeza; }

        public int getId_Terreno() { return this.id_Terreno; }

        public Double getArea() { return this.area; }

        public String getCod_Postal() { return this.cod_postal; }

        public Double getLatitude() { return this.latitude; }

        public Double getLongitude() { return this.longitude; }

        public List<Inspecao> getInspecoes() { return this.inspecoes; }

        public String getNif() { return this.nif; }

        public String getProprietario() { return this.proprietario; }

        public void setEstadoLimpeza(Boolean estadoLimpeza) { this.estadoLimpeza = estadoLimpeza; }

        public void setId_Terreno(int id_Terreno) { this.id_Terreno = id_Terreno; }

        public void setArea(Double area) { this.area = area; }

        public void setLatitude(Double latitude) { this.latitude = latitude; }

        public void setLongitude(Double longitude) { this.longitude = longitude; }

        public void setInspecoes(List<Inspecao>inspecs) { this.inspecoes = inspecs; }

       

        
    }

}
