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
    public class SupervisoresController : ControllerBase
    {
        public SupervisoresController(GestaoFlorestasService gestaoFlorestasService)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }

        [Route("Registo")]
        [HttpPost]
        public ActionResult Registo([FromBody] string supervisor)
        {
            string[] campos = supervisor.Split(',');
            try
            {
                this.GestaoFlorestasService.registoSupervisor(campos[0], campos[1], campos[2], campos[3], campos[4]);
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
            try
            {
                this.GestaoFlorestasService.loginSupervisor(Username, Password);
            }

            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            //Response.Cookies.Append("UserCookie", "S" + Username);//colocar aqui o cookie.
            return Ok();
        }

        [Route("Trocaprop")]
        [HttpPut]
        public ActionResult TrocaProp([FromBody] string body) //body username,password,idTerreno,nifnovoprop
                                           
        {
            string[] campos = body.Split(',');
            try
            {
                this.GestaoFlorestasService.trocaProprietarioTerreno(campos[0],campos[1],Int32.Parse(campos[2]),campos[3]);
            }

            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return Ok();
        }



        [Route("Seguranca")]
        [HttpGet]
        public ActionResult ConcelhoSeguro([FromQuery] string Username,
                                           [FromQuery] string Password)
        {
            int res;
            try
            {
                res = this.GestaoFlorestasService.terrenosPorLimparConcelho(Username, Password);
            }

            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return new JsonResult(res);//se o resultado for 0 entao todos os terrenos foram limpos, caso contrario o concelho não está seguros pois tem x terrenos por limpar
        }

       //------------------------------------------------

        [Route("AgendarLimpeza")]
        [HttpPost]
        public ActionResult AgendarLimpeza([FromBody] string body) //body "username,password,idTrabalhador,idTerreno"
        {
            string[] campos = body.Split(',');

            try
            {
                this.GestaoFlorestasService.agendarLimpeza(campos[0], campos[1], campos[2], Int32.Parse(campos[3]));
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return Ok();

        }


        [Route("Agendarinspecao")]
        [HttpPost]
        public ActionResult AgendarInspecao([FromBody] string body) //body "username,password,codPostaldaZona"
        {
            string[] campos = body.Split(',');

            try
            {
                this.GestaoFlorestasService.agendarInspecao(campos[0], campos[1], campos[2]);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return Ok();

        }



    }
}