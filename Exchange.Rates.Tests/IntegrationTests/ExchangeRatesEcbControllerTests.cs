using Exchange.Rates.Tests.Services;
using Exchange.Rates.Tests.Services.Factories;
using Exchange.Rates.Tests.Services.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace Exchange.Rates.Tests.IntegrationTests
{
    public class ExchangeRatesEcbControllerTests : ControllerEcbTestsBase
    {
        private const string BASE_URL = "/api/exchangeratesecb/";
        private readonly HttpClientHelper _httpClientHelper;

        public ExchangeRatesEcbControllerTests(WebApiEcbTestFactory factory)
            : base(factory)
        {
            _httpClientHelper = new HttpClientHelper(Client);
        }

        [Fact]
        public async Task UsdBaseRatesTest()
        {
            var symbols = "EUR,CHF,CZK";
            var result = await _httpClientHelper.GetAsync<string>(BASE_URL + $"usdbaserates?symbols={symbols}").ConfigureAwait(false);
            Assert.NotNull(result);
        }
    }
}
