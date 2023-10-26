using MediatR;
using TesteApiBrasil;

public class GetWeatherByAirportQuery : IRequest<AirportWeatherModel>
{
    public required string AirportICAOCode { get; set; }
}