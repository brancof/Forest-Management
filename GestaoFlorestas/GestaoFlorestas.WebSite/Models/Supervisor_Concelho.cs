using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Supervisor_Concelho
    {
        public String nome { get; set; }
        public String username { get; set; }
        private String password;
        public String email { get; set; }
        public String concelho { get; set; }
        public int notificacoesPorLer { get; set; }

        private NotificacaoDAO notificacoes;

        public Supervisor_Concelho()
        {
            this.nome = "";
            this.username = "";
            this.password = "";
            this.email = "";
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = 0;

        }

        public Supervisor_Concelho(String nome, String username, String email, String password, String concelho, int Nnotificacao)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.concelho = concelho;
            this.email = email;
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = Nnotificacao;
        }


        public Supervisor_Concelho(Supervisor_Concelho S)
        {
            this.nome = S.getNome();
            this.username = S.getUsername();
            this.password = S.getPassword();
            this.email = S.getEmail();
            this.notificacoesPorLer = S.getNNotificacoes();
            
        }

        public String getNome() { return this.nome; }
        public String getPassword() { return this.password; }
        public String getUsername() { return this.username; }
        public String getEmail() { return this.email; }
        public String getConcelho() { return this.concelho; }
        public int getNNotificacoes() { return this.notificacoesPorLer; }

        public void setNome(String nome) { this.nome = nome; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
        public void setNNotificacoes(int n) { this.notificacoesPorLer = n; }

    }
}
