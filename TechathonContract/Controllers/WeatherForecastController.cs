using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechathonContract.Models;

namespace TechathonContract.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IBAO _BAO;
        UserManager<ApplicationUser> _userManager;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IBAO BAO, ILogger<WeatherForecastController> logger, UserManager<ApplicationUser> userManager)
        {
            _BAO = BAO;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            string name = _userManager.Users.First().UserName;
            var user = _BAO.SaveUser(12312);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }        

        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload()
        {
            var files = Request.Form.Files;
            return Ok("All the files are successfully uploaded.");
        }

        [HttpGet]
        [Route("UploadGet")]
        public IActionResult UploadGet()
        {
            var files = "asasa";
            return Ok("All the files are successfully uploaded.");

        }
        
    }
}
