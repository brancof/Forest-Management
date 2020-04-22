using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Concelho
    {
        public int codigo { get; set; }
        public String nome { get; set; }
        public int area { get; set; }
        public Distrito distrito { get; set; }
        private TerrenoDAO terrenosCamara;
        public List<int> idsTerrenos { get; set; }




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
