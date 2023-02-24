using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.IServices
{

    public interface ICustomerService
    {
        public Task<IEnumerable<CCUSTOMER>> GetCustomers();
        public Task<IEnumerable<CCUSTOMER>> GetCustomers_Alive();
        public Task<CCUSTOMER> GetCustomerBySN(Int64 SN);
        public Task<Int64> SaveCustomer(CCUSTOMER sales);
        public Task<bool> DeleteCustomer_SQL_DELETE(Int64 SN);
        public Task<bool> DeleteCustomer_SET_STATE(Int64 SN);
    }

}
