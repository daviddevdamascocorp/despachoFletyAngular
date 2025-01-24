using DespachoDelivery.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace DespachoDelivery.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionesController : ControllerBase
    {
        private SqlConnection _connection;
        public IConfiguration _configuration { get; set; }
        public DireccionesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Connection()
        {
            string connectionCupon = _configuration["ConnectionStrings:SQLConnection"];
            _connection = new SqlConnection(connectionCupon);
        }

        [HttpGet("estado/{cntry}")]
        public IActionResult GetStates(string cntry)
        {
            List<ProvinceModel> prov = States();
            Console.WriteLine(prov);
            return Ok(prov);

        }
        [HttpGet("municipio/{state}")]
        public IActionResult GetMunicipio(string state)
        {
            List<MunicipalityModel> mun = Municipality(state);
            return Ok(mun);
        }

        [HttpGet("ciudad/{state}")]
        public IActionResult GetCities(string state)
        {
            List<CityModel> city = Cities(state);
            return Ok( city );

        }

        [HttpGet("zona/{state}/num/{mun}")]
        public IActionResult GetZone(string state, string mun)
        {

            List<ZoneModel> zoneList = Zones(state, mun);
            return Ok(zoneList );


        }
        public List<ProvinceModel> States()
        {
            Connection();
            List<ProvinceModel> listaEstados = new List<ProvinceModel>();
            SqlCommand command = new SqlCommand("select CD_PROVINCIA ,NOMBRE_DE_PROVINCIA  FROM PROVINCIA", _connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            System.Data.DataTable dataTable = new System.Data.DataTable();

            _connection.Open();
            adapter.Fill(dataTable);
            _connection.Close();
            foreach (DataRow item in dataTable.Rows)
            {
                listaEstados.Add(new ProvinceModel
                {
                    ProvinceId = Convert.ToString(item["CD_PROVINCIA"]).Trim(),
                    ProvinceName = Convert.ToString(item["NOMBRE_DE_PROVINCIA"]).Trim()
                });

            }
            return listaEstados;
        }

        public List<MunicipalityModel> Municipality(string stateId)
        {
            Connection();
            List<MunicipalityModel> listaMunicipios = new List<MunicipalityModel>();
            SqlCommand commandMunicipality = new SqlCommand("select CD_MUNICIPIO ,NOMBRE_DE_MUNICIPIO  " +
                "FROM MUNICIPIO where CD_PROVINCIA =  @ParameterName", _connection);
            SqlDataAdapter adapterMun = new SqlDataAdapter(commandMunicipality);
            commandMunicipality.Parameters.AddWithValue("@ParameterName", stateId);
            System.Data.DataTable dataTableMun = new System.Data.DataTable();
            _connection.Open();
            adapterMun.Fill(dataTableMun);
            _connection.Close();
            foreach (DataRow item in dataTableMun.Rows)
            {
                listaMunicipios.Add(new MunicipalityModel
                {
                    MunucipId = Convert.ToString(item["CD_MUNICIPIO"]).Trim(),
                    MunucipName = Convert.ToString(item["NOMBRE_DE_MUNICIPIO"]).Trim(),
                });
            }
            return listaMunicipios;
        }

        public List<CityModel> Cities(string stateId)
        {
            Connection();
            List<CityModel> citiesList = new List<CityModel>();
            SqlCommand commandCity = new SqlCommand("select CD_CIUDAD ,DE_CIUDAD  FROM CIUDAD where CD_PROVINCIA = @provinceId", _connection);
            SqlDataAdapter adapterCit = new SqlDataAdapter(commandCity);
            commandCity.Parameters.AddWithValue("provinceId", stateId);
            System.Data.DataTable dataTableCity = new System.Data.DataTable();

            _connection.Open();
            adapterCit.Fill(dataTableCity);
            _connection.Close();
            foreach (DataRow item in dataTableCity.Rows)
            {
                citiesList.Add(new CityModel
                {
                    CityId = Convert.ToString(item["CD_CIUDAD"]).Trim(),
                    CityName = Convert.ToString(item["DE_CIUDAD"]).Trim(),
                });
            }
            return citiesList;
        }
        public List<ZoneModel> Zones(string stateId, string MunucipId)
        {
            Connection();
            List<ZoneModel> zoneList = new List<ZoneModel>();
            SqlCommand commandZone = new SqlCommand("select CD_ZONA ,NOMBRE_DE_ZONA  FROM ZONA where CD_PROVINCIA = @provinceId and CD_MUNICIPIO = @muniId", _connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandZone);
            commandZone.Parameters.AddWithValue("@provinceId", stateId);
            commandZone.Parameters.AddWithValue("@muniId", MunucipId);
            System.Data.DataTable zoneData = new System.Data.DataTable();
            _connection.Open();
            sqlDataAdapter.Fill(zoneData);
            _connection.Close();

            foreach (DataRow item in zoneData.Rows)
            {
                zoneList.Add(new ZoneModel { ZoneId = Convert.ToString(item["CD_ZONA"]).Trim(), ZoneName = Convert.ToString(item["NOMBRE_DE_ZONA"]).Trim() });
            }
            return zoneList;
        }
    }
}
