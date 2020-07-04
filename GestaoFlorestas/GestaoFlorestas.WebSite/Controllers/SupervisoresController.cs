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
    public class SupervisoresController : ControllerBase
    {
        public SupervisoresController(GestaoFlorestasService gestaoFlorestasService,
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
            Claim c = new Claim("Supervisor", username);
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


            if (claim.Type.Equals("Supervisor") && claim.Value.Equals(username)) return true;

            else return false;
        }


        //-------------------------------------Endpoints

        [Route("Registo")]
        [HttpPost]
        public ActionResult Registo([FromBody] string supervisor)
        {
            string[] campos = supervisor.Split(',');
            try
            {
                this.GestaoFlorestasService.registoSupervisor(campos[0], campos[1], campos[2], campos[3], campos[4]);
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
            object r;
            try
            {
                r = this.GestaoFlorestasService.loginSupervisor(Username, Password);
            }

            catch (ExistingUserException e)
            {
                return Unauthorized();
            }

            List<object> res = new List<object>();
            res.Add(r);
            var token = GerarToken(Username);
            res.Add(token);
            return new JsonResult(res);
            
        }



        [Authorize]
        [Route("Terrenosnif")]
        [HttpGet]

        public ActionResult TerrenosNif([FromQuery] string Username,
                                        [FromQuery] string Nif,
                                        [FromHeader] string Authorization)
        {
            object r;
            if (MiddleWare(Authorization, Username))
            {
                r = this.GestaoFlorestasService.terrenosNifConcelho(Username, Int32.Parse(Nif));
                return new JsonResult(r);
            }
            else return Unauthorized();
        }


        [Authorize]
        [Route("Terrenoscamara")]
        [HttpGet]

        public ActionResult TerrenosCamara([FromQuery] string Username,
                                           [FromHeader] string Authorization)
        {
            object r;
            if (MiddleWare(Authorization, Username))
            {
                r = this.GestaoFlorestasService.terrenosCamara(Username);
                return new JsonResult(r);
            }
            else return Unauthorized();
        }

        [Authorize]
        [Route("Trabalhadorescamara")]
        [HttpGet]

        public ActionResult TrabalhadoresCamara([FromQuery] string Username,
                                          [FromHeader] string Authorization)
        {
            object r;
            if (MiddleWare(Authorization, Username))
            {
                r = this.GestaoFlorestasService.trabalhadoresCamara(Username);
                return new JsonResult(r);
            }
            else return Unauthorized();
        }


        [Route("Trocaprop")]
        [HttpPut]
        public ActionResult TrocaProp([FromBody] string body, [FromHeader] string Authorization) //body username,idTerreno,nifnovoprop
                                           
        {
            string[] campos = body.Split(',');
            if (MiddleWare(Authorization, campos[0]))
            {
                try
                {
                    this.GestaoFlorestasService.trocaProprietarioTerreno(campos[0], Int32.Parse(campos[1]), campos[2]);
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
        [Route("Seguranca")]
        [HttpGet]
        public ActionResult ConcelhoSeguro([FromQuery] string Username,
                                           [FromHeader] string Authorization)
        {
            int res;
            if (MiddleWare(Authorization, Username))
            {
                res = this.GestaoFlorestasService.terrenosPorLimparConcelho(Username);
                return new JsonResult(res);//se o resultado for 0 entao todos os terrenos foram limpos, caso contrario o concelho não está seguros pois tem x terrenos por limpar
            }
            else return Unauthorized();
        }

        //------------------------------------------------

        [Authorize]
        [Route("AgendarLimpeza")]
        [HttpPost]
        public ActionResult AgendarLimpeza([FromBody] string body, [FromHeader] string Authorization) //body "username,idTrabalhador,idTerreno"
        {
            string[] campos = body.Split(',');
            if (MiddleWare(Authorization, campos[0]))
            {
                try
                {
                    this.GestaoFlorestasService.agendarLimpeza(campos[0], campos[1], Int32.Parse(campos[2]));
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
        [Route("Agendarinspecao")]
        [HttpPost]
        public ActionResult AgendarInspecao([FromBody] string body, [FromHeader] string Authorization) //body "username,codPostaldaZona"
        {
            string[] campos = body.Split(',');
            if (MiddleWare(Authorization, campos[0]))
            {
                try
                {
                    this.GestaoFlorestasService.agendarInspecao(campos[0], campos[1]);
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
        [Route("Zonasconcelho")]
        [HttpGet]
        public ActionResult ZonasConcelho([FromQuery] string Username, [FromHeader] string Authorization) 
        {
            object res;
            if (MiddleWare(Authorization, Username))
            {
                res = this.GestaoFlorestasService.zonasConcelho(Username);
                return new JsonResult(res);
            }
            else return Unauthorized();
        }


        [Authorize]
        [Route("Concelho")]
        [HttpGet]
        public ActionResult GetConcelho([FromQuery] string Username, [FromHeader] string Authorization)
        {
            object res;
            if (MiddleWare(Authorization, Username))
            {
                res = this.GestaoFlorestasService.getConcelho(Username);
                return new JsonResult(res);
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
                object result = this.GestaoFlorestasService.notificacoesSupervisor(Username);
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
                this.GestaoFlorestasService.visualizarNotificacoesSuper(body);
                return Ok();
            }
            else return Unauthorized();
        }

        [Authorize]
        [Route("Notificacoesnovas")]
        [HttpGet]
        public ActionResult NotificacoesPorLer([FromQuery] string Username,
                                               [FromHeader] string Authorization)
        {
            if (MiddleWare(Authorization, Username))
            {
                object result = this.GestaoFlorestasService.notificacoesPorLerSupervisor(Username);
                return new JsonResult(result);
            }
            else return Unauthorized();
        }

        [Authorize]
        [Route("Notificacoes/Elim")]
        [HttpDelete]
        public ActionResult EliminaNotificacao([FromBody] string body,
                                              [FromHeader] string Authorization)//body: "username-|-idNotificacao"
        {
            string[] campos = body.Split("-|-");

            if (MiddleWare(Authorization, campos[0]))
            {
                try
                {
                    this.GestaoFlorestasService.eliminaNotificacaoSupervisor(campos[0], campos[1]);
                    return Ok();
                }
                catch (ExistingUserException e) { return Unauthorized(); }

            }
            else return Unauthorized();
        }

        [Route("Resetpassword")]
        [HttpPut]
        public ActionResult resetPass([FromBody] string body)
        {
            this.GestaoFlorestasService.criaTokenPassword(body, "Supervisor");
            return Ok();
        }

        [Route("Verificatoken")]
        [HttpPut]
        public ActionResult verifyToken([FromBody] string body) //body:"username-|-token-|-password"
        {
            string[] campos = body.Split("-|-");

            int r = this.GestaoFlorestasService.VerificaTokenPassword(campos[0], "Supervisor", campos[1], campos[2]);

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
                this.GestaoFlorestasService.changeNameSup(campos[0], newName);
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
                this.GestaoFlorestasService.changeEmailSup(campos[0], newEmail);
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
                    this.GestaoFlorestasService.changePasswordSup(campos[0], campos[1], campos[2]);
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