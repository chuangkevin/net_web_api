using Microsoft.AspNetCore.Components;
using WebAPI.IServices;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.Pages
{
    public partial class CustomerMgr
    {
        public IEnumerable<CCUSTOMER> ForBind_CustomerEntries;
        [Inject]
        public ICustomerService CustomerServices { get; set; }

        public CCUSTOMER ForBind_Customer = new CCUSTOMER();
        protected override async Task OnInitializedAsync()
        {
            await this.GetCustomers();
        }
        protected async Task GetCustomers()
        {
            ForBind_CustomerEntries = await CustomerServices.GetCustomers();
        }
        protected async Task SaveCustomer()
        {
            await CustomerServices.SaveCustomer(ForBind_Customer);
            await this.GetCustomers();
            this.ClearAll();
        }
        protected async Task UpdateCustomer()
        {
            await CustomerServices.SaveCustomer(ForBind_Customer);
            await this.GetCustomers();
            this.ClearAll();
        }
        protected async Task GetCustomerBySN(Int64 customerSN)
        {
            ForBind_Customer = await CustomerServices.GetCustomerBySN(customerSN);
            ForBind_Customer.IsUpdate = true;
            await this.GetCustomers();
        }
        protected async Task DeleteSales_SetState(Int64 customerSN)
        {
            await CustomerServices.DeleteCustomer_SET_STATE(customerSN);
            await this.GetCustomers();
        }

        protected async Task DeleteSales_DeleteAlways(Int64 customerSN)
        {
            await CustomerServices.DeleteCustomer_SQL_DELETE(customerSN);
            await this.GetCustomers();
        }
        public void ClearAll()
        {
            ForBind_Customer.NAME = "";
            ForBind_Customer.PHONE_NO = "";
            ForBind_Customer.IS_DELETE = false;
            ForBind_Customer.UPDATE_DATE = null;
        }
    }
}
