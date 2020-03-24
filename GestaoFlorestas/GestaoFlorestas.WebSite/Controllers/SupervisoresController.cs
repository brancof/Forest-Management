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
    public class SupervisoresController : ControllerBase
    {
        public SupervisoresController(GestaoFlorestasService gestaoFlorestasService)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }

        [Route("Registo")]
        [HttpGet] //Put ou Post???
        public ActionResult Registo([FromQuery] string Username,
                                    [FromQuery] string Nome,
                                    [FromQuery] string Mail,
                                    [FromQuery] string Password,
                                    [FromQuery] string Concelho)
        {
            this.GestaoFlorestasService.registoSupervisor(Nome, Username, Mail, Password, Concelho);

            return Ok();
        }

        [Route("Login")]
        [HttpGet]

        public ActionResult Login([FromQuery] string Username,
                                  [FromQuery] string Password)
        {
            this.GestaoFlorestasService.loginSupervisor(Username, Password);


            Response.Cookies.Append("UserCookie", "S" + Username);//colocar aqui o cookie.
            return Ok();
        }
    }
}