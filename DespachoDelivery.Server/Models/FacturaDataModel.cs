﻿namespace DespachoDelivery.Server.Models
{
    public class FacturaDataModel
    {
        public string NumFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public string CodCliente { get; set; }
        public string NomCliente { get; set; }
        public string Serial { get; set; }
        public string CodArticulo { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUSD { get; set; }
        public decimal PrecioPoliza { get; set; }
        public string CodigoAlmacen { get; set; }

        public string Articulo { get; set; }
    }
}
