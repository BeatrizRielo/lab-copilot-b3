namespace PortfolioApi.Services;

/// <summary>
/// Exceção de regra de negócio. Mapeada para HTTP 400/404 nos controllers.
/// </summary>
public class RegraNegocioException : Exception
{
    public RegraNegocioException(string message) : base(message) { }
}

public class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string message) : base(message) { }
}
