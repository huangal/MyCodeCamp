using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Models;

namespace MyCodeCamp.Controllers
{

    /// <summary>
    /// Authentication controller manage all fuctionality for authentication mechanism.
    /// </summary>
    [Route("api/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly ISamlEnrollmentService _samlService;
        private readonly IConfiguration _config;

        //private readonly IHttpContextAccessor _httpContextAccessor;  
        //private IConfigurationRoot _config;

        public AuthenticationController(ISamlEnrollmentService samlService, IConfiguration config)
        {
           // this._httpContextAccessor = httpContextAccessor; 
            _samlService = samlService;
            _config = config;
        }




        [HttpGet("Token/Claims")]
        public IActionResult GetClaims()
        {
            string userDataId = Guid.NewGuid().ToString();


            return Ok(userDataId);

        }


        /// <summary>
        /// Post a SALM2 respone assertion
        /// </summary>
        /// <returns>Return an Instance for the identity domain object</returns>
        /// <param name="form">Http post form request</param>
        [HttpPost("SAML2/SSO/Post")]
        public IActionResult PostResponse( IFormCollection form)
        {
            string samlRequest = form["SAMLRequest"];
            string samlResponse = form["SAMLResponse"];
            string relayState = form["RelayState"];

            if( !samlResponse.IsValidString())
            {
                return BadRequest("Invalid SAML Assertions");
            }

            var samlResponseXml = _samlService.ProcessSamlResponse(samlResponse, relayState);
            if( !samlResponse.IsValidString())
            {
                return new InvalidEntityObjectResult(ModelState);
            }


            string key = Guid.NewGuid().ToString();


            _samlService.SetCookie(key, samlResponseXml, 10);



            // string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["key"];  

            //read cookie from Request object  
            // string cookieValueFromReq = Request.Cookies["Key"]; 


            string value = _samlService.GetCookie(key);



            return Ok(samlResponseXml);


        }


        [HttpGet("Cookie/{key}/{value}/{expireTime}")]
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        //[ValidateModel]
        [HttpPost("Token")]
        public IActionResult CreateToken()
        {
            try
            {
                var user = new CampUser { Name = "Ramon" };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };


                var token = new JwtSecurityToken(
                    issuer: _config["Tokens:Issuer"],
                    audience: _config["Tokens:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: credentials
  
                );


                return Ok( new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });





                //var token = JwtSecurityToken(
                //    issuer:"http://localhost:5000/api/Authentication",
                //    audience: "http://localhost:5000/api/Authentication",
                //    claims: claims,
                //    expires: DateTime.UtcNow.AddMinutes(15),
                //    signingCredentials: cred
  
                //); 
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return BadRequest("Failed to create token");
        }



    }
}
