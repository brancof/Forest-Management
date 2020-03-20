using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Concelho
    {
        private int codigo;
        private String nome;
        private int area;
        private Distrito distrito;
        private TerrenoDAO terrenosCamara;
        private List<int> idsTerrenos;

       


        public Concelho()
        {
            this.codigo = 0;
            this.nome = "";
            this.area = 0;
            this.distrito = null;
            
        }

        public Concelho(int codigo, String nome, int area, Distrito dist, List<int> ids)
        {
            this.codigo = codigo;
            this.nome = nome;
            this.area = area;
            this.distrito = dist;
            this.idsTerrenos = ids;
            
        }

        public Concelho(Concelho C)
        {
            this.getCodigo();
            this.nome = C.getNome();
            this.area = C.getArea();
            this.distrito = C.getDistrito();


        }

        public String getNome() { return this.nome; }

        public int getArea() { return this.area; }

        public int getCodigo() { return this.codigo; }

        public Distrito getDistrito() { return this.distrito; }

        public void setNome(String nome) { this.nome = nome; }

        public void setArea(int area) { this.area = area; }
        public void setDistrito(Distrito dist) { this.distrito = dist; }

        


    }
}
