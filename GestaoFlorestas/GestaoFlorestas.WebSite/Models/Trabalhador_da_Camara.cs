using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Trabalhador_da_Camara
    {
        public String nome { get; set; }
        public String username { get; set; }
        private String password;
        public String email { get; set; }
        public List<int> idsLimpezasPendentes { get; set; }

        public Double latitude { get; set; }
        public Double longitude { get; set; }

        private TerrenoDAO terrenosALimpar;
        public String concelho { get; set; }
        public int notificacoesPorLer { get; set; }

        private NotificacaoDAO notificacoes;



        public Trabalhador_da_Camara()
        {
            this.nome = "";
            this.username = "";
            this.password = "";
            this.concelho = "";
            this.email = "";
            this.terrenosALimpar = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = 0;
        }


        public Trabalhador_da_Camara(String nome, String username, String email, String password, String concel, int Nnotificacao,double lat, double longi, List<int> lp)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.concelho = concel;
            this.idsLimpezasPendentes = lp;
            this.email = email;
            this.terrenosALimpar = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = Nnotificacao;
            this.latitude = lat;
            this.longitude = longi;
        }

        public Trabalhador_da_Camara(String nome, String username, String email, String password, String concel, int Nnotificacao)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.concelho = concel;
            this.email = email;
            this.terrenosALimpar = new TerrenoDAO();
            this.idsLimpezasPendentes = new List<int>();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = Nnotificacao;
        }

        public Trabalhador_da_Camara(Trabalhador_da_Camara t)
        {
            this.nome = t.getNome();
            this.username = t.getUsername();
            this.password = t.getPassword();
            this.concelho = t.getConcelho();
            this.email = t.getMail();
            this.notificacoesPorLer = t.getNNotificacoes();
        }

        public String getNome() { return this.nome; }
        public String getUsername() { return this.username; }
        public String getPassword() { return this.password; }
        public String getConcelho() { return this.concelho; }
        public String getMail() { return this.email; }
        public List<int> getTerrenosPendentes() { return this.idsLimpezasPendentes; }
        public int getNNotificacoes() { return this.notificacoesPorLer; }
        public double getLatitude() { return this.latitude; }
        public double getLongitude() { return this.longitude; }

        public void setNome(String nome) { this.nome = nome; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
        public void setConcelho(String concel) { this.concelho = password; }



        public bool hasLimpeza(int idTerreno)
        {
            return this.terrenosALimpar.contains(idTerreno);
        }

        public List<Terreno> getTerrenosALimparObj()
        {
            List<Terreno> result = new List<Terreno>();
            for (int i = 0; i < this.idsLimpezasPendentes.Count(); i++)
            {
                result.Add(this.terrenosALimpar.get(this.idsLimpezasPendentes[i]));
            }
            return result;
        }

        public void setNNotificacoes(int n) { this.notificacoesPorLer = n; }

        public bool limpaTerreno(int idTerreno)
        { 
                return this.idsLimpezasPendentes.Remove(idTerreno);
        }

        public List<Notificacao> getNotificacoesObj()
        {
            return this.notificacoes.get(this.username, "Trabalhador");
        }
    }
}
