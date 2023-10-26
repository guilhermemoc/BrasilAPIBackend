using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TesteApiBrasil.Controllers;

[ApiController]
[Route("[controller]")]
public class CityAirportWeatherController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<CityAirportWeatherController> _logger;

    public CityAirportWeatherController(IMediator mediator, ILogger<CityAirportWeatherController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("GetByCityName")]
    public async Task<IActionResult> GetWeatherByCityName(string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
            return BadRequest("Cidade Inválida");
        
        var query = new GetWeatherByCityNameQuery { CityName = cityName };
        
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }

    [HttpGet]
    [Route("GetByAirportCode")]
    public async Task<IActionResult> GetWeatherByAirportCode(string airportCode)
    {
        if (string.IsNullOrEmpty(airportCode))
            return BadRequest("Código de aeroporto Inválido");
            
        var query = new GetWeatherByAirportQuery { AirportICAOCode = airportCode };
        
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }
}
