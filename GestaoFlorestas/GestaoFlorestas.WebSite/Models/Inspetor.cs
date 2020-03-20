using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Inspetor
    {
        private String nome;
        private String username;
        private String password;
        private List<int> terrenosAInspecionar;
        private TerrenoDAO terrenos;

        public Inspetor()
        {
            this.nome = "";
            this.username = "";
            this.password = "";
            this.terrenosAInspecionar = null;
            this.terrenos = new TerrenoDAO();

        }

        public Inspetor(String nome, String username, String password, List<int> terrenosAInspec)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.terrenosAInspecionar = terrenosAInspec;
            this.terrenos = new TerrenoDAO();
            
        }

        public Inspetor(String nome, String username, String password)
        {
            this.nome = nome;
            this.username = username;
            this.password = password;
            this.terrenosAInspecionar = null;
            this.terrenos = new TerrenoDAO();

        }

        public Inspetor(Inspetor I)
        {
            this.nome = I.getNome();
            this.username = I.getUsername();
            this.password = I.getPassword();
            this.terrenos = new TerrenoDAO();
            this.terrenosAInspecionar = I.getTerrenosAInspecionar();
        }

        public String getNome() { return this.nome; }
        public String getPassword() { return this.password; }
        public String getUsername() { return this.username; }
        public List<int> getTerrenosAInspecionar() { return this.terrenosAInspecionar; }
        public void setNome(String nome) { this.nome = nome; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
        public void setTerrenosAInspecionar(List<int> terr) { this.terrenosAInspecionar = terr; }
    }
}
