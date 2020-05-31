using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using System.Security.Claims;

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



        //----------------------------------Middleware-----------------------------------

        private string GerarToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var identityClaims = new ClaimsIdentity();
            Claim c = new Claim("Proprietário", username);
            identityClaims.AddClaim(c);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }


        private Boolean MiddleWare(string Authorization, string username)
        {
            string[] a = Authorization.Split(' ');


            var jwt = a[1];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            List<Claim> claims = new List<Claim>(token.Claims);

            Claim claim = claims[0];


            if (claim.Type.Equals("Proprietário") && claim.Value.Equals(username)) return true;

            else return false;
        }


        //-------------------------------------Endpoints


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

        public  ActionResult GetL([FromQuery] string Username,
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

            List<object> res = new List<object>();
            res.Add(p);
            var token = GerarToken(Username);
            res.Add(token);
            return new JsonResult(res);
        }

        //-------------------------------------------------------------------------------Limpeza de terrenos-------------------------------------------------------------

        //[Authorize]
        [Route("Limpeza")]
        [HttpPut]
        public ActionResult LimpaTerreno([FromBody] string body, [FromHeader] string Authorization) //body: "username,idTerreno"
        {
            string[] campos = body.Split(',');

            int idTerreno = Int32.Parse(campos[1]);

            if (MiddleWare(Authorization, campos[0])){

                try
                {
                    this.GestaoFlorestasService.limparTerreno(idTerreno, campos[0]);
                }
                catch (ExistingUserException e)
                {
                    return Unauthorized();
                }

                return Ok();
            }
            else return Unauthorized();
        }

        //----------------------------------------------------------------Informação dos terrenos----------------------------------------------------

        //[Authorize]
        [Route("Terrenos")]
        [HttpGet]
        public ActionResult GetTerrenos([FromQuery] string Username,
                                        [FromHeader] string Authorization)
        {
            if (MiddleWare(Authorization, Username)) return new JsonResult(this.GestaoFlorestasService.terrenosDoProprietario(Username));
            
            else return Unauthorized();
        }


        [Authorize]
        [Route("Info/Changes/Nome")]
        [HttpPut]
        public ActionResult ChangeName([FromBody] string body, [FromHeader] string Authorization) //body: "username,newName"
        {
            string[] campos = body.Split(',');

            string newName = campos[2];

            if (MiddleWare(Authorization, campos[0]))
            {
                this.GestaoFlorestasService.changeNameProp(campos[0], newName);
                return Ok();
            }

            else return Unauthorized();
        }


        //------------------------------------notificacoes -----------------------------------------
        [Authorize]
        [Route("Notificacoes")]
        [HttpGet]
        public ActionResult GetNotifications([FromQuery] string Username,
                                             [FromHeader] string Authorization)
        {
            if (MiddleWare(Authorization, Username))
            {
                object result = this.GestaoFlorestasService.notificacoesProprietario(Username);
                return new JsonResult(result);
            }
            else return Unauthorized();
        }

        [Authorize]
        [Route("Notificacoes/Ler")]
        [HttpPut]
        public ActionResult AtualizaNotifications([FromBody] string body, [FromHeader] string Authorization)//body: "username"
        {
            if (MiddleWare(Authorization, body))
            {
                this.GestaoFlorestasService.visualizarNotificacoesProp(body);
                return Ok();
            }
            else return Unauthorized();
        }



        /*em principio nao sera preciso

        //[Authorize]
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


        //[Authorize]
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
        //[Authorize]

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
         
         */
        

    }
}
