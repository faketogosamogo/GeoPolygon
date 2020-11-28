using Newtonsoft.Json;

namespace GeoPolygon.Models.GeoServices.GeoStreetMap
{
    public class PolygonVM
    {
        [JsonProperty("geojson")]
        public GeoVM Geo { get; set; }
    }
}
