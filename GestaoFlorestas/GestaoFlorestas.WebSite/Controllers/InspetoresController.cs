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
        [HttpGet] //Put ou Post???
        public ActionResult Registo([FromQuery] string Username,
                                    [FromQuery] string Nome,
                                    [FromQuery] string Mail,
                                    [FromQuery] string Password)
        {
            try
            {
                this.GestaoFlorestasService.registoInspetores(Username, Nome, Mail, Password);
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
    }
}