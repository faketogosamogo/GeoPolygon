using GeoPolygon.Models.GeoServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace GeoPolygon.PolygonServices
{
    public interface IPolygonFileJsonSaver : IPolygonFileSaver { }

    public class PolygonFileJsonSaver : IPolygonFileJsonSaver
    {
        private ILogger<PolygonFileJsonSaver> _logger;
        public PolygonFileJsonSaver(ILogger<PolygonFileJsonSaver> logger)
        {
            _logger = logger;
        }
        public async Task SaveToFileAsync(Polygon polygon, string path)
        {
            _logger.LogInformation($"start save polygon to path: {path}");
            var poligonJson = JsonConvert.SerializeObject(polygon);
            await File.WriteAllTextAsync(path, poligonJson);
            _logger.LogInformation($"polygon saved to path: {path}");
        }
    }
}
