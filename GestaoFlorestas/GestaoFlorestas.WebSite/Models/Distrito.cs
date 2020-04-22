using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Distrito
    {
        public int codigo { get; set; }
        public String nome { get; set; }
        public int area { get; set; }
        public int habitantes { get; set; }


        public Distrito()
        {
            this.codigo = 0;
            this.nome = "";
            this.area = 0;
            
            this.habitantes = 0;

        }

        public Distrito(int cod, String nome, int area, int habitantes)
        {
            this.codigo = cod;
            this.nome = nome;
            this.area = area;
            this.habitantes = habitantes;
        }

        public Distrito(Distrito D)
        {
            this.nome = D.getNome();
            this.area = D.getArea();
            this.codigo = D.getCodigo();
            this.habitantes = D.getHabitantes();
        }

        public String getNome() { return this.nome; }

        public int getArea() { return this.area; }

        public int getCodigo() { return this.codigo; }

        public int getHabitantes() { return this.habitantes; }

        public void setNome(String nome) { this.nome = nome; }

        public void setArea(int area) { this.area = area; }

        public void setHabitantes(int habitantes) { this.habitantes = habitantes; }
    }
}
