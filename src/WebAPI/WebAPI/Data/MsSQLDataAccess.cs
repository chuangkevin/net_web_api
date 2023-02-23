namespace WebApi.Data
{
    public class MsSQLDataAccess
    {
        public class CCUSTOMER
        {
            public long SN { get; set; }

            public string NAME { get; set; }

            public string PHONE_NO { get; set; }

            public bool IS_DELETE { get; set; }

            public DateTime CREATE_DATE { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public bool IsUpdate { get; set; } = false;

        }



        public class CHOUSE_FOR_SALE
        {
            public long SN { get; set; }

            public long CUSTOMER_SN { get; set; }

            public decimal? PRICE { get; set; }

            public string ADDRESS { get; set; }

            public DateTime CREATE_DATE { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public DateTime? SOLD_DATE { get; set; }

            public bool? IS_DELETE { get; set; }

            public bool IsUpdate { get; set; } = false;
        }
    }
}