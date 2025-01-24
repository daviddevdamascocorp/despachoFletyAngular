namespace DespachoDelivery.Server.Models
{
    public class FletyCliente
    {
        public string NumFactura { get; set; }
        public string CodCliente { get; set; }
        public string NomCliente { get; set; }

        public List<Articulos> Articulos { get; set; }
        public string Status { get; set; }
        public string Sucursal { get; set; }

        public string IdSucursal { get; set; }
        public string NumeroTelefono { get; set; }
        public DateTime? FechaFactura { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string Direccion { get; set; }

        public string Observaciones { get; set; }
    }

    public class Articulos
    {
        public string? CodArticulo { get; set; }
        public string? Descripcion { get; set; }

        public int CantidadArticulo { get; set; }
    }
}
