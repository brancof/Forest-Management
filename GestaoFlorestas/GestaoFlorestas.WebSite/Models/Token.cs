using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Token
    {
        public String username { get; set; }

        private String token { get; set; }

        public String TipoUser { get; set; }

        public DateTime dataEmissao { get; set; }


        public Token()
        {
            this.token = "";
            this.username = "";
            this.TipoUser = "";
            this.dataEmissao = new DateTime();
        }

        public Token (String user, String tipo, String tok, DateTime date)
        {
     
            this.token = tok;
            this.username = user;
            this.TipoUser = tipo;
            this.dataEmissao = date;
        }

        public String getToken()
        {
            return this.token;
        }

        public String getUsername()
        {
            return this.username;
        }

        public String getTipoUser()
        {
            return this.TipoUser;
        }

        public DateTime getDataEmissao()
        {
            return this.dataEmissao;
        }

    }
}
}
