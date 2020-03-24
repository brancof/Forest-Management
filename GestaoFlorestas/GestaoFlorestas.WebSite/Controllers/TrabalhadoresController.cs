using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFlorestas.WebSite.Controllers
{
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
        [HttpGet] //Put ou Post???
        public ActionResult Registo([FromQuery] string Nome, 
                                    [FromQuery] string Username,
                                    [FromQuery] string Mail,
                                    [FromQuery] string Password,
                                    [FromQuery] string Concelho)
        {
            this.GestaoFlorestasService.registoTrabalhadores(Nome, Username, Mail, Password,Concelho);

            return Ok();
        }

        [Route("Login")]
        [HttpGet]

        public ActionResult Login([FromQuery] string Username,
                                  [FromQuery] string Password)
        {
            this.GestaoFlorestasService.loginTrabalhadores(Username, Password);


            Response.Cookies.Append("UserCookie", "T" + Username);//colocar aqui o cookie.
            return Ok();
        }
    }
}