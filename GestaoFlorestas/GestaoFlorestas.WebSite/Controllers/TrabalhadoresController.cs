using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;
using GestaoFlorestas.WebSite.Exceptions;


namespace GestaoFlorestas.WebSite.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]")]
    [ApiController]
    public class TrabalhadoresController : ControllerBase
    {

        public TrabalhadoresController(GestaoFlorestasService gestaoFlorestasService)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }


        [Route("Registo")]
        [HttpGet] 
        public ActionResult Registo([FromBody] string trabalhador)
        {
            string[] campos = trabalhador.Split(',');
            try
            {
                this.GestaoFlorestasService.registoTrabalhadores(campos[0], campos[1], campos[2], campos[3], campos[4]);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [Route("Login")]
        [HttpGet]

        public ActionResult Login([FromQuery] string Username,
                                  [FromQuery] string Password)
        {
            object tc;
            try
            {
                tc = this.GestaoFlorestasService.loginTrabalhadores(Username, Password);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            //Response.Cookies.Append("UserCookie", "T" + Username);//colocar aqui o cookie.
            return new JsonResult(tc);
        }



        //-------------------Limpeza de Terreno---------------------------------
        [Route("Limpeza")]
        [HttpPut]
        public ActionResult LimpaTerreno([FromBody] string body) //body: "username,password,idTerreno"
        {
            string[] campos = body.Split(',');
            object result;
            int idTerreno = Int32.Parse(campos[2]);
            try
            {
                result = this.GestaoFlorestasService.limparTerrenoTrabalhador(campos[0], campos[1], idTerreno);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return new JsonResult(result); //retorna o trabalhador com a lista de terrenos por limpar atualizada.
        }


        //------------------get terrenos pra limpeza--------------------------------

        [Route("LimpezasPendentes")]
        [HttpGet]
        public ActionResult GetTerrenos([FromQuery] string Username,
                                        [FromQuery] string Password)
        {
            object res;
            try
            {
                res = this.GestaoFlorestasService.terrenosALimpar(Username,Password);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            return new JsonResult(res);
        }


        //-------------notificacoes-----------------------------------------


        [Route("Notificacoes")]
        [HttpGet]
        public ActionResult GetNotifications([FromQuery] string Username,
                                            [FromQuery] string Password)
        {
            object result;

            try
            {
                result = this.GestaoFlorestasService.notificacoesTrabalhador(Username,Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

           


            return new JsonResult(result);
        }

        [Route("Notificacoes/Ler")]
        [HttpPut]
        public ActionResult AtualizaNotifications([FromBody] string body)//body: "username,password"
        {
            string[] campos = body.Split(',');
            try
            {
                this.GestaoFlorestasService.visualizarNotificacoesTrabalhador(campos[0], campos[1]);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return Ok();

        }
    }
}