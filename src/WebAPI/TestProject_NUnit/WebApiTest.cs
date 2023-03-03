using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPI.APIControllers;
using WebAPI.IServices;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.Tests
{
    
    //[TestFixture]
    //public class DatabaseApiControllerTests
    //{
    //    private Mock<ICustomerService> _mockCustomerService;
    //    private DatabaseApiController _databaseApiController;
    //    private const string _apiKey = "KevinChuang";

    //    [SetUp]
    //    public void Setup()
    //    {
    //        _mockCustomerService = new Mock<ICustomerService>();
    //        _databaseApiController = new DatabaseApiController()
    //        {
    //            ControllerContext = new ControllerContext()
    //            {
    //                HttpContext = new DefaultHttpContext()
    //            }
    //        };
    //    }

    //    [Test]
    //    public async Task GetCustomers_WithValidApiKey_ReturnsOk()
    //    {
    //        // Arrange
    //        var customers = new List<CCUSTOMER>()
    //        {
    //            new CCUSTOMER() { NAME = "Customer1", PHONE_NO = "0912345678" },
    //            new CCUSTOMER() { NAME = "Customer2", PHONE_NO = "0223456789" },
    //        };
    //        _mockCustomerService.Setup(x => x.GetCustomers()).ReturnsAsync(customers);

    //        // Set the API key in the request header
    //        _databaseApiController.ControllerContext.HttpContext.Request.Headers.Add("X-API-KEY", _apiKey);

    //        // Act
    //        var ret = await _databaseApiController.GetCustomers(_mockCustomerService.Object);

    //        // Assert
    //        Assert.IsInstanceOf<OkObjectResult>(ret.Result);
    //        Assert.AreEqual(customers, (ret.Result as OkObjectResult).Value);
    //    }

    //    [Test]
    //    public async Task GetCustomers_WithInvalidApiKey_ReturnsUnauthorized()
    //    {
    //        // Arrange
    //        var apiKey = "InvalidApiKey";
    //        var customers = new List<CCUSTOMER>()
    //        {
    //            new CCUSTOMER() { NAME = "Customer1", PHONE_NO = "0912345678" },
    //            new CCUSTOMER() { NAME = "Customer2", PHONE_NO = "0223456789" },
    //        };
    //        _mockCustomerService.Setup(x => x.GetCustomers()).ReturnsAsync(customers);

    //        // Set the API key in the request header
    //        _databaseApiController.ControllerContext.HttpContext.Request.Headers.Add("X-API-KEY", apiKey);

    //        // Act
    //        var ret = await _databaseApiController.GetCustomers(_mockCustomerService.Object);

    //        // Assert
    //        Assert.IsInstanceOf<UnauthorizedResult>(ret.Result);
    //    }

    //    [Test]
    //    public async Task GetCustomersBySN_ReturnsNotFoundResult_WhenApiKeyIsValidAndCustomerDoesNotExist()
    //    {
    //        // Arrange
    //        const long nonExistingCustomerSN = 999;

    //        // Set the API key in the request header
    //        _databaseApiController.ControllerContext.HttpContext.Request.Headers.Add("X-API-KEY", _apiKey);

    //        // Act
    //        var ret = await _databaseApiController.GetCustomersBySN(_mockCustomerService.Object, nonExistingCustomerSN);

    //        // Assert
    //        Assert.That(ret, Is.TypeOf<NotFoundResult>());
    //    }

    //}
    [TestFixture]
    public class DatabaseApiControllerTests
    {
        private WebApplicationFactory<DatabaseApiController> _factory;
        private Mock<ICustomerService> _mockCustomerService;
        private const string _apiKey = "KevinChuang";

        [SetUp]
        public void SetUp()
        {
            // 建立 WebApplicationFactory，用於建立模擬網路環境的測試伺服器
            _factory = new WebApplicationFactory<DatabaseApiController>();
        }

        [TearDown]
        public void TearDown()
        {
            // 釋放 WebApplicationFactory 資源
            _factory.Dispose();
        }

        [Test]
        public async Task GetCustomers_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/DatabaseApi/GetCustomers");
            request.Headers.Add("X-Api-Key", "KevinChuang");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetCustomersAlive_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/DatabaseApi/GetCustomersAlive");
            request.Headers.Add("X-Api-Key", "KevinChuang");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetCustomersBySN_ReturnsNotFoundResult_WhenApiKeyIsValidAndCustomerDoesNotExist()
        {
            // Arrange
            const long nonExistingCustomerSN = 999;
            var client = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/DatabaseApi/GetCustomersBySN/{nonExistingCustomerSN}");
            request.Headers.Add("X-Api-Key", "KevinChuang");


            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }


    }

}