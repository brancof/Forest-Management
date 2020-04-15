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
            Proprietario p;
            String result = "";
            try
            {
               p = this.GestaoFlorestasService.loginProprietario(Username, Password);
               result = JsonConvert.SerializeObject(p);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            

            //Response.Cookies.Append("UserCookie","P"+Username);//colocar aqui o cookie.
            return new JsonResult(p);
        }


    }
}
