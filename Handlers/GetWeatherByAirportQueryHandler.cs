using MediatR;
using Newtonsoft.Json;
using SDKBrasilAPI;
using TesteApiBrasil;

public class GetWeatherByAirportQueryHandler : IRequestHandler<GetWeatherByAirportQuery, AirportWeatherModel>
{
    private IBrasilAPI _brasilApi;
    private IAirportRepository _airportRepository;
    public GetWeatherByAirportQueryHandler(IBrasilAPI brasilApi, AirportRepository airportRepository)
    {
        _brasilApi = brasilApi;
        _airportRepository = airportRepository;
    }

    public async Task<AirportWeatherModel> Handle(GetWeatherByAirportQuery request, CancellationToken cancellationToken)
    {
        var responseApi = await _brasilApi.CptecClimaAeroporto(request.AirportICAOCode);

        if (responseApi != null && responseApi.Climas != null)
        {
            Console.WriteLine($"resposta API Brasil para aeroporto {request.AirportICAOCode}: {JsonConvert.SerializeObject(responseApi)}");
            
            var item = responseApi.Climas.FirstOrDefault();

            if (item != null)
            {
                var entity = new DadosMeteorologicos
                {
                    Atualizado_em = item.AtualizadoEm,
                    Codigo_icao = item.CodigoIcao,
                    Condicao = item.Condicao,
                    Condicao_Desc = item.CondicaoDesc,
                    Direcao_vento = item.DirecaoVento ?? 0,
                    Umidade = item.Umidade ?? 0,
                    Temp = item.Temp ?? 0,
                    Pressao_atmosferica = item.PressaoAtmosferica != null
                        ? Convert.ToString(item.PressaoAtmosferica) 
                        : string.Empty,
                    Vento = item.Vento ?? 0,
                };

                await _airportRepository.Create(entity);

                var stringWeather = String.Concat(
                $"Informações para {request.AirportICAOCode}: ", 
                $"Pressão Atmosférica {item.PressaoAtmosferica}, ",
                $"Condição {item.CondicaoDesc}, ",
                $"Vento {item.Vento}, ",
                $"Direção do Vento {item.Vento}, ",
                $"Umidade {item.Umidade}, ",
                $"Temperatura {item.Temp}ºC. ",
                $"Atualizado em: {item.AtualizadoEm}");

                var response = new AirportWeatherModel { WeatherResult = stringWeather };

                return await Task.FromResult(response);
            }
        }

        return new AirportWeatherModel { WeatherResult = $"Sem resultados para {request.AirportICAOCode}" };
    }
}