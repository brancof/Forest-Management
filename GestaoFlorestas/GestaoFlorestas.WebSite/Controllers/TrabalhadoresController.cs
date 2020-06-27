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
    public class TrabalhadoresController : ControllerBase
    {



        public TrabalhadoresController(GestaoFlorestasService gestaoFlorestasService,
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
            Claim c = new Claim("Trabalhador", username);
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


            if (claim.Type.Equals("Trabalhador") && claim.Value.Equals(username)) return true;

            else return false;
        }


        //-------------------------------------Endpoints

        [Route("Registo")]
        [HttpPost] 
        public ActionResult Registo([FromBody] string trabalhador)
        {
            string[] campos = trabalhador.Split(',');
            try
            {
                this.GestaoFlorestasService.registoTrabalhadores(campos[0], campos[1], campos[2], campos[3], campos[4]);
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
            object tc;
            try
            {
                tc = this.GestaoFlorestasService.loginTrabalhadores(Username, Password);
            }
            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            List<object> res = new List<object>();

            res.Add(tc);
            res.Add(GerarToken(Username));

            
            return new JsonResult(res);
        }



        //-------------------Limpeza de Terreno---------------------------------
        [Authorize]
        [Route("Limpeza")]
        [HttpPut]
        public ActionResult LimpaTerreno([FromBody] string body, [FromHeader] string Authorization) //body: "username,idTerreno"
        {
            string[] campos = body.Split(',');
            object result;
            int idTerreno = Int32.Parse(campos[1]);

            if (MiddleWare(Authorization, campos[0]))
            {
                result = this.GestaoFlorestasService.limparTerrenoTrabalhador(campos[0], idTerreno);
                return new JsonResult(result); //retorna o trabalhador com a lista de terrenos por limpar atualizada.
            }
            else return Unauthorized();
            
        }


        //------------------get terrenos pra limpeza--------------------------------

        [Authorize]
        [Route("LimpezasPendentes")]
        [HttpGet]
        public ActionResult GetTerrenos([FromQuery] string Username,
                                        [FromHeader] string Authorization)
        {
            object res;
            if (MiddleWare(Authorization, Username))
            {
                res = this.GestaoFlorestasService.terrenosALimpar(Username);
                return new JsonResult(res);
            }
            else return Unauthorized();
        }


        //-------------notificacoes-----------------------------------------

        [Authorize]
        [Route("Notificacoes")]
        [HttpGet]
        public ActionResult GetNotifications([FromQuery] string Username,
                                            [FromHeader] string Authorization)
        {
            object result;

            if (MiddleWare(Authorization, Username))
            {
                result = this.GestaoFlorestasService.notificacoesTrabalhador(Username);
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
                this.GestaoFlorestasService.visualizarNotificacoesTrabalhador(body);
                return Ok();
            }
            else return Unauthorized();
        }



        //-------sugestão de limpeza (algoritmo)---------------

        [Authorize]
        [Route("Sugestao")]
        [HttpGet]
        public ActionResult GetSugestao([FromQuery] string username, [FromHeader] string Authorization)
        {
            if (MiddleWare(Authorization, username))
            {
                object r = this.GestaoFlorestasService.getSugestaoLimpeza(username);
                return new JsonResult(r);
            }
            else return Unauthorized();
        }


        [Authorize]
        [Route("Localizacao")]
        [HttpPut]
        public ActionResult AtulizaLocalizacao([FromBody] string body, [FromHeader] string Authorization)//body:"username|latitude|longitude"
        {
            string[] campos = body.Split('|');
            if (MiddleWare(Authorization, campos[0]))
            {
                String lat = campos[1].Replace(".", ",");
                String lo = campos[2].Replace(".", ",");
                this.GestaoFlorestasService.atualizaLocalizacaoTrabalhadores(campos[0], Double.Parse(lat), Double.Parse(lo));
                return Ok();
            }
            return Unauthorized();
        }

        [Route("Resetpassword")]
        [HttpPut]
        public ActionResult resetPass([FromBody] string body)
        {
            this.GestaoFlorestasService.criaTokenPassword(body, "Trabalhador");
            return Ok();
        }

        [Route("Verificatoken")]
        [HttpPut]
        public ActionResult verifyToken([FromBody] string body) //body:"username-|-token-|-password"
        {
            string[] campos = body.Split("-|-");

            int r = this.GestaoFlorestasService.VerificaTokenPassword(campos[0], "Trabalhador", campos[1], campos[2]);

            if (r == 1) return Ok();
            else return Unauthorized();
        }
    }
}