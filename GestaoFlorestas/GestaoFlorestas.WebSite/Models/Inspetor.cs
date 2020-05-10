using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Inspetor
    {
        public String nome { get; set; }
        public String username { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public List<int> terrenosAInspecionar { get; set; }
        private TerrenoDAO terrenos;
        public int notificacoesPorLer { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }

        private NotificacaoDAO notificacoes;

        public Inspetor()
        {
            this.nome = "";
            this.username = "";
            this.password = "";
            this.email = "";
            this.terrenosAInspecionar = null;
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = 0;

        }

        public Inspetor(String nome, String username, String email, String password, int notificacoes, Double latitude, Double longitude, List<int> terrenosAInspec)
        {
            this.nome = nome;
            this.username = username;
            this.email = email;
            this.password = password;
            this.latitude = latitude;
            this.longitude = longitude;
            this.terrenosAInspecionar = terrenosAInspec;
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = notificacoes;

        }

        public Inspetor(String nome, String username, String email, String password, int notificacoes, Double latitude, Double longitude)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.email = email;
            this.latitude = latitude;
            this.longitude = longitude;
            this.terrenosAInspecionar = new List<int>();
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = notificacoes;

        }

        public Inspetor(String nome, String username, String email, String password, int notificacoes) { 
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.email = email;
            
            this.terrenosAInspecionar = new List<int>();
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = notificacoes;

        }

        public Inspetor(Inspetor I)
        {
            this.nome = I.getNome();
            this.username = I.getUsername();
            this.password = I.getPassword();
            this.email = I.getEmail();
            this.latitude = I.getLatitude();
            this.longitude = I.getLongitude();
            this.terrenos = new TerrenoDAO();
            this.terrenosAInspecionar = I.getTerrenosAInspecionar();
            this.notificacoes = new NotificacaoDAO();
            this.notificacoesPorLer = I.getNNotificacoes();
        }

        public String getNome() { return this.nome; }
        public String getPassword() { return this.password; }
        public String getUsername() { return this.username; }
        public String getEmail() { return this.email; }
        public List<int> getTerrenosAInspecionar() { return this.terrenosAInspecionar; }
        public int getNNotificacoes() { return this.notificacoesPorLer; }

        public void setNome(String nome) { this.nome = nome; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
        public void setTerrenosAInspecionar(List<int> terr) { this.terrenosAInspecionar = terr; }
        public void setNNotificacoes(int n) { this.notificacoesPorLer = n; }

        public Double getLatitude() { return this.latitude; }
        public Double getLongitude() { return this.longitude; }


        public bool containsTerreno(int idTerreno)
        {
            return this.terrenosAInspecionar.Contains(idTerreno);
        }
    }
}
