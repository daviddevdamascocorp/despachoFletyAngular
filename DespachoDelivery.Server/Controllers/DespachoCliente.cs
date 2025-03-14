﻿using DespachoDelivery.Server.Models;
using DespachoDelivery.Server.Sap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SAPbobsCOM;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using QuestPDF;
using QuestPDF.Fluent;
using DespachoDelivery.Server.Models.Comprobante;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DespachoDelivery.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespachoCliente : ControllerBase
    {
        private SqlConnection _connection;
        private SqlConnection _connectionDamscoProd;
        private SqlConnection _connectionKlkPos;
        private SqlConnection _connectionDescuento;
        private bool isSuccess;
        private string message;

        public IConfiguration _configuration { get; set; }

        public DespachoCliente(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Connection()
        {
            string connectionCupon = _configuration["ConnectionStrings:SQLConnection"];
            _connection = new SqlConnection(connectionCupon);
        }

        public void ConnectionDamscoProd()
        {

            string connectionDamascoProd = _configuration["ConnectionStrings:SQLConnection2"];

            _connectionDamscoProd = new SqlConnection(connectionDamascoProd);
        }

        public void ConnectionKlkPos()
        {
            string connectionKlkPos = _configuration["ConnectionStrings:SQLConnection3"];
            _connectionKlkPos = new SqlConnection(connectionKlkPos);

        }

        // GET: api/<DespachoCliente>


        [HttpGet("tiendas")]
        public IEnumerable<SelectListItem> GetTiendas()
        {
            var sucursales = ObtenerSucursales();
            return sucursales;
        }

        public List<SelectListItem> ObtenerSucursales()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                ConnectionDamscoProd();
               
                   
                    string query = "select * from aplicativofront";
                SqlCommand sqlCommand = new SqlCommand(query, _connectionDamscoProd);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                System.Data.DataTable dataTable = new System.Data.DataTable();
                _connectionDamscoProd.Open();
                adapter.Fill(dataTable);
                _connectionDamscoProd.Close();
                foreach (DataRow item in dataTable.Rows)
                {
                    
                    string storeId = Convert.ToString(item["id_sucursal"]).Trim();
                    string Almacen = Convert.ToString(item["u_almacen"]).Trim();
                    string storeName = Convert.ToString(item["nombre"]).Trim();
                    list.Add(new SelectListItem
                    {
                        Value = storeId + Almacen,
                        Text = storeName,
                    });
                }

                    
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener sucursales: " + ex.Message);
            }
            finally
            {
                SapConnection.Disconnect();
            }

            return list;
        }
        // GET api/<DespachoCliente>/5
        [HttpGet("factura/{numfac}/almacen/{sucursal}")]
        public IActionResult DatosDeCliente(long sucursal, string numfac)
        {
            ConnectionKlkPos();
            string destinoStr = sucursal.ToString();
            int totalDigits = destinoStr.Length;

            int primerosDosDigitos = (int)(sucursal / (int)Math.Pow(10, totalDigits - 2));
            var resultado = BuscarFactura(primerosDosDigitos, numfac);
            return Ok(resultado);
        }

        public List<FacturaDataModel> BuscarFactura(int idsucursal, string numfac)
        {


            string Almacen2 = ObtenerAlmacen(idsucursal);

            string Almacen = Regex.Replace(Almacen2, @"\s+", "");
            string query = @"
                        SELECT li.NumFactura, fa.FechaFactura, fa.CodCliente, fa.NomCliente, li.CodArticulo,li.Serial,li.Descripcion,ROUND(li.PrecioUSD, 2) AS PrecioUSD, li.CodigoAlmacen
FROM KLK_FACTURALINE li
INNER JOIN KLK_FACTURAHDR fa ON li.NumFactura = fa.NumFactura and li.Sucursal=fa.Sucursal
                    WHERE fa.NumFactura = @numFactura AND fa.IDSucursal = @Almacen";

            var result = new List<FacturaDataModel>();
            SqlCommand sqlCommand = new SqlCommand(query, _connectionKlkPos);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            sqlCommand.Parameters.AddWithValue("@numFactura", numfac);
            sqlCommand.Parameters.AddWithValue("@Almacen", idsucursal);

            _connectionKlkPos.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var precioDefinDolar = Math.Round(Convert.ToDecimal(reader["PrecioUSD"]), 0);
                var precioSinInva = precioDefinDolar / 1.16m;
                var precioPoliza = Math.Round(precioSinInva * 0.06m, 2);

                result.Add(new FacturaDataModel
                {
                    NumFactura = reader["NumFactura"].ToString(),
                    FechaFactura = Convert.ToDateTime(reader["FechaFactura"]),
                    CodCliente = reader["CodCliente"].ToString(),
                    NomCliente = reader["NomCliente"].ToString(),
                    Serial = reader["Serial"].ToString(),
                    CodArticulo = reader["CodArticulo"].ToString(),
                    Descripcion = reader["Descripcion"].ToString(),
                    PrecioUSD = Math.Round(Convert.ToDecimal(reader["PrecioUSD"]), 0),
                    PrecioPoliza = precioPoliza,
                    CodigoAlmacen = reader["CodigoAlmacen"].ToString(),
                    Articulo = reader["CodArticulo"].ToString()
                });
            }
            _connectionKlkPos.Close();
            return result;
        }

        private string ObtenerAlmacen(long sucursal)
        {
            ConnectionDamscoProd();
            string almacen = null;

            string query = @"
        SELECT u_almacen 
            FROM aplicativofront 
        WHERE id_sucursal = @sucursal";
            SqlCommand cmd = new SqlCommand(query, _connectionDamscoProd);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@sucursal", sucursal);

            _connectionDamscoProd.Open();
            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                almacen = result.ToString();
            }
            _connectionDamscoProd.Close();
            return almacen;
        }

        // POST api/<DespachoCliente>
        /*[HttpPost("subir")]
        public IActionResult Post( ClientInfoModel clientInfo)
        {

            var respMessage = SendMessage(clientInfo.PhoneNumberClient);

            var dataAlmacenada = SaveDataPolicy(clientInfo);
            Console.WriteLine(respMessage);

            return Ok(dataAlmacenada.ToString);
        }*/

        // PUT api/<DespachoCliente>/5
        [HttpGet("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        public async Task SendMessage(string PhoneNumber)
        {
            MessageContent messageContent = new MessageContent();

            //  var CouponDateFormatted = dates.ToString("dd/MM/yyyy");
            var result = "";
            var messageBody = $"¡Gracias por su compra! Nos complace informarle que cuenta con un servicio de entrega asignado con Flety,en un plazo de 48 horas su pedido sera entregado. Para realizar el seguimiento de su envío por favor contactarse al siguiente numero de Flety 04126087124 para compartir sus opiniones, haga clic aquí:   https://forms.office.com/r/iDEhvgHMx1?origin=lprLink  Damasco siempre te da mas." +
                $"";
            var url = "http://200.74.198.50:14010/notifismsdamas";
            messageContent.Message = messageBody;
            messageContent.ClientNumber = PhoneNumber;
            messageContent.PriorityNumber = 0;
            messageContent.Department = "01";
            messageContent.IdCampaing = "PromoDamasco";
            string messageReq = JsonConvert.SerializeObject(messageContent);
            HttpResponseMessage resp = null;
            using (var httpClient = new HttpClient())
            {

                var bodyRequest = new StringContent(messageReq, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("X-Auth-Apikey", "57xg$mG8%H4*");
                resp = await httpClient.PostAsync(url, bodyRequest);
                resp.EnsureSuccessStatusCode();
                Console.WriteLine(resp.Content);

            }


        }

        [HttpGet("consulta/{numfac}/almacen/{sucursal}/despacho/{despacho}")]
        public IActionResult GetDeliveryClientes(string numfac, string sucursal,int despacho)
        {

            var sucurLong = Convert.ToInt64(sucursal);
            int totalDigits = sucursal.Length;

            int primerosDosDigitos = (int)(sucurLong / (int)Math.Pow(10, totalDigits - 2));
            var resp = GetDeliveryClientesResp(primerosDosDigitos, numfac, despacho);
            return Ok(resp);

        }

        [HttpPost("subir")]
        public IActionResult Post(ClientInfoModel clientInfo)
        {
            ConnectionKlkPos();
           // var respMessage = SendMessage(clientInfo.PhoneNumberClient);
            FletyCliente resp = new FletyCliente();
            try
            {

                // Abrir la conexión
                _connectionKlkPos.Open();

                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO  Flety
                ([Numfactura],[Tienda],[Estatus],[Fecha_Actualizacion],[DireccionEnvio],[ObservacionEnvio],[CorreoCliente],[TelefonoCliente],[Proveedor],[Fecha_Creacion]) 
                VALUES (@numfactura, @Tienda,'Pendiente', @fecha,@direccion,@observacion,@correo,@telefono,@proveedor,@fechaCreacion);SELECT SCOPE_IDENTITY();",
                         _connectionKlkPos);
                

                    // Asignar parámetros
                    sqlCommand.Parameters.AddWithValue("@numfactura", clientInfo.InvoiceNumber);
                    sqlCommand.Parameters.AddWithValue("@Tienda", clientInfo.Sucursal);
                    sqlCommand.Parameters.AddWithValue("@fecha", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@direccion", clientInfo.AddressClient);
                    sqlCommand.Parameters.AddWithValue("@observacion", clientInfo.Observation);
                    sqlCommand.Parameters.AddWithValue("@correo", clientInfo.CorreoCliente);
                    sqlCommand.Parameters.AddWithValue("@telefono", clientInfo.PhoneNumberClient);
                sqlCommand.Parameters.AddWithValue("@proveedor", clientInfo.Provider);
                sqlCommand.Parameters.AddWithValue("@fechaCreacion", clientInfo.DateCreation);

                // Ejecutar la consulta


                // isSuccess = true;
                // message = "Ingreso guardado exitosamente.";

                var despacho = sqlCommand.ExecuteScalar();
                string destinoStr = clientInfo.Sucursal.ToString();
                 var sucurLong = Convert.ToInt64(clientInfo.Sucursal);
                 int totalDigits = destinoStr.Length;

                 int primerosDosDigitos = (int)(sucurLong / (int)Math.Pow(10, totalDigits - 2));
                
                 resp = GetDeliveryClientesResp(primerosDosDigitos, clientInfo.InvoiceNumber, Convert.ToInt32(despacho));
                _connectionKlkPos.Close();

                return  Ok(resp);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }


        }

        [HttpGet("sucursal/{sucursal}/numfactura/{numfactura}")]
        public FletyCliente GetDeliveryClientesResp(int sucursal, string numfactura, int id_despacho)
        {
           FletyCliente fletyCliente = new FletyCliente();

            ConnectionKlkPos();
            //cabcera del cliente

            SqlCommand facturaHeaderCommand = new SqlCommand("SELECT fa.NumFactura as NumFactura,fa.CodCliente as CodCliente,fa.NomCliente as NomCliente,fa.Telefono as Telefono," +
                "MAX(fe.Estatus) AS Estatus, fa.Sucursal as Sucursal,fa.IDSucursal as IDSucursal," +
                "fe.DireccionEnvio as DireccionEnvio,fe.Id_despacho as Despacho,fe.ObservacionEnvio as ObservacionEnvio,fe.TelefonoCliente as TelefonoCliente,fa.FechaFactura AS FechaFac,MAX(fe.Fecha_Actualizacion) AS FechaActFlety,fe.DireccionEnvio " +
                "FROM KLK_FACTURAHDR fa JOIN flety fe ON fa.NumFactura = fe.Numfactura AND fa.IDSucursal = @IdSucursal  where fa.NumFactura=@numfactura and fe.Id_despacho = @idDespacho GROUP BY fa.NumFactura, fa.CodCliente,fa.NomCliente,fa.Telefono,fa.Sucursal," +
                "fa.IDSucursal,fe.DireccionEnvio,fe.ObservacionEnvio,fe.TelefonoCliente,fa.FechaFactura,fe.Id_despacho ", _connectionKlkPos);

            SqlDataAdapter adapterHeader = new SqlDataAdapter(facturaHeaderCommand);

            facturaHeaderCommand.Parameters.AddWithValue("@numfactura", numfactura);
            facturaHeaderCommand.Parameters.AddWithValue("@IdSucursal", sucursal);
            facturaHeaderCommand.Parameters.AddWithValue("@idDespacho", id_despacho);
            _connectionKlkPos.Open();
            using (var reader = facturaHeaderCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    try
                    {
                        fletyCliente.IdDespacho = Convert.ToInt32(reader["Despacho"]);
                        fletyCliente.NumFactura = Convert.ToString(reader["NumFactura"]).Trim();
                        fletyCliente.CodCliente = Convert.ToString(reader["CodCliente"]).Trim();
                        fletyCliente.NomCliente = Convert.ToString(reader["NomCliente"]).Trim();
                        fletyCliente.Status = Convert.ToString(reader["Estatus"]).Trim();
                        fletyCliente.Sucursal = Convert.ToString(reader["Sucursal"]).Trim();
                        fletyCliente.NumeroTelefono = Convert.ToString(reader["Telefono"]).Trim();
                        fletyCliente.FechaFactura = Convert.ToDateTime(reader["FechaFac"]);
                        fletyCliente.FechaActualizacion = Convert.ToDateTime(reader["FechaActFlety"]);
                        fletyCliente.Direccion = Convert.ToString(reader["DireccionEnvio"]).Trim();
                        fletyCliente.Observaciones = Convert.ToString(reader["ObservacionEnvio"]).Trim();
                        fletyCliente.IdSucursal = Convert.ToString(reader["IDSucursal"]).Trim();

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            _connectionKlkPos.Close();
            //cabaecera del product
            List<Articulos> listaArticulos = new List<Articulos>();
            SqlCommand commandLineaFactura = new SqlCommand("select fa.NumFactura as  NumFactura,li.CodArticulo as CodArticulo,li.Descripcion as Descripcion,li.Cantidad as Cantidad from KLK_FACTURALINE li" +
                " inner join flety fe on li.NumFactura=fe.Numfactura and LEFT(fe.Tienda, 2)=li.IDSucursal" +
                " inner join KLK_FACTURAHDR fa on fa.NumFactura=li.NumFactura and fa.IDSucursal=li.IDSucursal " +
                " where fa.NumFactura=@numFactura and li.IDSucursal=@idSucursal and li.CodArticulo not like '%S0000007' ", _connectionKlkPos);

            commandLineaFactura.Parameters.AddWithValue("@numfactura", numfactura);
            commandLineaFactura.Parameters.AddWithValue("@IdSucursal", sucursal);
            SqlDataAdapter adapter = new SqlDataAdapter(commandLineaFactura);

            System.Data.DataTable dataTable = new System.Data.DataTable();
            _connectionKlkPos.Open();
            adapter.Fill(dataTable);
            _connectionKlkPos.Close();
            foreach (DataRow item in dataTable.Rows)
            {
                listaArticulos.Add(new Articulos
                {
                    CantidadArticulo = Convert.ToInt32(item["Cantidad"]),
                    CodArticulo = Convert.ToString(item["CodArticulo"]).Trim(),
                    Descripcion = Convert.ToString(item["Descripcion"]).Trim()



                });

            }

            fletyCliente.Articulos = listaArticulos;
            return fletyCliente;
        }

        [HttpPost("getpdf")]
        public IActionResult GenerarPdf(Comprobante comprobante)

        {
           
            var sucur = Convert.ToInt32(comprobante.IdSucursal);
         
            Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            Byte[] bytes;
            var resultado = GetDeliveryClientesResp(sucur,comprobante.NumFactura,comprobante.IdDespacho);
            try {
                var document = new GuiaDespacho(resultado);
                var fileNamePdf = "Despacho" + comprobante.NumFactura + ".pdf";
                var pdf = document.GeneratePdf();
                     return File(pdf, "application/pdf", fileNamePdf);
            } catch (Exception ex) { return BadRequest(ex.Message); }
           
           
           

        }

        
    }
}
