using PortfolioApi.Validation;

namespace PortfolioApi.Tests;

public class TickerValidatorTests
{
    [Theory]
    [InlineData("PETR4")]
    [InlineData("MXRF11")]
    [InlineData("BOVA11")]
    [InlineData("TAEE11")]
    [InlineData("VALE3")]
    public void IsValid_TickersValidos_RetornaTrue(string ticker)
    {
        Assert.True(TickerValidator.IsValid(ticker));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("PETR")]      // sem dígito
    [InlineData("PE4")]       // poucas letras
    [InlineData("PETROBRAS")] // sem dígito e longo demais
    [InlineData("12345")]     // só números
    public void IsValid_TickersInvalidos_RetornaFalse(string? ticker)
    {
        Assert.False(TickerValidator.IsValid(ticker));
    }

    [Fact]
    public void Normalize_RemoveEspacosEConverteMaiusculas()
    {
        Assert.Equal("PETR4", TickerValidator.Normalize("  petr4 "));
    }
}
