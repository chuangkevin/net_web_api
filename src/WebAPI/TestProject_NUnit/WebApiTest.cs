using Microsoft.AspNetCore.Components;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi;
using Microsoft.AspNetCore.Hosting;


namespace TestProject_NUnit
{
    [TestFixture]
    public class WebApiTest
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}