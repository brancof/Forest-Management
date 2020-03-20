using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Inspecao
    {
        private int terreno;
        private String inspetor;
        private int resultado;
        private String relatorio;
        private DateTime dataHora;



        public Inspecao()
        {
            this.terreno = 0;
            this.inspetor = "";
            this.resultado = 0;
            this.relatorio = "";
            this.dataHora = new DateTime();
        }

        public Inspecao(int terreno, String inspetor, int resultado, String relatorio, DateTime date)
        {
            this.terreno = terreno;
            this.inspetor = inspetor;
            this.resultado = resultado;
            this.relatorio = relatorio;
            this.dataHora = date;
        }

        public Inspecao(Inspecao I)
        {
            this.terreno = I.getTerreno();
            this.inspetor = I.getInspetor();
            this.resultado = I.getResultado();
            this.relatorio = I.getRelatorio();
            this.dataHora = I.getDate();
        }

        public int getTerreno() { return this.terreno; }

        public String getInspetor() { return this.inspetor; }

        public int getResultado() { return this.resultado; }
        public String getRelatorio() { return this.relatorio; }

        public DateTime getDate() { return this.dataHora; }
        public void setResultado(int resultado) { this.resultado = resultado; }
        public void setRelatorio(String relatorio) { this.relatorio = relatorio; }

    }
}
