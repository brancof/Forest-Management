using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Freguesia
    {
        private int codigo;
        private int area;
        private String nome;
        private String concelho;
        private List<String> codigosZonas;
        private ZonaDAO zonas;
        private String codInspetorResponsavel;
        private InspetorDAO inspetor;


        public Freguesia()
        {
            this.codigo = 0;
            this.area = 0;
            this.nome = "";
            this.concelho = "";
            this.zonas = new ZonaDAO();
            this.codInspetorResponsavel = "";
            this.codigosZonas = new List<string>();
            this.inspetor = new InspetorDAO(); ;
        }

        public Freguesia(int codigo, int area, String nome, String concelho, List<String> codsZonas, String codInspetorRespons)
        {
            this.area = area;
            this.nome = nome;
            this.concelho = concelho;
            this.codigosZonas = codsZonas;
            this.codigo = codigo;
            this.codInspetorResponsavel = codInspetorRespons;
        }


        public Freguesia(Freguesia F)
        {
            this.area = F.getArea();
            this.nome = F.getNome();
            this.concelho = F.getConcelho();
            this.codigosZonas = F.getCodigosZonas();
            this.codigo = F.getCodigo();
            this.codInspetorResponsavel = F.getCodInsp();
        }


        public int getArea() { return this.area; }

        public int getCodigo() { return this.codigo; }

        public String getNome() { return this.nome; }

        public String getConcelho() { return this.concelho; }

        public List<String> getCodigosZonas() { return this.codigosZonas; }


        public String getCodInsp() { return this.codInspetorResponsavel; }


        public void setArea(int area) { this.area = area; }


        public void setNome(String nome) { this.nome = nome; }

        public void setCodigosZonas(List<String>cods) { this.codigosZonas=cods; }

        public void setCodInsp (String codInsp) { this.codInspetorResponsavel = codInsp;}

    }
}
