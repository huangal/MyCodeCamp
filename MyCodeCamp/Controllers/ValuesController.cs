using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Models;

namespace MyCodeCamp.Controllers
{
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        private ITopicAreaService _service;
        private IMapper _mapper;
        private WeblogConfiguration _config;
        private EmailConfiguration _emailConfig;

        public ValuesController(ITopicAreaService service, IMapper mapper, WeblogConfiguration config, EmailConfiguration emailConfiguration)
        {
            _service = service;
            _mapper = mapper;
            _config = config;
            _emailConfig = emailConfiguration;
        }

        [Route("Topics")]
        [HttpGet]

        public IActionResult GetTopicAreas()
        {
            try
            {
                var topics = _service.GetAllTopics();


                if (topics != null)
                {
                    var result = _mapper.Map<List<TopicAreaModel>>(topics);
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return BadRequest(ex);
            }

          //  return BadRequest("Invalid request");

        }


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpPost("encode")]
        public string Encode([FromBody]string value)
        {

            //string name = null;

           // var data = name.Base64Encode();

            return value.Base64Encode();
        }


        [HttpPost("decode")]
        public string Decode([FromBody]string value)
        {
            return value.Base64Decode();
        }





        [HttpGet("saml")]
        public IActionResult GetSaml()
        {

            XmlDocument doc = new XmlDocument();
           
            string path = "file:///Users/henryhuangal/Documents/SAML2TEST.xml";
            doc.Load(path);


          var xmlHelper = new XmlHelper();

            xmlHelper.GetXml(doc);








            string hello = "Cool Beans";
            return Ok(hello);

        }

    }
}
