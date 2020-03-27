using GestaoFlorestas.WebSite.Exceptions;
using GestaoFlorestas.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Services
{
    public class GestaoFlorestasService
    {
        private ProprietarioDAO proprietarios;
        private TerrenoDAO terrenos;
        private InspetorDAO inspetores;
        private LocalidadeDAO locais;
        private ZonaDAO zonas;
        private TrabalhadorCamDAO trabalhadores;
        private SupervisorDAO supervisores;
        private Estado estado;

        public GestaoFlorestasService()
        {
            proprietarios = new ProprietarioDAO();
            terrenos = new TerrenoDAO();
            inspetores = new InspetorDAO();
            locais = new LocalidadeDAO();
            zonas = new ZonaDAO();
            trabalhadores = new TrabalhadorCamDAO();
            supervisores = new SupervisorDAO();
        }


        //Proprietarios
        public void registoProprietario(String username, String nome,String mail,String nif,String password) {
            
            if (!proprietarios.contains(username))
            {
                Proprietario p = new Proprietario(nome, mail, nif, password, username);
                proprietarios.put(p);
            }
            else throw new ExistingUserException();
            
        }

        public void loginProprietario(String username,String password)
        {
            if (inspetores.contains(username))
            {
                Proprietario p = proprietarios.get(username);
                if (this.proprietarios.verificarPassword(password,username))
                {
                    estado = this.estado = new Estado(1, username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }


        //Inspetores
        public void registoInspetores(String username, String nome, String mail, String password)
        {
           
            if (!inspetores.contains(username))
            {
                Inspetor i = new Inspetor(nome,username,mail,password);
                inspetores.put(i);
            }
            else throw new ExistingUserException();
        }

        public void loginInspetor(String username, String password)
        {
            if (inspetores.contains(username))
            {
                Inspetor p = inspetores.get(username);
                if (this.inspetores.verificarPassword(password, username))
                {
                    estado = this.estado = new Estado(2, username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }

        //Supervisores
        public void registoSupervisor(String nome, String username, String mail, String password, String concelho)
        {
            
            if (!supervisores.contains(username))
            {
                Supervisor_Concelho s = new Supervisor_Concelho(nome, username,mail, password,concelho);
                supervisores.put(s);
            }
            else throw new ExistingUserException();
        }

        public void loginSupervisor(String username, String password)
        {
            if (supervisores.contains(username))
            {
                Supervisor_Concelho p = supervisores.get(username);
                if (this.supervisores.verificarPassword(password,username))
                {
                    estado = this.estado = new Estado(3, username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }

        
        public void registoTrabalhadores(String nome, String username,String mail, String password, String concelho)
        {
            
            if (!trabalhadores.containsTrabalhador(username))
            {
                Trabalhador_da_Camara s = new Trabalhador_da_Camara(nome, username, mail, password, concelho);
                trabalhadores.put(s);
            }
            else throw new ExistingUserException();
        }

        public void loginTrabalhadores(String username, String password)
        {
            if (trabalhadores.containsTrabalhador(username))
            {
                //Trabalhador_da_Camara p = trabalhadores.get(username);
                if (this.trabalhadores.verificarPassword(password, username))
                {
                    estado = this.estado = new Estado(4, username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }




        //Terrenos (utilizadores que usam: proprietarios e trabalhadores da camara)
        public void limparTerreno (int idTerreno)
        {
            if (this.terrenos.contains(idTerreno))
            {
                Terreno terreno = terrenos.get(idTerreno);
                terreno.setEstadoLimpeza(true);
                terrenos.put(terreno); //muda na bd
            }
            //else --> terreno nao existe
        }




        public void trocaProprietarioTerreno(int idTerreno,String nifNovoProp)
        {
            Terreno t = this.terrenos.get(idTerreno);
            t.setNif(nifNovoProp);
            Proprietario p = this.proprietarios.getByNif(nifNovoProp);
            t.setProp(p.getUsername()); // se o cidadao como nif nao tiver registado, o username estará a null.
            terrenos.put(t);
        }




    }
}
