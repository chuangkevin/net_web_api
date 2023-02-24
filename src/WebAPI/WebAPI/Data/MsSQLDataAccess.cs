using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Data
{
    public class MsSQLDataAccess
    {

        public class CCUSTOMER
        {

            public long SN { get; set; }

            [Required(ErrorMessage = "請輸入姓名")]
            [StringLength(32, ErrorMessage = "字數過長")]
            public string NAME { get; set; }

            [RegularExpression(@"(\d{2,3}-?|\(\d{2,3}\))\d{3,4}-?\d{4}|09\d{2}(\d{6}|-\d{3}-\d{3})", ErrorMessage = "請輸入09開頭的手機，或是02開頭的市話")]
            public string PHONE_NO { get; set; }

            public bool IS_DELETE { get; set; }

            public DateTime CREATE_DATE { get; set; }

            public DateTime? UPDATE_DATE { get; set; }

            public bool IsUpdate { get; set; } = false;

            public CCUSTOMER()
            {
                IsUpdate = false;

            }
            public CCUSTOMER(string customerName, string customerPhoneNo)
                : this()
            {
                this.NAME = customerName;
                this.PHONE_NO = customerPhoneNo;
            }
            public CCUSTOMER(string jsonstring, bool isUpdate = true)
                : this()
            {
                var t = JsonConvert.DeserializeObject<CCUSTOMER>(jsonstring);
                this.SN = t.SN;
                this.NAME = t.NAME;
                this.PHONE_NO = t.PHONE_NO;
                this.IS_DELETE = t.IS_DELETE;
                this.CREATE_DATE = t.CREATE_DATE;
                this.UPDATE_DATE = t.UPDATE_DATE;
                this.IsUpdate = isUpdate;
            }
            public CCUSTOMER(Int64 customerSN, string jsonstring)
                : this(jsonstring)
            {
                this.SN = customerSN;
            }

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