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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace GestaoFlorestas.WebSite.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    
    [Route("[controller]")]
    [ApiController]
    public class ProprietariosController : ControllerBase
    {
      

        public ProprietariosController(GestaoFlorestasService gestaoFlorestasService,
                                        IOptions<AppSettings> appSettings)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
            this._appSettings = appSettings.Value;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }
        private readonly AppSettings _appSettings;


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

        public async Task<ActionResult> GetL([FromQuery] string Username,
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
            //return Ok(await GerarToken());
            return new JsonResult(p);
        }

        //-------------------------------------------------------------------------------Limpeza de terrenos-------------------------------------------------------------

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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


        [Authorize]
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
        [Authorize]
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

        [Authorize]
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


            this.GestaoFlorestasService.changeNameProp(p, newName);




            return Ok();
        }


        //------------------------------------notificacoes -----------------------------------------
        [Authorize]
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

        [Authorize]
        [Route("Notificacoes/Ler")]
        [HttpPut]
        public ActionResult AtualizaNotifications([FromBody] string body)//body: "username,password"
        {
            string[] campos = body.Split(',');
            try
            {
                this.GestaoFlorestasService.visualizarNotificacoesProp(campos[0], campos[1]);

            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }
            return Ok();

        }

        /*private async Task<string> GerarToken()
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }*/
    }
}
