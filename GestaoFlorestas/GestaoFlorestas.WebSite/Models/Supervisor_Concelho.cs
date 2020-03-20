using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Supervisor_Concelho
    {
        private String nome;
        private String username;
        private String password;
        private String concelho;

        public Supervisor_Concelho()
        {
            this.nome = "";
            this.username = "";
            this.password = "";
        }

        public Supervisor_Concelho(String nome, String username, String password, String concelho)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.concelho = concelho;
        }


        public Supervisor_Concelho(Supervisor_Concelho S)
        {
            this.nome = S.getNome();
            this.username = S.getUsername();
            this.password = S.getPassword();
        }

        public String getNome() { return this.nome; }
        public String getPassword() { return this.password; }
        public String getUsername() { return this.username; }

        public String getConcelho() { return this.concelho; }

        public void setNome(String nome) { this.nome = nome; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
    }
}
