using PortfolioApi.Services;
using Microsoft.EntityFrameworkCore;

namespace PortfolioApi.Middleware;

/// <summary>
/// Converte exceções de domínio em respostas HTTP apropriadas (400/404/409).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NaoEncontradoException ex)
        {
            await EscreverErro(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (RegraNegocioException ex)
        {
            await EscreverErro(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            await EscreverErro(context, StatusCodes.Status409Conflict,
                "O ativo foi alterado por outra operação. Tente novamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado.");
            await EscreverErro(context, StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
        }
    }

    private static async Task EscreverErro(HttpContext context, int status, string mensagem)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { erro = mensagem });
    }
}
