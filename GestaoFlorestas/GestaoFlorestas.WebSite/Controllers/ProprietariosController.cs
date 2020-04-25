using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using GestaoFlorestas.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Web.Http.Cors;
using GestaoFlorestas.WebSite.Exceptions;
using Newtonsoft.Json;


namespace GestaoFlorestas.WebSite.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]")]
    [ApiController]
    public class ProprietariosController : ControllerBase
    {

        public ProprietariosController(GestaoFlorestasService gestaoFlorestasService)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }


        [Route("Registo")]
        [HttpPost]
        public ActionResult PostProprietario([FromBody] string proprietario)
        {
            string[] campos = proprietario.Split(',');
            try
            {
                this.GestaoFlorestasService.registoProprietario(campos[0], campos[1], campos[2], campos[3], campos[4]);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [Route("Login")]
        [HttpGet]

        public ActionResult GetL([FromQuery] string Username,
                                [FromQuery] string Password)
        {
            object p;
            try
            {
                p = this.GestaoFlorestasService.loginProprietario(Username, Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            //Response.Cookies.Append("UserCookie","P"+Username);//colocar aqui o cookie.
            return new JsonResult(p);
        }

        //-------------------------------------------------------------------------------Limpeza de terrenos-------------------------------------------------------------

        [Route("Limpeza")]
        [HttpPut]
        public ActionResult LimpaTerreno([FromBody] string body) //body: "username,password,idTerreno"
        {
            string[] campos = body.Split(',');

            int idTerreno = Int32.Parse(campos[2]);

            Proprietario p;

            try
            {
                p = this.GestaoFlorestasService.loginProprietario(campos[0], campos[1]);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            if (!p.hasTerreno(idTerreno)) return Unauthorized();


            this.GestaoFlorestasService.limparTerreno(idTerreno);


            return Ok();
        }

        //----------------------------------------------------------------Informação dos terrenos----------------------------------------------------

        [Route("Terrenos")]
        [HttpGet]
        public ActionResult GetTerrenos([FromQuery] string Username,
                                        [FromQuery] string Password)
        {
            Proprietario p;

            try
            {
                p = this.GestaoFlorestasService.loginProprietario(Username, Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            Object result = this.GestaoFlorestasService.terrenosDoProprietario(p);


            return new JsonResult(result);
        }

        [Route("Terrenos/Zona")]
        [HttpGet]
        public ActionResult GetTerrenoZona([FromQuery] string Username,
                                           [FromQuery] string Password, [FromQuery] string IdTerreno)
        {
            Proprietario p;

            int idTerreno = Int32.Parse(IdTerreno);
            try
            {
                p = this.GestaoFlorestasService.loginProprietario(Username, Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            if (!p.hasTerreno(idTerreno)) return Unauthorized();

            Zona result = this.GestaoFlorestasService.zoneTerreno(idTerreno);


            return new JsonResult(result);
        }

        [Route("Terrenos/Concelho")]
        [HttpGet]
        public ActionResult GetTerrenoConcelho([FromQuery] string Username,
                                        [FromQuery] string Password, [FromQuery] string IdTerreno)
        {
            Proprietario p;

            int idTerreno = Int32.Parse(IdTerreno);
            try
            {
                p = this.GestaoFlorestasService.loginProprietario(Username, Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            if (!p.hasTerreno(idTerreno)) return Unauthorized();

            Concelho result = this.GestaoFlorestasService.concelhoTerreno(idTerreno);


            return new JsonResult(result);
        }



        //-----------------------------------------------------------Informação Pessoal----------------------------------------------------------
        [Route("Info")]
        [HttpGet]
        public ActionResult GetInfo([FromQuery] string Username,
                                    [FromQuery] string Password)
        {
            Proprietario p;

            try
            {
                p = this.GestaoFlorestasService.loginProprietario(Username, Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

           


            return new JsonResult(p);
        }


        [Route("Info/Changes/Nome")]
        [HttpPut]
        public ActionResult ChangeName([FromBody] string body) //body: "username,password,newName"
        {
            string[] campos = body.Split(',');

            Proprietario p;

            string newName = campos[2];

            try
            {
                p = this.GestaoFlorestasService.loginProprietario(campos[0], campos[1]);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            this.GestaoFlorestasService.changeNameProp(p,newName);

            


            return Ok();
        }


        //------------------------------------notificacoes -----------------------------------------
        [Route("Notificacoes")]
        [HttpGet]
        public ActionResult GetNotifications([FromQuery] string Username,
                                             [FromQuery] string Password)
        {
            Proprietario p;

            try
            {
                p = this.GestaoFlorestasService.loginProprietario(Username, Password);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            List<Notificacao> result = this.GestaoFlorestasService.notificacoesProprietario(p);


            return new JsonResult(result);
        }

        /*[HttpPut]
        public ActionResult AtualizaNotifications([FromQuery] string Username,
                                                  [FromQuery] string Password)
        {

        }*/

    }

}
