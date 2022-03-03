using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TesteAuditoria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            using var context = new TesteAuditoriaContext();
            return context.WeatherForecasts.ToList();
        }

        [HttpPost]
        public ActionResult Post()
        {
            using (var context = new TesteAuditoriaContext())
            {
                var weatherForecast = new WeatherForecast()
                {
                    Summary = "Teste",
                    TemperatureC = 15,
                    Date = DateTime.Now
                };

                context.WeatherForecasts.Add(weatherForecast);
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpPut]
        public ActionResult Put()
        {
            using (var context = new TesteAuditoriaContext())
            {
                var weatherForecast = context.WeatherForecasts.First();
                weatherForecast.Summary = "Teste Alteração";
                weatherForecast.Date = DateTime.Today.AddDays(3);
                weatherForecast.TemperatureC = 50;
                context.SaveChanges();
            }

            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            using (var context = new TesteAuditoriaContext())
            {
                var weatherForecast = context.WeatherForecasts.First();
                context.WeatherForecasts.Remove(weatherForecast);
                context.SaveChanges();
            }

            return Ok();
        }
    }
}
