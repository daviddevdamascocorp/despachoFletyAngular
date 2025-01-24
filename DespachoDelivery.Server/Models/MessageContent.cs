using Newtonsoft.Json;

namespace DespachoDelivery.Server.Models
{
    public class MessageContent
    {

        [JsonProperty("mensaje")]
        public string Message { get; set; }
        [JsonProperty("num_dest")]
        public string ClientNumber { get; set; }
        [JsonProperty("prior")]
        public int PriorityNumber { get; set; }
        [JsonProperty("dep")]
        public string Department { get; set; }
        [JsonProperty("id")]
        public string IdCampaing { get; set; }

    }
}
