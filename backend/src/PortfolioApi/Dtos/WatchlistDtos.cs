namespace PortfolioApi.Dtos;

public record WatchlistCreateDto(string Ticker, decimal PrecoAlvo);

public record WatchlistUpdateDto(string Ticker, decimal PrecoAlvo);

public record WatchlistDto(int Id, string Ticker, decimal PrecoAlvo, DateTime CriadoEm);
