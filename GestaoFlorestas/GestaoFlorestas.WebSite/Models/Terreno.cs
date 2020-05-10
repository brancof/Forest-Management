using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Terreno
    {
        public Boolean estadoLimpeza { get; set; }
        public int id_Terreno { get; set; }
        public Double area { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public String proprietario { get; set; }
        public String cod_postal { get; set; }
        private ZonaDAO zona;
        public List<Inspecao> inspecoes { get; set; }
        public String nif { get; set; }
        public string morada { get; set; }
        private int nivelPrioridade;

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

        public Terreno(Boolean estadoLimpeza, int id_Terreno, Double area, Double latitude, Double longitude, String pro, String codPostal, String nif, String mor,List<Inspecao>inspecs)
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
            this.morada = mor;
        }

        public Terreno(Boolean estadoLimpeza, int id_Terreno, Double area, Double latitude, Double longitude, String pro, String codPostal, String nif, String mor, int prioridade)
        {
            this.estadoLimpeza = estadoLimpeza;
            this.id_Terreno = id_Terreno;
            this.area = area;
            this.latitude = latitude;
            this.longitude = longitude;
            this.cod_postal = codPostal;
            this.inspecoes = new List<Inspecao>();
            this.nif = nif;
            this.zona = new ZonaDAO();
            this.proprietario = pro;
            this.morada = mor;
            this.nivelPrioridade = prioridade;
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

        public int getNivelPrioridade() { return this.nivelPrioridade; }

        public void setEstadoLimpeza(Boolean estadoLimpeza) { this.estadoLimpeza = estadoLimpeza; }

        public void setId_Terreno(int id_Terreno) { this.id_Terreno = id_Terreno; }

        public void setArea(Double area) { this.area = area; }

        public void setLatitude(Double latitude) { this.latitude = latitude; }

        public void setLongitude(Double longitude) { this.longitude = longitude; }

        public void setInspecoes(List<Inspecao>inspecs) { this.inspecoes = inspecs; }

        public void setNif(String novoNif) { this.nif = novoNif; }

        public void setProp(String prop) { this.proprietario = prop; }

        public void setMorada(String m) { this.morada = m; }

        public Zona getZoneObject()
        {
            return this.zona.get(this.cod_postal);
        }


        public string getConcelho()
        {
            string[] campos = this.morada.Split(", ");
            return campos[1];
        }

        public string getMorada()
        {
            return this.morada;
        }

        public int nivelCritico()
        {
            int res = 0;
            Zona z = zona.get(this.cod_postal);
            res = z.getNivelCriticoReal();
            return res;
        }
        
    }

}
