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
    public class DatabaseApiController : ControllerBase
    {
        private string _apiKey = "KevinChuang";

        /// <summary>
        /// Return all customers.
        /// ApiKey : KevinChuang
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CCUSTOMER>>> GetCustomers(
    [FromServices] ICustomerService customerService,
    [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            if (apiKey == _apiKey)
            {
                var customers = await customerService.GetCustomers();
                return Ok(customers);
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Return all customers.
        /// ApiKey : KevinChuang
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CCUSTOMER>>> GetCustomersAlive(
    [FromServices] ICustomerService customerService,
    [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            if (apiKey == _apiKey)
            {
                var customers = await customerService.GetCustomers_Alive();
                return Ok(customers);
            }
            else
            {
                return Unauthorized();
            }
        }


        /// <summary>
        /// Return specific customer by SN.
        /// ApiKey : KevinChuang
        /// </summary>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        [HttpGet("{customerSN}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCustomersBySN([FromServices] ICustomerService customerService
            , [FromHeader(Name = "X-Api-Key")] string apiKey
            , Int64 customerSN)
        {
            if (apiKey == _apiKey)
            {
                var customer = await customerService.GetCustomerBySN(customerSN);
                if (customer != null)
                {
                    return Ok(customer);
                }
                return NotFound();
            }
            return Unauthorized();
        }


        /// <summary>
        /// Create customer with name and phone number.
        /// ApiKey : KevinChuang
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromServices] ICustomerService customerService
            , [FromBody] JsonElement requestParam
            , [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            if (apiKey == _apiKey)
            {
                CCUSTOMER toCreate = new CCUSTOMER(requestParam.ToString(),false);
                var result = await customerService.SaveCustomer(toCreate);
                return Ok(result);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Update specific customer's info by SN.
        /// ApiKey : KevinChuang
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCustomer([FromServices] ICustomerService customerService
            , [FromBody] JsonElement requestParam
            , [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            if (apiKey == _apiKey)
            {
                CCUSTOMER toCreate = new CCUSTOMER(requestParam.ToString());
                var result = await customerService.SaveCustomer(toCreate);
                return Ok(result);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Delete specific customer by SN.
        /// ApiKey : KevinChuang
        /// </summary>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        [HttpDelete("{customerSN}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCustomer([FromServices] ICustomerService customerService
            , [FromHeader(Name = "X-Api-Key")] string apiKey
            , Int64 customerSN)
        {
            if (apiKey == _apiKey)
            {
                var isDeleted = await customerService.DeleteCustomer_SET_STATE(customerSN);
                return Ok(isDeleted);
            }

            return Unauthorized();
        }
    }
}
