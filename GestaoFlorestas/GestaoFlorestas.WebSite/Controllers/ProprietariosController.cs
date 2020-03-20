using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFlorestas.WebSite.Models;
using GestaoFlorestas.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFlorestas.WebSite.Controllers
{
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
    }
}
