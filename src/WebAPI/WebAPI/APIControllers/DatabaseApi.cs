using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.IServices;
using WebAPI.Services;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DatabaseApi : ControllerBase
    {

        [HttpGet]
        public async Task<IEnumerable<CCUSTOMER>> GetCustomers([FromServices] ICustomerService customerService)
        {
            return await customerService.GetCustomers();
        }

        [HttpGet]
        public async Task<CCUSTOMER> GetCustomersBySN([FromServices] ICustomerService customerService, Int64 customerSN)
        {
            return await customerService.GetCustomerBySN(customerSN);
        }
        [HttpPost]
        public string CreateCustomer([FromBody] JsonElement requestParam)
        {
            string ret = "";

            CCUSTOMER toCreate = new CCUSTOMER();

            return ret;
        }

        [HttpPut]
        public string UpdateCustomer()
        {
            string ret = "";

            return ret;
        }

        [HttpDelete]
        public string DeleteCustomer()
        {
            string ret = "";

            return ret;
        }
    }
}
