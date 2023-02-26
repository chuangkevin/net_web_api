using Microsoft.AspNetCore.Components;
using WebAPI.IServices;
using WebAPI.Services;
using static WebApi.Data.MsSQLDataAccess;

namespace WebAPI.Pages
{
    public partial class CustomerMgr
    {
        public IEnumerable<CCUSTOMER> ForBind_CustomerEntries;
        //[Inject]
        //public ICustomerService CustomerServices { get; set; }
        public bool SeeOnlyValid { get; set; }

        [Inject]
        public IApiService ApiCustomerServices { get; set; }

        public CCUSTOMER ForBind_Customer = new CCUSTOMER();
        protected override async Task OnInitializedAsync()
        {
            await this.GetCustomers();
        }


        protected async Task GetCustomers()
        {
            if (SeeOnlyValid)
                ForBind_CustomerEntries = await ApiCustomerServices.GetCustomers_Alive();
            else
                ForBind_CustomerEntries = await ApiCustomerServices.GetCustomers();
        }

        protected async Task SaveCustomer()
        {
            await ApiCustomerServices.CreateCustomer(ForBind_Customer);
            await this.GetCustomers();
            this.ClearAll();
            StateHasChanged();
        }
        protected async Task UpdateCustomer()
        {
            await ApiCustomerServices.UpdateCustomer(ForBind_Customer);
            await this.GetCustomers();
            this.ClearAll();
        }
        protected async Task GetCustomerBySN(Int64 customerSN)
        {
            ForBind_Customer = await ApiCustomerServices.GetCustomerBySN(customerSN);
            ForBind_Customer.IsUpdate = true;
            await this.GetCustomers();
        }
        protected async Task DeleteSales_SetState(Int64 customerSN)
        {
            await ApiCustomerServices.DeleteCustomer(customerSN);
            await this.GetCustomers();
        }

        protected async Task DeleteSales_DeleteAlways(Int64 customerSN)
        {
            await ApiCustomerServices.DeleteCustomer(customerSN);
            await this.GetCustomers();
        }
        public void ClearAll()
        {
            ForBind_Customer = new CCUSTOMER();
        }
    }
}
