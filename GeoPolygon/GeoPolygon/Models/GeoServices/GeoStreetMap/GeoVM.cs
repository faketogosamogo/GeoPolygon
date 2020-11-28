using Newtonsoft.Json;

namespace GeoPolygon.Models.GeoServices.GeoStreetMap
{
    public class GeoVM
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("coordinates")]
        public dynamic Coordinates { get; set; }
    }
}
