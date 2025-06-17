using Microsoft.AspNetCore.Mvc;
using ResponseDataWebAPI;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text.RegularExpressions;

namespace ResponseDataWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WeatherForecastController : BaseController //inherit BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("ResponseDataExample")]
        public async Task<IActionResult> ResponseDataExample()
        {
            TblUserRegistrationDTO tblUserRegistrationDTO = new TblUserRegistrationDTO();
            return OkResult(tblUserRegistrationDTO);
            //return OkResult(1, "Deleted successfully");
            //return OkResult(1, "Successful login.", new { });
        }

        [HttpPost("EmailValidation")]
        [Consumes("application/json")]
        [SwaggerOperation("Logs in user.")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(string), description: "status 0: Valid Email.<br/> status 1: Invalid email.") ]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ResponseData), description: "Request data is not provided properly or missing required information.")]

        public async Task<IActionResult> EmailValidation(string str)
        {
            if (str.Length < 5)
            {
                return OtherResult(HttpStatusCode.BadRequest, 1, "Please provide length of string which is greater then 5");
            }
            Regex regex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-]+$");
            if (!regex.IsMatch(str))
            {
                return OtherResult(HttpStatusCode.BadRequest, 1, "Invalid Email");
            }
            return OkResult(0, "Email is valid");
            //return OkResult("Email is valid");
        }

        [HttpPost("Enter0To5")]
        public async Task<IActionResult> BedRequest(int num)
        {
            if (num >= 0 && num <= 5)
            {
                return OkResult();
            }
            return OtherResult(HttpStatusCode.BadRequest);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByNameStartWith(string nameStartWith)
        {
            //
            var data = Summaries.Where(x => x.StartsWith(nameStartWith)).Select(x => x).ToList();
            string p = null;
            if (data.Count != 0)
            {
                return OkResult(new { id = 1, Name = "REshma", p = p });
            }
            return OtherResult(HttpStatusCode.BadRequest, $"Data that starts with {nameStartWith} not found");
        }
    }

    public class TblUserRegistrationDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int CityId { get; set; }
    }
}