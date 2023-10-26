public class ClimaDia
{
    public required string Data { get; set; }
    public required string Condicao { get; set; }
    public float Min { get; set; }
    public float Max { get; set; }

    public float Indice_uv { get; set; }
    public required string Condicao_desc { get; set; }
}

public class DadosClima
{
    public required string Cidade { get; set; }
    public required string Estado { get; set; }
    public required string Atualizado_em { get; set; }
    public required List<ClimaDia> Clima { get; set; }
}
