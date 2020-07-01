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
    public class InspetoresController : ControllerBase
    {
        public InspetoresController(GestaoFlorestasService gestaoFlorestasService,
                                     IOptions<AppSettings> appSettings)
        {
            this.GestaoFlorestasService = gestaoFlorestasService;
            this._appSettings = appSettings.Value;
        }

        public GestaoFlorestasService GestaoFlorestasService { get; }
        private readonly AppSettings _appSettings;


        private string GerarToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var identityClaims = new ClaimsIdentity();
            Claim c = new Claim("Inspetor", username);
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

            if (claim.Type.Equals("Inspetor") && claim.Value.Equals(username)) return true;

            else return false;
        }



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
            object i;
            try
            {
                i = this.GestaoFlorestasService.loginInspetor(Username, Password);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            List<object> res = new List<object>();
            res.Add(i);
            var token = GerarToken(Username);
            res.Add(token);
            return new JsonResult(res);
        }

        [Authorize]
        [Route("Realizarinspecao")]
        [HttpPut]
        public ActionResult RealizarInspecao([FromBody] string body,[FromHeader] string Authorization) // body "username|resultado|relatorio|idTerreno
        {

            string[] campos = body.Split('|');

            if (MiddleWare(Authorization, campos[0]))
            {
                try
                {
                    this.GestaoFlorestasService.realizarInspecao(campos[0], Int32.Parse(campos[1]), campos[2], Int32.Parse(campos[3]));
                }
                catch (ExistingUserException e)
                {
                    return Unauthorized();
                }
                return Ok();
            }
            else return Unauthorized();
        }

        [Authorize]
        [Route("Sugestaoinspecao")]
        [HttpGet]
        public ActionResult Inspecao([FromQuery] string Username,[FromHeader] string Authorization) // body "username"
        {
            object result = null;

            if (MiddleWare(Authorization, Username))
            {
                result = this.GestaoFlorestasService.getSugestaoInspecao(Username);
                return new JsonResult(result);
            }
            else return Unauthorized();
        }

        //-------------------------------------------------notificações-------------------------------------------------
        [Authorize]
        [Route("Notificacoes")]
        [HttpGet]
        public ActionResult GetNotifications([FromQuery] string Username,
                                            [FromHeader] string Authorization)
        {
            if (MiddleWare(Authorization, Username))
            {
                object result = this.GestaoFlorestasService.notificacoesInspetor(Username);
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
                this.GestaoFlorestasService.visualizarNotificacoesInsp(body);
                return Ok();
            }
            else return Unauthorized();
        }

        [Authorize]
        [Route("Localizacao")]
        [HttpPut]
        public ActionResult AtulizaLocalizacao ([FromBody] string body, [FromHeader] string Authorization)//body:"username|latitude|longitude"
        {
            string[] campos = body.Split('|');
            if (MiddleWare(Authorization, campos[0])){
                String lat = campos[1].Replace(".", ",");
                String lo = campos[2].Replace(".", ",");
                this.GestaoFlorestasService.atualizaLocalizacao(campos[0], Double.Parse(lat), Double.Parse(lo));
                return Ok();
            }
            return Unauthorized();
        }

        [Route("Resetpassword")]
        [HttpPut]
        public ActionResult resetPass([FromBody] string body)
        {
            this.GestaoFlorestasService.criaTokenPassword(body, "Inspetor");
            return Ok();
        }

        [Route("Verificatoken")]
        [HttpPut]
        public ActionResult verifyToken([FromBody] string body) //body:"username-|-token-|-password"
        {
            string[] campos = body.Split("-|-");

            int r = this.GestaoFlorestasService.VerificaTokenPassword(campos[0], "Inspetor", campos[1], campos[2]);

            if (r == 1) return Ok();
            else return Unauthorized();
        }

        [Authorize]
        [Route("Info/Changes/Nome")]
        [HttpPut]
        public ActionResult ChangeName([FromBody] string body, [FromHeader] string Authorization) //body: "username,newName"
        {
            string[] campos = body.Split(',');

            string newName = campos[1];

            if (MiddleWare(Authorization, campos[0]))
            {
                this.GestaoFlorestasService.changeNameInsp(campos[0], newName);
                return Ok();
            }

            else return Unauthorized();
        }

        [Authorize]
        [Route("Info/Changes/Email")]
        [HttpPut]
        public ActionResult ChangeEmail([FromBody] string body, [FromHeader] string Authorization) //body: "username,newEmail"
        {
            string[] campos = body.Split(',');

            string newEmail = campos[1];

            if (MiddleWare(Authorization, campos[0]))
            {
                this.GestaoFlorestasService.changeEmailInsp(campos[0], newEmail);
                return Ok();
            }

            else return Unauthorized();
        }


        [Authorize]
        [Route("Info/Changes/Password")]
        [HttpPut]
        public ActionResult ChangePassword([FromBody] string body, [FromHeader] string Authorization) //body: "username,oldPassword,newPassword"
        {
            string[] campos = body.Split(',');


            if (MiddleWare(Authorization, campos[0]))
            {
                try
                {
                    this.GestaoFlorestasService.changePasswordInsp(campos[0], campos[1], campos[2]);
                    return Ok();
                }
                catch (ExistingUserException e)
                {
                    return Unauthorized();
                }
            }

            else return Unauthorized();
        }
    }
}