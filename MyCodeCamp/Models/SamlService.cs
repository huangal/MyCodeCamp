using System;
using Microsoft.AspNetCore.Http;

namespace MyCodeCamp.Models
{



    public class SamlService : ISamlEnrollmentService
    {
        IHttpContextAccessor _httpContextAccessor;

        public SamlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        public string ProcessSamlRequest(string samlRequest)
        {
            if (samlRequest.IsValidString())
            {
                return samlRequest.Base64Decode();
            }
            return string.Empty;
        }

        public string ProcessSamlResponse(string samlResponse, string relayState = "")
        {
            if (samlResponse.IsValidString())
            {
                return samlResponse.Base64Decode();
            }
            return string.Empty;
        }


        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }


        public string GetCookie(string key)
        {
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies[key];  

            //read cookie from Request object  
            string cookieValueFromReq = _httpContextAccessor.HttpContext.Request.Cookies[key]; 

            return cookieValueFromContext;


        }
    }
}
