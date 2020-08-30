using Exchange.Rates.Tests.Services.Factories;
using System.Net.Http;
using Xunit;

namespace Exchange.Rates.Tests.Services.Fixtures
{
    /// <summary>
    /// Base Controller tests IClassFixture
    /// </summary>
    public class ControllerCoinCapTestsBase : IClassFixture<WebApiCoinCapTestFactory>
    {
        protected HttpClient Client;

        public ControllerCoinCapTestsBase(WebApiCoinCapTestFactory factory)
        {
            Client = factory.CreateClient();
        }
    }
}