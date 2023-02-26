using System.Net.Http;
using System.Threading.Tasks;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.IServices
{
    public interface IApiService
    {
        public Task<IEnumerable<CCUSTOMER>> GetCustomers();
        public Task<IEnumerable<CCUSTOMER>> GetCustomers_Alive();
        public Task<CCUSTOMER> GetCustomerBySN(Int64 customerSN);
        public Task<Int64> CreateCustomer(CCUSTOMER targetCustomer);
        public Task<Int64> UpdateCustomer(CCUSTOMER targetCustomer);
        public Task<bool> DeleteCustomer(Int64 customerSN);


    }
}
