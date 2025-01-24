using DespachoDelivery.Server.Models;
using Microsoft.AspNetCore.Mvc;
using QuestPDF;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DespachoDelivery.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        // GET: api/<ComprobantesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ComprobantesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ComprobantesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ComprobantesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ComprobantesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /*[HttpGet("getpdf/{numfac}/sucursal/{SucursalId}")]
        public IActionResult GenerarPdf(string numfac, string SucursalId)

        {
            int totalDigits = SucursalId.Length;
            var sucur = Convert.ToInt32(SucursalId);

            Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            Byte[] bytes;
            var resultado = GetDeliveryClientesResp(sucur, numfac);
            try
            {
                var document = new GuiaDespacho(resultado);
                var fileNamePdf = "Despacho" + numfac + ".pdf";
                var pdf = document.GeneratePdf();
                return File(pdf, "application/pdf", fileNamePdf);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }




        }*/

        [HttpGet("getpdf1/{numfac}/sucursal/{SucursalId}")]
        public void abc(string numfac, string SucursalId) { Console.WriteLine("Hola"); }
    }
}
