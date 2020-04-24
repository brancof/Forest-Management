using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Notificacao
    {
        private String id;

        private String conteudo;

        private Boolean Visualizacao;

        private String username;

        private String TipoUser;


        public Notificacao()
        {
            Guid g = Guid.NewGuid();
            this.id = g.ToString();
            this.conteudo = "";
            this.Visualizacao = false;
            this.username = "";
            this.TipoUser = "";
        }

        public Notificacao(String cont, Boolean Visual, String user, String tipo)
        {
            Guid g = Guid.NewGuid();
            this.id = g.ToString();
            this.conteudo = cont;
            this.Visualizacao = Visual;
            this.username = user;
            this.TipoUser = tipo;
        }

        public Notificacao(String id, String cont, Boolean Visual, String user, String tipo)
        {
            this.id = id;
            this.conteudo = cont;
            this.Visualizacao = Visual;
            this.username = user;
            this.TipoUser = tipo;
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

    }
}
