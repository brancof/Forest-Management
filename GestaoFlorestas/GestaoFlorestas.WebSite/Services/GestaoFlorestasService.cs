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
        private NotificacaoDAO notifications;
       

        public GestaoFlorestasService()
        {
            proprietarios = new ProprietarioDAO();
            terrenos = new TerrenoDAO();
            inspetores = new InspetorDAO();
            locais = new LocalidadeDAO();
            zonas = new ZonaDAO();
            trabalhadores = new TrabalhadorCamDAO();
            supervisores = new SupervisorDAO();
            notifications = new NotificacaoDAO();
        }


        //-----------------------------------------------------------------------------Proprietarios--------------------------------------------------------------------
        public void registoProprietario(String username, String nome,String mail,String nif,String password) {
            
            if (!proprietarios.contains(username))
            {
                Proprietario p = new Proprietario(nome, mail, nif, password, username,0);
                proprietarios.put(p);
            }
            else throw new ExistingUserException();
            
        }

        public Proprietario loginProprietario(String username,String password)
        {
            if (proprietarios.contains(username))
            {
                Proprietario p = proprietarios.get(username);
                if (this.proprietarios.verificarPassword(password,username))
                {
                    return p;
                    
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

            
        }


        public void changeNameProp(Proprietario p,string newName)
        {
            p.setNome(newName);
            this.proprietarios.put(p);//atualiza a BD
        }


        public List<Terreno> terrenosDoProprietario (Proprietario p)
        {
            return p.getTerrenosObject();
        }

        public Zona zoneTerreno (int terrenoId)
        {
            Terreno t = this.terrenos.get(terrenoId);
            return t.getZoneObject();
        }

        public Concelho concelhoTerreno (int terrenoId)
        {
            Terreno t = this.terrenos.get(terrenoId);
            Zona z = t.getZoneObject();
            Freguesia f = z.getFreguesiaObject();
            return f.getConcelhoObject();
        }




        public List<Notificacao> notificacoesProprietario (Proprietario p)
        {
            return p.getNotificacoesObjects();
        }


        public void limparTerreno(int idTerreno)
        {
            if (this.terrenos.contains(idTerreno))
            {
                Terreno terreno = terrenos.get(idTerreno);
                terreno.setEstadoLimpeza(true);
                terrenos.put(terreno); //muda na bd
            }
        }


        public void visualizarNotificacoesProp(string username, string password)
        {
            if (proprietarios.contains(username))
            {
                if (this.proprietarios.verificarPassword(password, username))
                {
                    this.notifications.visualizarNotificacoes(username, "Proprietario");
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }

        //---------------------------------------------------------Inspetores---------------------------------------------------------------------
        public void registoInspetores(String username, String nome, String mail, String password)
        {
           
            if (!inspetores.contains(username))
            {
                Inspetor i = new Inspetor(nome,username,mail,password,0);
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
                   
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }



        public void realizarInspecao(string username, string password, int resultado, string relatorio, int idTerreno)
        {
            Inspetor i;
            
            if (inspetores.contains(username))
            {

                if (this.inspetores.verificarPassword(password, username))
                {
                    i = inspetores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

            if (i.containsTerreno(idTerreno))
            {
                terrenos.limpezaTerreno(idTerreno, resultado);
                Terreno t = terrenos.get(idTerreno);
                string prop = t.getProprietario();
                if (proprietarios.contains(prop))
                {
                    if (resultado == 0)
                    {
                        string conteudo = "O seu terreno com a morada " + 
                            t.getMorada() + 
                            " foi inspecionado e foi reprovado. Solicita-se que realize uma limpeza mais profunda. Pode obter mais detalhes consultando a inspeçoes realizadas aos seu terrenos."; //conteudo da notificação
                        Notificacao n = new Notificacao(conteudo, false, prop, "Proprietario", DateTime.UtcNow); //objeto representante da notificacao
                        this.notifications.put(n); //adiciona a notificacao à bd
                    }
                    else if (resultado == 1)
                    {
                        string conteudo = "O seu terreno com a morada " + t.getMorada() + " foi inspecionado e passou com distinção. Pode obter mais detalhes consultando a inspeçoes realizadas aos seu terrenos."; //conteudo da notificação
                        Notificacao n = new Notificacao(conteudo, false, prop, "Proprietario", DateTime.UtcNow); //objeto representante da notificacao
                        this.notifications.put(n); //adiciona a notificacao à bd
                    }

                }

                Inspecao inspec = new Inspecao(idTerreno, username, resultado, relatorio, DateTime.UtcNow);
                
                inspetores.AtualizarCoordenadas(username, t.getLatitude(), t.getLongitude());

                inspetores.putInspecaoRealizada(inspec);
            }
            else throw new ExistingUserException();


            


        }


        public Terreno getSugestaoInspecao(string username, string password)
        {
            Inspetor i;
            Terreno res = null;
            if (inspetores.contains(username))
            {
                if (this.inspetores.verificarPassword(password, username))
                {
                    i = inspetores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

            List<Terreno> t = inspetores.getTerrenosPendentes(i.getUsername());

            double nivelPrioridade = 0;
            int index = -1;
            for (int j = 0; j < t.Count(); j++)
            {
                Terreno te = t[j];
                Double dist;
                Double l1 = i.getLatitude() * Math.PI / 180;
                Double l2 = te.getLatitude() * Math.PI / 180;
                Double lo1 = i.getLongitude() * Math.PI / 180;
                Double lo2 = te.getLongitude() * Math.PI / 180;
                dist = 6371 * Math.Acos(Math.Cos(l1) * Math.Cos(l2) * Math.Cos(lo2 - lo1) + Math.Sin(l1) * Math.Sin(l2));
                if (dist != 0)
                {
                    int nCritico = te.nivelCritico();
                    double np = (nCritico * nCritico * nCritico) / dist;
                    if (np > nivelPrioridade)
                    {
                        index = j;
                        nivelPrioridade = np;
                    }
                }

            }

            res = t[index];



            return res;
        }

        //----------------------------------------------Supervisores----------------------------------------

        public void registoSupervisor(String nome, String username, String mail, String password, String concelho)
        {
            
            if (!supervisores.contains(username))
            {
                Supervisor_Concelho s = new Supervisor_Concelho(nome, username,mail, password,concelho,0);
                supervisores.put(s);
            }
            else throw new ExistingUserException();
        }

        public Supervisor_Concelho loginSupervisor(String username, String password)
        {
            if (supervisores.contains(username))
            {
                Supervisor_Concelho p = supervisores.get(username);
                if (this.supervisores.verificarPassword(password,username))
                {
                    return p;
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();   
        }



        public void trocaProprietarioTerreno(string username, string password, int idTerreno, String nifNovoProp)
        {
            Supervisor_Concelho p;
            if (supervisores.contains(username))
            {

                if (this.supervisores.verificarPassword(password, username))
                {
                    p = supervisores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

            Terreno t = this.terrenos.get(idTerreno);
            string concelhoTerr = t.getConcelho();

            if (concelhoTerr.Equals(p.getConcelho()))
            {

                t.setNif(nifNovoProp);
                if (this.proprietarios.containsByNif(nifNovoProp))
                {
                    Proprietario prop = this.proprietarios.getByNif(nifNovoProp);
                    t.setProp(prop.getUsername());
                }

                else t.setProp(null);
            }
            else throw new ExistingUserException();

            terrenos.put(t); //atualiza terreno na bd
        }

        public int terrenosPorLimparConcelho(string username, string password)
        {
            Supervisor_Concelho p;
            if (supervisores.contains(username))
            {
                
                if (this.supervisores.verificarPassword(password, username))
                {
                    p = supervisores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

            string concelho = p.getConcelho();

            return this.locais.numeroDeTerrenosPorLimpar(concelho);
        }



        public void agendarLimpeza (string username, string password, string usernameTrabalhador, int idTerreno)
        {
            Supervisor_Concelho p;
            if (supervisores.contains(username))
            {

                if (this.supervisores.verificarPassword(password, username))
                {
                    p = supervisores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
            Terreno t = this.terrenos.get(idTerreno);
            string concelhoTerr = t.getConcelho();

            if (concelhoTerr.Equals(p.getConcelho()))
            {
                Trabalhador_da_Camara tc = this.trabalhadores.get(usernameTrabalhador);
                if (tc.getConcelho().Equals(p.getConcelho()))
                {
                    this.trabalhadores.putLimpezas(idTerreno, usernameTrabalhador); //adiciona a limpeza pendente à bd
                    string conteudo = "Foi adicionado à sua lista de Limpezas pendentes um novo terreno que necessita de ser limpo."; //conteudo da notificação
                    Notificacao n = new Notificacao(conteudo, false, usernameTrabalhador, "Trabalhador", DateTime.UtcNow); //objeto representante da notificacao
                    this.notifications.put(n); //adiciona a notificacao à bd
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

        }


        public void agendarInspecao(string username, string password, string codPostal)
        {
            Supervisor_Concelho p;
            if (supervisores.contains(username))
            {

                if (this.supervisores.verificarPassword(password, username))
                {
                    p = supervisores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();

            Zona z = this.zonas.get(codPostal);

            string concelhoZ = z.getConcelho();

            string inspetor = z.getInspetor();

            if (concelhoZ.Equals(p.getConcelho()))
            {
                List<int> terrenos = this.zonas.getTerrenos(codPostal);
                
                for(int i = 0; i < terrenos.Count; i++)
                {
                    Inspecao insp = new Inspecao(terrenos[i], inspetor, 0, "", DateTime.UtcNow);
                    this.inspetores.putInspecaoNova(insp);
                }


            }

            else throw new ExistingUserException();

        }



        public List<Zona> zonasConcelho(string concelho)
        {
            return locais.zonasConcelho(concelho);
        }




        //---------------------------------------------Trabalhadores----------------------------------------------------------------

        public void registoTrabalhadores(String nome, String username,String mail, String password, String concelho)
        {
            
            if (!trabalhadores.containsTrabalhador(username))
            {
                Trabalhador_da_Camara s = new Trabalhador_da_Camara(nome, username, mail, password, concelho,0);
                trabalhadores.put(s);
            }
            else throw new ExistingUserException();
        }

        public Trabalhador_da_Camara loginTrabalhadores(String username, String password)
        {
            if (trabalhadores.containsTrabalhador(username))
            {
                
                if (this.trabalhadores.verificarPassword(password, username))
                {
                    return trabalhadores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }


        public Trabalhador_da_Camara limparTerrenoTrabalhador(string username,string password, int idTerreno)
        {
            Trabalhador_da_Camara tc;
            if (trabalhadores.containsTrabalhador(username))
            {

                if (this.trabalhadores.verificarPassword(password, username))
                {
                    tc = trabalhadores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
            if (tc.limpaTerreno(idTerreno))
            {
                if (this.terrenos.contains(idTerreno))
                {
                    Terreno terreno = terrenos.get(idTerreno);
                    terreno.setEstadoLimpeza(true);
                    terrenos.put(terreno); //muda na bd
                    this.trabalhadores.LimpezaRealizada(idTerreno, username);
                }
            }
            return tc;
        }

        public List<Terreno> terrenosALimpar(string username, string password)
        {
            Trabalhador_da_Camara tc;
            if (trabalhadores.containsTrabalhador(username))
            {

                if (this.trabalhadores.verificarPassword(password, username))
                {
                    tc = trabalhadores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
            return tc.getTerrenosALimparObj();
        }

        public List<Notificacao> notificacoesTrabalhador(string username, string password)
        {
            Trabalhador_da_Camara tc;
            if (trabalhadores.containsTrabalhador(username))
            {

                if (this.trabalhadores.verificarPassword(password, username))
                {
                    tc = trabalhadores.get(username);
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
            return tc.getNotificacoesObj();
        }

        public void visualizarNotificacoesTrabalhador(string username, string password)
        {
            if (trabalhadores.containsTrabalhador(username))
            {

                if (this.trabalhadores.verificarPassword(password, username))
                {
                    this.notifications.visualizarNotificacoes(username, "Trabalhador");
                }
                else throw new ExistingUserException();
            }
            else throw new ExistingUserException();
        }


        





    }
}
