using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Notificacao
    {
        public String id { get; set; }

        public String conteudo { get; set; }

        public Boolean Visualizacao { get; set; }

        public String username { get; set; }

        public String TipoUser { get; set; }

        public DateTime dataEmissao { get; set; }


        public Notificacao()
        {
            Guid g = Guid.NewGuid();
            this.id = g.ToString();
            this.conteudo = "";
            this.Visualizacao = false;
            this.username = "";
            this.TipoUser = "";
            this.dataEmissao = new DateTime();
        }

        public Notificacao(String cont, Boolean Visual, String user, String tipo, DateTime date)
        {
            Guid g = Guid.NewGuid();
            this.id = g.ToString();
            this.conteudo = cont;
            this.Visualizacao = Visual;
            this.username = user;
            this.TipoUser = tipo;
            this.dataEmissao = date;
        }

        public Notificacao(String id, String cont, Boolean Visual, String user, String tipo, DateTime date)
        {
            this.id = id;
            this.conteudo = cont;
            this.Visualizacao = Visual;
            this.username = user;
            this.TipoUser = tipo;
            this.dataEmissao = date;
        }

        public String idUnico()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }

        public String getId()
        {
            return this.id;
        }

        public String getConteudo()
        {
            return this.conteudo;
        }

        public String getUsername()
        {
            return this.username;
        }

        public String getTipoUser()
        {
            return this.TipoUser;
        }

        public Boolean getVisualizacao()
        {
            return this.Visualizacao;
        }

        public DateTime getDataEmissao()
        {
            return this.dataEmissao;
        }

    }
}
