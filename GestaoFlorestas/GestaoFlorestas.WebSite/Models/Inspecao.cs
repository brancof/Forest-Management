using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Inspecao
    {
        public String id { get; set; }
        public int terreno { get; set; }
        public String inspetor { get; set; }
        public int resultado { get; set; }
        public String relatorio { get; set; }
        public DateTime dataHora { get; set; }



        public Inspecao()
        {
            Guid g = Guid.NewGuid();
            this.id = g.ToString();
            this.terreno = 0;
            this.inspetor = "";
            this.resultado = 0;
            this.relatorio = "";
            this.dataHora = new DateTime();
        }

        public Inspecao(int terreno, String inspetor, int resultado, String relatorio, DateTime date)
        {
            Guid g = Guid.NewGuid();
            this.id = g.ToString();
            this.terreno = terreno;
            this.inspetor = inspetor;
            this.resultado = resultado;
            this.relatorio = relatorio;
            this.dataHora = date;
        }

        public Inspecao(string id, int terreno, String inspetor, int resultado, String relatorio, DateTime date)
        {
            this.id = id;
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
        public string getId() { return this.id; }

        public DateTime getDate() { return this.dataHora; }
        public void setResultado(int resultado) { this.resultado = resultado; }
        public void setRelatorio(String relatorio) { this.relatorio = relatorio; }

    }
}
