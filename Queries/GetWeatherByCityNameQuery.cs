using MediatR;
using TesteApiBrasil;

public class GetWeatherByCityNameQuery : IRequest<CityWeatherModel>
{
    public required string CityName { get; set; }
}