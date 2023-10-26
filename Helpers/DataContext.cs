using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

public class DataContext
{
    private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Server={_dbSettings.Server}; Database={_dbSettings.Database}; User Id={_dbSettings.UserId}; Password={_dbSettings.Password};";
        return new SqlConnection(connectionString);
    }

    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    private async Task _initDatabase()
    {
        var connectionString = $"Server={_dbSettings.Server}; Database=master; User Id={_dbSettings.UserId}; Password={_dbSettings.Password};";
        using var connection = new SqlConnection(connectionString);
        var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_dbSettings.Database}') CREATE DATABASE [{_dbSettings.Database}];";
        await connection.ExecuteAsync(sql);
    }

    private async Task _initTables()
    {
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sql = """

            IF OBJECT_ID('DadosMeteorologicos', 'U') IS NULL

            CREATE TABLE DadosMeteorologicos (
            id INT PRIMARY KEY IDENTITY(1,1),
            codigo_icao NVARCHAR(255),
            atualizado_em DATETIME,
            pressao_atmosferica NVARCHAR(10),
            vento INT,
            direcao_vento INT,
            umidade INT,
            condicao NVARCHAR(2),
            condicao_Desc NVARCHAR(255),
            temp INT
            );

            IF OBJECT_ID('DadosClima', 'U') IS NULL

            CREATE TABLE DadosClima (
            id INT PRIMARY KEY IDENTITY(1,1),
            cidade NVARCHAR(255),
            estado NVARCHAR(2),
            atualizado_em DATE
            );

            IF OBJECT_ID('ClimaDia', 'U') IS NULL

            CREATE TABLE ClimaDia (
            id INT PRIMARY KEY IDENTITY(1,1),
            data DATE,
            condicao NVARCHAR(2),
            min FLOAT,
            max FLOAT,
            indice_uv FLOAT,
            condicao_desc NVARCHAR(255),
            clima_id INT,
            FOREIGN KEY (clima_id) REFERENCES DadosClima(id)
            );

            """;
            await connection.ExecuteAsync(sql);
        }
    }
}