using Exchange.Rates.Tests.Services.Factories;
using System.Net.Http;
using Xunit;

namespace Exchange.Rates.Tests.Services.Fixtures
{
    /// <summary>
    /// Base Controller tests IClassFixture
    /// </summary>
    public class ControllerEcbTestsBase : IClassFixture<WebApiEcbTestFactory>
    {
        protected HttpClient Client;

        public ControllerEcbTestsBase(WebApiEcbTestFactory factory)
        {
            Client = factory.CreateClient();
        }
    }
}