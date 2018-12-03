using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Models;

namespace MyCodeCamp.Controllers
{
    [Route("api/Topics")]
    public class TopicsController : Controller
    {

        private readonly ITopicAreaService _service;

        public TopicsController(ITopicAreaService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult Get()
        {

            var topics = _service.GetAllTopics();

            if (topics != null)
            {
                return Ok(topics);
            }
            return NotFound();


            //  return BadRequest("Invalid request");

        }


        [HttpGet("All")]
        public ActionResult<IEnumerable<TopicAreaModel>> GetAllTopics()
        {

            var topics = _service.GetAllTopics();

            if (topics != null)
            {
                return Ok(topics);
            }
            return NotFound();

        }

        [HttpPost]
        public IActionResult PostTopic([FromBody] BookTopic topic)
        {
            if (topic == null)
                throw new ArgumentNullException();
                //return BadRequest();

            if(!ModelState.IsValid)
            {
                return new InvalidEntityObjectResult(ModelState);

            }

            return Ok(topic);
        }

        [HttpPut]
        public IActionResult PutTopic([FromBody]BookTopicUpdate topic)
        {
            if (topic == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return new InvalidEntityObjectResult(ModelState);

            }

            return Ok(topic);
        }


        [HttpGet("List")]
        public IActionResult GetTopics()
        {
            try
            {
                var topics = _service.GetAllTopics();


                if (topics != null)
                {
                    return Ok(topics);
                }
                 return NotFound();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
                return BadRequest(ex);
            }

            //  return BadRequest("Invalid request");

        }



        //[HttpGet("Heroes")]
        //public IActionResult GetHeroes()
        //{
        //    try
        //    {
        //        var heroes = _service.GetAllHeroes();


        //        if (heroes != null)
        //        {
        //            return Ok(heroes);
        //        }
        //        return NotFound();

        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine(ex);
        //        return BadRequest(ex);
        //    }
        //}





        /// <summary>
        /// Retrieves a list of implementation of IMock interface
        /// </summary>
        /// <remarks>Cool Beans!</remarks>
        /// <response code="200">List of implementation</response>
        /// <response code="400">Implementations not found</response>
        /// <response code="500">Unable to retrieve you request</response>
        [HttpGet("Implementations")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [ProducesResponseType(500)]
        public IActionResult GetImplementations()
        {
            try
            {
                string url = "https://hsa.umb.com";

                var match = url.IsValidUrl();


                match =("https://f01.servicesqa.nonprod.umb.com").IsValidUrl();


                match =("http://umb.com").IsValidUrl();
               

                match = ("http:/www.umb.net").IsValidUrl();


                match = ("abc").IsValidUrl();


                match = ("https://huangal.com").IsValidUrl();
               

                var baseApiUri = new Uri("https://f01.servicesqa.nonprod.umb.com");

                var mainframePath = new Uri(baseApiUri, "mainframe/");

                string fullUrl = mainframePath.AbsoluteUri;

                Console.WriteLine(fullUrl);





                // Create a relative Uri from a string.  allowRelative = true to allow for 
                // creating a relative Uri.
                Uri relativeUri = new Uri("/catalog/shownew.htm?date=today", UriKind.Relative);

                // Check whether the new Uri is absolute or relative.
                if (!relativeUri.IsAbsoluteUri)
                    Console.WriteLine("{0} is a relative Uri.", relativeUri);

                // Create a new Uri from an absolute Uri and a relative Uri.
                Uri combinedUri = new Uri(baseApiUri, relativeUri);
                Console.WriteLine(combinedUri.AbsoluteUri);




                var myUri = new Uri(baseApiUri, "catalog/shownew.htm");

                Console.WriteLine(myUri);

                return Ok(_service.GetAllInstances());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("SamlAssertion")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        [ProducesResponseType(500)]
        public IActionResult PostSamlAssertion([FromBody] string samlResponse)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(samlResponse))
                {
                    return BadRequest("Consumption URL hit without a SAML Response");
                }


                string value = samlResponse.Base64Decode();

                // MVC Already gives me this URL-decoded

                byte[] bytes = Convert.FromBase64String(samlResponse);

                // For this question, assume that this is not deflated.

                string samlXmlIfAscii = Encoding.ASCII.GetString(bytes);
                string samlXmlIfUtf8 = Encoding.UTF8.GetString(bytes);


                return Ok(_service.GetAllInstances());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok();
        }



        //static void DisplayAppSettings()
        //{
        //    try
        //    {
        //        System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();

        //        NameValueCollection appSettings = ConfigurationManager.AppSettings;

        //        for (int i = 0; i < appSettings.Count; i++)
        //        {
        //            string key = appSettings.GetKey(i);
        //            string value = (string)reader.GetValue(key, typeof(string));
        //            Console.WriteLine("Key : {0} Value: {1}", key, value);
        //        }
        //    }
        //    catch (ConfigurationErrorsException e)
        //    {
        //        Console.WriteLine("[DisplayAppSettings: {0}]", e.ToString());
        //    }

        //}



    }
}
