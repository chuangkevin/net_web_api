using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.APIControllers;
using WebAPI.IServices;
using WebAPI.Services;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.Tests
{
    [TestFixture]
    public class DatabaseApiControllerTests
    {
        private Mock<ICustomerService> _mockCustomerService;
        private DatabaseApiController _databaseApiController;
        private const string _apiKey = "KevinChuang";
        [SetUp]
        public void Setup()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _databaseApiController = new DatabaseApiController()
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task GetCustomers_WithValidApiKey_ReturnsOk()
        {
            // Arrange

            var customers = new List<CCUSTOMER>()
            {
                new CCUSTOMER() {NAME = "Customer1", PHONE_NO = "0912345678" },
                new CCUSTOMER() {NAME = "Customer2", PHONE_NO = "0223456789" },
            };
            _mockCustomerService.Setup(x => x.GetCustomers()).ReturnsAsync(customers);

            // Act
            var ret = await _databaseApiController.GetCustomers(_mockCustomerService.Object, _apiKey);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(ret.Result);
        }

        [Test]
        public async Task GetCustomers_WithInvalidApiKey_ReturnsUnauthorized()
        {
            // Arrange
            var apiKey = "InvalidApiKey";
            var customers = new List<CCUSTOMER>()
            {
                new CCUSTOMER() { NAME = "Customer1", PHONE_NO = "0912345678" },
                new CCUSTOMER() { NAME = "Customer2", PHONE_NO = "0223456789" },
            };
            _mockCustomerService.Setup(x => x.GetCustomers()).ReturnsAsync(customers);

            // Act
            var ret = await _databaseApiController.GetCustomers(_mockCustomerService.Object, apiKey);

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(ret.Result);
        }

        [Test]
        public async Task GetCustomersBySN_ReturnsNotFoundResult_WhenApiKeyIsValidAndCustomerDoesNotExist()
        {
            // Arrange
            const long nonExistingCustomerSN = 999;

            // Act
            var ret = await _databaseApiController.GetCustomersBySN(_mockCustomerService.Object, _apiKey, nonExistingCustomerSN);

            // Assert
            Assert.That(ret, Is.TypeOf<NotFoundResult>());
        }


        //[Test]
        //public async Task CreateCustomer_ReturnsOkObjectResult_WithValidApiKey()
        //{
        //    // Arrange
        //    var jsonElement = JsonDocument.Parse("{\"NAME\": \"John\", \"PHONE_NO\": \"0912345678\"}").RootElement;
        //    var apiKey = "KevinChuang";
        //    var expected = new CCUSTOMER("John","0912345678");
        //    _mockCustomerService.Setup(x => x.SaveCustomer(expected).ReturnsAsync(expected);

        //    // Act
        //    var ret = await _databaseApiController.CreateCustomer(_mockCustomerService.Object, jsonElement, apiKey) as OkObjectResult;
        //    var actual = ret.Value;

        //    // Assert
        //    Assert.That(ret.StatusCode, Is.EqualTo(200));
        //    Assert.That(actual, Is.EqualTo(expected));
        //}

        [Test]
        public async Task CreateCustomer_ReturnsUnauthorizedResult_WithInvalidApiKey()
        {
            // Arrange
            var jsonElement = JsonDocument.Parse("{\"NAME\": \"John\", \"PHONE_NO\": \"0912345678\"}").RootElement;
            var apiKey = "InvalidApiKey";

            // Act
            var ret = await _databaseApiController.CreateCustomer(_mockCustomerService.Object, jsonElement, apiKey);

            // Assert
            Assert.That(ret, Is.TypeOf<UnauthorizedResult>());
        }

        //[Test]
        //public async Task UpdateCustomer_ReturnsOkObjectResult_WithValidApiKey()
        //{
        //    // Arrange
        //    var jsonElement = JsonDocument.Parse("{\"SN\": 1, \"NAME\": \"John\", \"PHONE_NO\": \"0912345678\"}").RootElement;
        //    var apiKey = "KevinChuang";
        //    var expected = new { id = 1, name = "John", phoneNo = "0912345678" };
        //    //_mockCustomerService.Setup(x => x.SaveCustomer(It.IsAny<CCUSTOMER>())).ReturnsAsync(expected);

        //    // Act
        //    var ret = await _databaseApiController.UpdateCustomer(_mockCustomerService.Object, jsonElement, apiKey) as OkObjectResult;
        //    var actual = ret.Value;

        //    // Assert
        //    Assert.That(ret.StatusCode, Is.EqualTo(200));
        //    Assert.That(actual, Is.EqualTo(expected));
        //}

        [Test]
        public async Task UpdateCustomer_ReturnsUnauthorizedResult_WithInvalidApiKey()
        {
            // Arrange
            var jsonElement = JsonDocument.Parse("{\"SN\": 1, \"NAME\": \"John\", \"PHONE_NO\": \"0912345678\"}").RootElement;
            var apiKey = "InvalidApiKey";

            // Act
            var ret = await _databaseApiController.UpdateCustomer(_mockCustomerService.Object, jsonElement, apiKey);

            // Assert
            Assert.That(ret, Is.TypeOf<UnauthorizedResult>());
        }
    }
}
