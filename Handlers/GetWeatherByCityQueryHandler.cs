using MediatR;
using SDKBrasilAPI;
using TesteApiBrasil;
using Newtonsoft.Json;

public class GetWeatherByCityQueryHandler : IRequestHandler<GetWeatherByCityNameQuery, CityWeatherModel>
{
    private IBrasilAPI _brasilApi;
    private ICityRepository _cityRepository;
    public GetWeatherByCityQueryHandler(IBrasilAPI brasilApi, CityRepository cityRepository)
    {
        _brasilApi = brasilApi;
        _cityRepository = cityRepository;
    }

    public async Task<CityWeatherModel> Handle(GetWeatherByCityNameQuery request, CancellationToken cancellationToken)
    {
        var cityResponse = await _brasilApi.CptecCidade(request.CityName);

        var cityId = cityResponse.Cidades.Any() ? cityResponse.Cidades.First().id : 0;

        var cityEstado = cityResponse.Cidades.Any() ? cityResponse.Cidades.First().estado : string.Empty;

        var cityWeatherResponse = await _brasilApi.CptecClimaPrevisao(cityId);

        if (cityWeatherResponse != null)
        {
            var cityWeather = cityWeatherResponse.Clima.FirstOrDefault();

            if (cityWeather != null)
            {
                var entity = new DadosClima
                {
                    Atualizado_em = cityWeatherResponse.AtualizadoEm,
                    Cidade = cityWeatherResponse.Cidade,
                    Clima = new List<ClimaDia> {
                        new ClimaDia 
                        {
                            Condicao = cityWeather.Condicao,
                            Condicao_desc = cityWeather.CondicaoDesc,
                            Data = cityWeather.Data,
                            Max = cityWeather.Max ?? 0,
                            Min = cityWeather.Min ?? 0,
                            Indice_uv = cityWeather.IndiceUv ?? 0
                        }
                    },
                    Estado = cityEstado 
                };

                await _cityRepository.Create(entity);

                Console.WriteLine($"resposta API Brasil para cidade {request.CityName}: {JsonConvert.SerializeObject(cityWeather)}");
                
                var stringWeather = String.Concat(
                    $"{cityWeather.CondicaoDesc} em {request.CityName},",
                    $"no dia {cityWeather.Data},",
                    $"com Máx/Min {cityWeather.Max}ºC/{cityWeather.Min}ºC");

                var response = new CityWeatherModel
                {
                    WeatherResult = stringWeather
                };

                return response;
            }
        }
        
        Console.WriteLine($"sem informações da API Brasil para cidade: {request.CityName}");
        
        return new CityWeatherModel { WeatherResult = $"Sem resultados para {request.CityName}"}; 
    }
}