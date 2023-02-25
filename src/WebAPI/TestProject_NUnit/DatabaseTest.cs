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

namespace TestProject_NUnit
{
    [TestFixture]
    public class DatabaseTest
    {
        [Inject]
        public ICustomerService CustomerServices { get; set; }

        private string _ConnectionString => @$"Server={_Server};Database={_Database};User Id={_user}; Password={_pwd};Trusted_Connection=True;Integrated Security=false;TrustServerCertificate=true;MultipleActiveResultSets=true;";
        private string _Database => "WebApiDatabase";
        private string _Server => "192.168.195.55";
        private string _user => "webapi";
        private string _pwd => "!QAZ@WSX";

        public Int64 ExceptedSN = 0;
        public CCUSTOMER ExceptedCustomer = null;

        [SetUp]
        public void Setup()
        {
            ExceptedSN = 99;
            ExceptedCustomer = new CCUSTOMER("Kevin", "0928000999");

        }

        [Test]
        public async Task Test_SaveCustomer_Create()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);


            var customerToCreate = (CCUSTOMER)ExceptedCustomer.Clone();

            var entity = await repo.SaveCustomer(customerToCreate);

            //如果有正確操作，則return SN 會>0
            var isSuccess = entity > 0 ? true : false;



            //上述條件該為False
            Assert.IsTrue(isSuccess);

        }

        [Test]
        public async Task Test_SaveCustomer_Update()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);


            var customerToCreate = (CCUSTOMER)ExceptedCustomer.Clone();
            customerToCreate.SN = 5;

            var retSN = await repo.SaveCustomer(customerToCreate);

            //如果有正確操作，則return SN 會>0
            var isSuccess = retSN > 0 ? true : false;


            //上述條件該為False
            Assert.IsTrue(isSuccess);
        }


        [Test]
        public async Task Test_GetAllCustomer()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);

            var customers = await repo.GetCustomers();

            //如果有資料，則隨便一筆的SN都該>0
            var snValid = customers.FirstOrDefault()?.SN > 0;

            Assert.IsTrue(snValid);
        }

        [Test]
        public async Task Test_GetCustomerBySN()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);
          
            var customerToCreate = (CCUSTOMER)ExceptedCustomer.Clone();
            

            var entity = await repo.SaveCustomer(customerToCreate);

            var customer = await repo.GetCustomerBySN(entity);

            var retSN = customer?.SN;


            Assert.AreEqual(entity, retSN);
        }

        [Test]
        public async Task Test_GetCustomer_Alive()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);

            var customers = await repo.GetCustomers_Alive();

            //找出來的資料，有包含已設定為刪除的
            var hasNotValid = customers.Any(cu => cu.IS_DELETE);

            //上述條件該為False
            Assert.IsFalse(hasNotValid);
        }



        [Test]
        public async Task Test_DeleteCustomer_SetState()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);

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
        public async Task Test_DeleteCustomer_SQL_Delete()
        {
            var mockerDapper = new Mock<ICustomerService>();

            var expectedConnection = _ConnectionString;

            var repo = new CustomerService(expectedConnection, CustomerServices);

            var customerToCreate = (CCUSTOMER)ExceptedCustomer.Clone();
            customerToCreate.SN = ExceptedSN;

            var entity = await repo.SaveCustomer(customerToCreate);

            var isDeleteSuccess = await repo.DeleteCustomer_SQL_DELETE(entity);

            //上述條件該為true
            Assert.IsTrue(isDeleteSuccess);

        }



    }
}
