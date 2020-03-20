using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Trabalhador_da_Camara
    {
        private String nome;
        private String username;
        private String password;
        private List<int> idsLimpezasPendentes;
        private TerrenoDAO terrenosALimpar;
        private String concelho;
        

        public Trabalhador_da_Camara()
        {
            this.nome = "";
            this.username = "";
            this.password = "";
            this.concelho = "";
        }


        public Trabalhador_da_Camara(String nome, String username, String password, String concel, List<int> lp)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.concelho = concel;
            this.idsLimpezasPendentes = lp;
        }

        public Trabalhador_da_Camara(String nome, String username, String password, String concel)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.concelho = concel;
            this.idsLimpezasPendentes = null;
        }

        public Trabalhador_da_Camara(Trabalhador_da_Camara t)
        {
            this.nome = t.getNome();
            this.username = t.getUsername();
            this.password = t.getPassword();
            this.concelho = t.getConcelho();
        }

        public String getNome() { return this.nome; }
        public String getUsername() { return this.username; }
        public String getPassword() { return this.password; }
        public String getConcelho() { return this.concelho; }

        public List<int> getTerrenosPendentes() { return this.idsLimpezasPendentes; } 

        public void setNome(String nome) { this.nome = nome; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
        public void setConcelho(String concel) { this.concelho = password; }
    }
}
