using Dapper;
using Microsoft.Data.SqlClient;
using static WebAPI.Services.CustomerService;
using System.Data;
using WebAPI.IServices;
using WebApi.Data;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.Services
{
    public class CustomerService : ICustomerService
    {

        public IConfiguration _configuration { get; }
        public string _connectionString { get; }

        public CustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DockerDB");
        }
        public async Task<IEnumerable<CCUSTOMER>> GetCustomers()
        {
            IEnumerable<CCUSTOMER> ret;
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    ret = await conn.QueryAsync<CCUSTOMER>("GetCustomers", commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return ret;
        }

        public async Task<CCUSTOMER> GetCustomerBySN(Int64 customerSN)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CustomerSN", customerSN, DbType.Int64);
            CCUSTOMER ret = new CCUSTOMER();

            using (var conn = new SqlConnection(_connectionString))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    ret = await conn.QueryFirstOrDefaultAsync<CCUSTOMER>("GetCustomerBySN", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return ret;
        }

        /// <summary>
        /// 取得狀態不為DELETE的顧客
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CCUSTOMER>> GetCustomers_Alive()
        {
            IEnumerable<CCUSTOMER> ret;
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    ret = await conn.QueryAsync<CCUSTOMER>("GetCustomers_Alive", commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return ret;
        }

        /// <summary>
        /// 可能是Create或是Update，根據Property決定行為
        /// </summary>
        /// <param name="targetCustomer"></param>
        /// <returns></returns>
        public async Task<Int64> SaveCustomer(CCUSTOMER targetCustomer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", targetCustomer.NAME, DbType.String);
            parameters.Add("PhoneNumber", targetCustomer.PHONE_NO, DbType.String);
            parameters.Add("SN", DbType.Int64, direction: ParameterDirection.ReturnValue);
            Int64 ret = -1;

            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    if (targetCustomer.IsUpdate)
                    {
                        parameters.Add("CustomerSN", targetCustomer.SN, DbType.Int64);
                        var tt = await conn.ExecuteScalarAsync("UpdateCustomer", parameters, commandType: CommandType.StoredProcedure);

                        Int64.TryParse(tt.ToString(),out ret);
                    }
                    else
                    {

                        var xx=await conn.ExecuteScalarAsync("CreateCustomer", parameters, commandType: CommandType.StoredProcedure);
                        Int64.TryParse(xx.ToString(), out ret);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return ret;
        }

        /// <summary>
        /// 在SQL裡面真實刪除DB資料
        /// </summary>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCustomer_SQL_DELETE(Int64 customerSN)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CustomerSN", customerSN, DbType.Int64);

            using (var conn = new SqlConnection(_connectionString))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("DeleteCustomer_Delete", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// 把客戶的IS_DELETE欄位更新為False，以免影響關聯Table
        /// </summary>
        /// <param name="customerSN"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCustomer_SET_STATE(Int64 customerSN)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CustomerSN", customerSN, DbType.Int64);

            using (var conn = new SqlConnection(_connectionString))
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("DeleteCustomer_SetState", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }
    }






}

