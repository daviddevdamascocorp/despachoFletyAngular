using Microsoft.AspNetCore.Mvc.Rendering;

namespace DespachoDelivery.Server.Models
{
    public class Stores
    {
        public List<SelectListItem>? Sucursales { get; set; }
        public string SucursalId { get; set; }
    }
}
