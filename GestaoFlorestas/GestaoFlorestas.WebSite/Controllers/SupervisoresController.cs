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
        [HttpGet] //Put ou Post???
        public ActionResult Registo([FromQuery] string Username,
                                    [FromQuery] string Nome,
                                    [FromQuery] string Mail,
                                    [FromQuery] string Password,
                                    [FromQuery] string Concelho)
        {
            try
            {
                this.GestaoFlorestasService.registoSupervisor(Nome, Username, Mail, Password, Concelho);
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
    }
}