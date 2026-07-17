using PortfolioApi.Models;

namespace PortfolioApi.Dtos;

public record AtivoCreateDto(string Ticker, TipoAtivo Tipo, int Quantidade, decimal PrecoMedio);

public record AtivoUpdateDto(string Ticker, TipoAtivo Tipo, int Quantidade, decimal PrecoMedio);

public record AtivoDto(int Id, string Ticker, TipoAtivo Tipo, int Quantidade, decimal PrecoMedio);
