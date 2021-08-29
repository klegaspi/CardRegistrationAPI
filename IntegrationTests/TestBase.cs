using IntegrationTests.Common;
using System.Net.Http;
using WebApplication4;

namespace IntegrationTests
{
    public class TestBase
    {
        protected readonly HttpClient TestClient;
        public TestBase()
        {
            var factory = new CustomWebApplicationFactory<Startup>();
            TestClient = factory.CreateClient();
        }
    }
}
