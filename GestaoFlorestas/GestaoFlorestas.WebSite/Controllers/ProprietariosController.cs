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
        [HttpGet]
        public ActionResult Get([FromQuery] string Username, 
                                [FromQuery] string Nome, 
                                [FromQuery] string Mail, 
                                [FromQuery] string Nif, 
                                [FromQuery] string Password)
        {
            this.GestaoFlorestasService.registoProprietario(Username, Nome, Mail, Nif, Password);
            
            return Ok();
        }

        [Route("Login")]
        [HttpGet]

        public ActionResult GetL([FromQuery] string Username,
                                [FromQuery] string Password)
        {
            this.GestaoFlorestasService.loginProprietario(Username, Password);

            
            Response.Cookies.Append("UserCookie","P"+Username);//colocar aqui o cookie.
            return Ok();
        }


    }
}
