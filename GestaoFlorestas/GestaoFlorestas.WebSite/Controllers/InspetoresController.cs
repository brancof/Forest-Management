using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Exceptions;
using GestaoFlorestas.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace GestaoFlorestas.WebSite.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]")]
    [ApiController]
    public class InspetoresController : ControllerBase
    {
        public InspetoresController(GestaoFlorestasService gestaoFlorestasService)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }


        [Route("Registo")]
        [HttpPost] //Put ou Post???
        public ActionResult PostInspetor([FromBody] string inspetor)
        {
            string[] campos = inspetor.Split(',');
            try
            {
                this.GestaoFlorestasService.registoInspetores(campos[0], campos[1], campos[2], campos[3]);
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
                this.GestaoFlorestasService.loginInspetor(Username, Password);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            //Response.Cookies.Append("UserCookie", "I" + Username);//colocar aqui o cookie.
            return Ok();
        }

        [Route("Realizarinspecao")]
        [HttpPut]
        public ActionResult RealizarInspecao([FromBody] string body) // body "username|password|resultado|relatorio|idTerreno
        {

            string[] campos = body.Split('|');
            double res = 0;

            try
            {
                res = this.GestaoFlorestasService.realizarInspecao(campos[0], campos[1], Int32.Parse(campos[2]), campos[3], Int32.Parse(campos[4]));
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            //Response.Cookies.Append("UserCookie", "I" + Username);//colocar aqui o cookie.
            return new JsonResult(res);
        }

        [Route("Sugestaoinspecao")]
        [HttpGet]
        public ActionResult Inspecao([FromBody] string body) // body "username|password"
        {
            object result = null;

            string[] campos = body.Split('|');
            try
            {
                result = this.GestaoFlorestasService.getSugestaoInspecao(campos[0], campos[1]);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }


            //Response.Cookies.Append("UserCookie", "I" + Username);//colocar aqui o cookie.
            return new JsonResult(result);
        }
    }
}