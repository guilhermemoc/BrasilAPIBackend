using Dapper;

public interface ICityRepository
{
    Task Create(DadosClima dadosClima);
}

public class CityRepository : ICityRepository
{
    private DataContext _context;
    public CityRepository(DataContext context)
    {
       _context = context; 
    }
    public async Task Create(DadosClima dadosClima)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            INSERT INTO PrevisaoClima (cidade, estado, atualizado_em)
            VALUES (@cidade, @estado, @atualizado_em);

            DECLARE @climaId INT;
            SET @climaId = SCOPE_IDENTITY(); 

            INSERT INTO ClimaDia (data, condicao, min, max, indice_uv, condicao_desc, clima_id)
            VALUES (@data1, @condicao1, @min1, @max1, @indice_uv1, @condicao_desc1, @climaId);

            INSERT INTO ClimaDia (data, condicao, min, max, indice_uv, condicao_desc, clima_id)
            VALUES (@data2, @condicao2, @min2, @max2, @indice_uv2, @condicao_desc2, @climaId);
        """;

        var response = await connection.ExecuteAsync(sql, dadosClima);
    }
}
