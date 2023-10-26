public class DadosMeteorologicos
{
    public required string Codigo_icao { get; set; }
    public required DateTime? Atualizado_em { get; set; }
    public required string? Pressao_atmosferica { get; set; }
    public required int Vento { get; set; }
    public required int Direcao_vento { get; set; }
    public required int Umidade { get; set; }
    public required string Condicao { get; set; }
    public required string Condicao_Desc { get; set; }
    public int Temp { get; set; }
}
