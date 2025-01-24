namespace DespachoDelivery.Server.Models
{
    public class ListadoFletyCliente
    {
        public string NumFactura { get; set; }
        public string CodCliente { get; set; }
        public string NomCliente { get; set; }

        public string Status { get; set; }
        public string Sucursal { get; set; }
        public string IdSucursal { get; set; }

        public DateTime? FechaFactura { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string Direccion { get; set; }
    }
}
