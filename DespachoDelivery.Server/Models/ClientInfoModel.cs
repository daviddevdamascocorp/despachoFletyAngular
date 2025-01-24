namespace DespachoDelivery.Server.Models
{
    public class ClientInfoModel
    {
        public string InvoiceNumber { get; set; }

        public string Sucursal { get; set; }
        int idSucursal { get; set; }

        public string Cedula { get; set; }


        public DateTime InvoiceDate { get; set; }


        public string StoreCode { get; set; }

        public string NameClient { get; set; }


        public string CorreoCliente { get; set; }
        public string SurnameClient { get; set; }




        public string PhoneNumberClient { get; set; }

        public string? PhoneNumberClient2 { get; set; }


        public List<Article> Articles { get; set; }



        public String AddressClient { get; set; }

        public String Observation { get; set; }
    }
}
