using Xunit;
using Moq;
using MediatR;
using TesteApiBrasil.Controllers;
using Microsoft.AspNetCore.Mvc;

public class CityAirportWeatherControllerTests
{
    [Fact]
    public async Task GetWeatherByCityName_Returns_OkResult()
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<CityAirportWeatherController>>();
        var controller = new CityAirportWeatherController(mediatorMock.Object, loggerMock.Object);
        var cityName = "São Paulo";

        var result = await controller.GetWeatherByCityName(cityName);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetWeatherByAirportCode_Returns_OkResult()
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<CityAirportWeatherController>>();
        var controller = new CityAirportWeatherController(mediatorMock.Object, loggerMock.Object);
        var codigoAeroportoCongonhas = "CGH";

        var result = await controller.GetWeatherByAirportCode(codigoAeroportoCongonhas);

        Assert.IsType<OkObjectResult>(result);
    }
}
