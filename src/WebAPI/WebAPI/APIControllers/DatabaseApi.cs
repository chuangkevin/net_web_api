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

        /// <summary>
        /// Return all customers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<CCUSTOMER>> GetCustomers([FromServices] ICustomerService customerService)
        {
            return await customerService.GetCustomers();
        }

        /// <summary>
        /// Return specific customer by SN.
        /// </summary>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CCUSTOMER> GetCustomersBySN([FromServices] ICustomerService customerService, Int64 customerSN)
        {
            return await customerService.GetCustomerBySN(customerSN);
        }

        /// <summary>
        /// Create customer with name and phone number.
        /// </summary>
        /// <param name="customerService"></param>
        /// <param name="requestParam"></param>
        /// <param name="NAME"></param>
        /// <param name="PHONE_NO"></param>
        ///  /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "NAME": "Kevin",
        ///        "PHONE_NO": "0912345678"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<Int64> CreateCustomer([FromServices] ICustomerService customerService, [FromBody] JsonElement requestParam)
        {
            CCUSTOMER toCreate = new CCUSTOMER(requestParam.ToString());

            return await customerService.SaveCustomer(toCreate);
        }

        /// <summary>
        /// Update specific customer's info by SN.
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "SN":18,
        ///        "NAME": "Kevin",
        ///        "PHONE_NO": "0912345678"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPut]
        public async Task<Int64> UpdateCustomer([FromServices] ICustomerService customerService,[FromBody] JsonElement requestParam)
        {
            CCUSTOMER toCreate = new CCUSTOMER(requestParam.ToString());
            

            return await customerService.SaveCustomer(toCreate);
        }

        /// <summary>
        /// Delete specific customer by SN.
        /// </summary>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteCustomer([FromServices] ICustomerService customerService, Int64 customerSN)
        {
            return await customerService.DeleteCustomer_SET_STATE(customerSN);
        }
    }
}
