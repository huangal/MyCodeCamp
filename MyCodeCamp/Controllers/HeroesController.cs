using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Models;


namespace MyCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class HeroesController : Controller
    {
        private readonly ITopicAreaService _service;

        public HeroesController(ITopicAreaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var heroes = await _service.GetAllHeroesAsync();


                if (heroes != null)
                {
                    return Ok(heroes);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return BadRequest(ex);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var hero = await _service.GetHeroAsync(id);

                if (hero != null)
                    return Ok(hero);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return BadRequest(ex);
            }
        }


 
    }
}
