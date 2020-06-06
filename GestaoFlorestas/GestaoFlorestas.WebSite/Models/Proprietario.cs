using GestaoFlorestas.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GestaoFlorestas.WebSite.Models
{
    public class Proprietario
    {
        public String nome { get; set; }
        public String mail { get; set; }
        private String nif { get; set; }
        private String password { get; set; }
        public String username { get; set; }
        public List<int> propriedades { get; set; }
        
        private TerrenoDAO terrenos;
        public int notificacoesPorLer { get; set; }
        private NotificacaoDAO notificacoes;


        public Proprietario()
        {
            this.nome = "";
            this.mail = "";
            this.nif = "";
            this.password = "";
            this.username = "";
            this.notificacoesPorLer =0;
            this.propriedades = new List<int>();
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
        }

        public Proprietario(String nome, String mail, String nif, String password, String username, int notifacacoes,List<int> props)
        {
            this.nome = nome;
            this.mail = mail;
            this.nif = nif;
            this.password = password;
            this.username = username;
            this.propriedades = props;
            this.notificacoesPorLer = notifacacoes;
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
        }

        public Proprietario(String nome, String mail, String nif, String password, String username, int not)
        {
            this.nome = nome;
            this.mail = mail;
            this.nif = nif;
            this.password = password;
            this.username = username;
            this.propriedades = new List<int>();
            this.notificacoesPorLer = not;
            this.terrenos = new TerrenoDAO();
            this.notificacoes = new NotificacaoDAO();
        }

        public Proprietario(Proprietario P)
        {
            this.nome = P.getNome();
            this.mail = P.getMail();
            this.nif = P.getNif();
            this.password = P.getPassword();
            this.username = P.getUsername();
            this.propriedades = P.getTerrenos();
            this.terrenos = new TerrenoDAO();
        }

        public String getNome() { return this.nome; }
        public String getMail() { return this.mail; }
        public String getNif() { return this.nif; }
        public String getPassword() { return this.password; }
        public String getUsername() { return this.username; }
        public int getNotificacoes() { return this.notificacoesPorLer; }
        public List<int> getTerrenos() { return this.propriedades; }

        public void setNome(String nome) { this.nome = nome; }
        public void setMail(String mail) { this.mail = mail; }
        public void setNif(String nif) { this.nif = nif; }
        public void setUsername(String username) { this.username = username; }
        public void setPassword(String password) { this.password = password; }
        public void setPropriedades(List<int> props) { this.propriedades = props; }
        public void setNotificacoes(int not) { this.notificacoesPorLer = not; }

        public bool hasTerreno(int id)
        {
            return this.propriedades.Contains(id);
        }

        public List<Terreno> getTerrenosObject()
        {
            List<Terreno> res = new List<Terreno>();
            for(int i=0; i < this.propriedades.Count(); i++)
            {
                res.Add(this.terrenos.get(this.propriedades[i]));
            }
            return res;
        }


        public List<Notificacao> getNotificacoesObjects()
        {
            return this.notificacoes.get(this.username, "Proprietario");
        }




    }
}