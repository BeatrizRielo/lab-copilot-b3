using PortfolioApi.Models;

namespace PortfolioApi.Dtos;

public record OrdemCreateDto(int AtivoId, TipoOrdem Tipo, int Quantidade, decimal Preco, DateTime? Data);

public record OrdemUpdateDto(TipoOrdem Tipo, int Quantidade, decimal Preco, DateTime? Data);

public record OrdemDto(int Id, int AtivoId, string Ticker, TipoOrdem Tipo, int Quantidade, decimal Preco, DateTime Data);
