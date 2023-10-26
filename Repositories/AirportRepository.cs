using Dapper;

public interface IAirportRepository
{
    Task Create(DadosMeteorologicos dadosClima);
}

public class AirportRepository : IAirportRepository
{
    private DataContext _context;
    public AirportRepository(DataContext context)
    {
       _context = context; 
    }
    public async Task Create(DadosMeteorologicos dadosMeteorologicos)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            INSERT INTO DadosMeteorologicos (codigo_icao, atualizado_em, pressao_atmosferica, 
                 vento, direcao_vento, umidade, condicao, condicao_Desc, temp)
            VALUES (@codigo_icao, @atualizado_em, @pressao_atmosferica, 
                 @vento, @direcao_vento, @umidade, @condicao, @condicao_Desc, @temp);
        """;

        var response = await connection.ExecuteAsync(sql, dadosMeteorologicos);
    }
}
