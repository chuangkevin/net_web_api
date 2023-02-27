using Azure.Core.Pipeline;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System.Data;
using WebAPI.IServices;
using WebAPI.Services;
using static WebApi.Data.MsSQLDataAccess;
using Assert = NUnit.Framework.Assert;
using Moq;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Connections;

namespace TestProject_NUnit
{
    [TestFixture]
    public class DatabaseTest
    {
        public Mock<ICustomerService> _MockCustomerServices { get; set; }


        public Int64 ExpectedSN = 0;
        public CCUSTOMER ExceptedCustomer = null;

        [SetUp]
        public void Setup()
        {
            _MockCustomerServices = new Mock<ICustomerService>();
            ExpectedSN = 99;
            ExceptedCustomer = new CCUSTOMER("Kevin", "0928000999");

        }

        [Test]
        public async Task Test_SaveCustomer_Create()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock
                .Setup(cs => cs.SaveCustomer(It.IsAny<CCUSTOMER>()))
                .ReturnsAsync(1);


            var repo = customerServiceMock.Object;

            var customerToCreate = new CCUSTOMER("Kevin", "0928000999");

            // Act
            var result = await repo.SaveCustomer(customerToCreate);

            // Assert
            Assert.AreEqual(1, result);
        }


        [Test]
        public async Task Test_SaveCustomer_Update()
        {
            // Arrange
            var existingCustomer = new CCUSTOMER { NAME = "Kevin", PHONE_NO = "0928000999" };
            var customerToUpdate = new CCUSTOMER { NAME = "John", PHONE_NO = "0912345678" };

            _MockCustomerServices
                .Setup(cs => cs.GetCustomerBySN(existingCustomer.SN))
                .ReturnsAsync(existingCustomer);

            _MockCustomerServices
                .Setup(cs => cs.SaveCustomer(customerToUpdate))
                .ReturnsAsync(1);

            var customerService = _MockCustomerServices.Object;

            // Act
            var result = await customerService.SaveCustomer(customerToUpdate);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task GetCustomers_ShouldReturnCustomers()
        {
            // Arrange
            var customers = new List<CCUSTOMER> { new CCUSTOMER { SN = 1, NAME = "Alice" } };
            _MockCustomerServices.Setup(cs => cs.GetCustomers()).ReturnsAsync(customers);

            var customerService = _MockCustomerServices.Object;

            // Act
            var result = await customerService.GetCustomers();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetCustomerBySN()
        {
            // Arrange
            var expectedSN = 1;
            var customerToSave = new CCUSTOMER { NAME = "Kevin", PHONE_NO = "0928000999" };
            _MockCustomerServices
                .Setup(cs => cs.SaveCustomer(customerToSave))
                .ReturnsAsync(expectedSN);

            _MockCustomerServices
                .Setup(cs => cs.GetCustomerBySN(1))
                .ReturnsAsync(customerToSave);

           
            // Act
            var result = await _MockCustomerServices.Object.GetCustomerBySN(expectedSN);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.NAME, Is.EqualTo(customerToSave.NAME));
            Assert.That(result.PHONE_NO, Is.EqualTo(customerToSave.PHONE_NO));

        }


        [Test]
        public async Task GetCustomers_Alive_ShouldNotReturnDeletedCustomers()
        {
            // Arrange
            var repo = _MockCustomerServices.Object;

            // Act
            var customers = await repo.GetCustomers_Alive();

            // Assert
            customers.Should().NotContain(c => c.IS_DELETE);
        }


        [Test]
        public async Task Test_DeleteCustomer_SetState()
        {
            var repo = _MockCustomerServices.Object;

            var customerToCreate = (CCUSTOMER)ExceptedCustomer.Clone();

            var entity = await repo.SaveCustomer(customerToCreate);
            var isSuccess = entity > 0 ? true : false;

            if (isSuccess)
            {
                var isDeleteSuccess = await repo.DeleteCustomer_SET_STATE(entity);

                //上述條件該為true
                Assert.IsTrue(isDeleteSuccess);
            }
        }


        [Test]
        public async Task Test_DeleteCustomer_SQL_DELETE_Success()
        {
            // Arrange
            var repo = _MockCustomerServices.Object;
            var customerToSave = (CCUSTOMER)ExceptedCustomer.Clone();
            _MockCustomerServices
            .Setup(cs => cs.SaveCustomer(customerToSave))
            .ReturnsAsync(1);

            _MockCustomerServices.Setup(cs => cs.DeleteCustomer_SQL_DELETE(1)).ReturnsAsync(true);


            // Act
            var tryDelete = await repo.DeleteCustomer_SQL_DELETE(1);

            // Assert
            tryDelete.Should().BeTrue();
        }


    }
}
