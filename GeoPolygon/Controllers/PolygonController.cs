using GeoPolygon.GeoManagers;
using GeoPolygon.GeoManagers.Exceptions;
using GeoPolygon.PolygonServices;
using GeoPolygon.Models.GeoServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPolygon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PolygonController : ControllerBase
    {
        private IGeoServiceManager _openStreetMapManager;
        private IPolygonFileSaver _polygonFileSaver;
        private IWebHostEnvironment _environment;
        private ILogger<PolygonController> _logger;
        public PolygonController(IOpenStreetMapManager openStreetMapManager, IPolygonFileJsonSaver polygonFileJsonSaver, IWebHostEnvironment environment, ILogger<PolygonController> logger)
        {
            _openStreetMapManager = openStreetMapManager;
            _polygonFileSaver = polygonFileJsonSaver;
            _environment = environment;
            _logger = logger;
        }
        [HttpGet]     
        public async Task<IActionResult> Get(string address, int? accuracy, string fileName)
        {
            if(string.IsNullOrEmpty(address) || string.IsNullOrEmpty(fileName))
            {
                return UnprocessableEntity("Параметры address и fileName обязательны");
            }
            if(accuracy!=null && accuracy< 2)
            {
                return UnprocessableEntity("Параметр accuracy не может быть меньше 2");
            }
            var polygon = new Polygon();
            try
            {
                polygon = await _openStreetMapManager.GetPolygonAsync(address);
            }
            catch (PolygonNotFoundException ex)
            {
                _logger.LogInformation($"Not found polygon by address: {address}");
                return NotFound($"Не найден полигон по адресу: {address}");
            }catch(Exception ex)
            {
                _logger.LogError($"Exception while getting polygon by address: {address}, ex: {ex.Message} trace: {ex.StackTrace}");
                return StatusCode(500);
            }
            if(polygon==null) return NotFound($"Не найден полигон по адресу: {address}");

            if (accuracy != null)
            {
                for(int i = 0; i < polygon.Coordinates.Length; i++) polygon.Coordinates[i] = polygon.Coordinates[i].Where((_, index) => index % accuracy == 0).ToArray();
            }

            try
            {
                await _polygonFileSaver.SaveToFileAsync(polygon, _environment.WebRootPath +"/" +fileName);
            }catch(Exception ex)
            {
                _logger.LogError($"Error of write polygon to file, address: {address}, fileName: {fileName}, ex: {ex.Message}, trace: {ex.StackTrace}");
                return Ok(polygon);
            }
            return StatusCode(201, polygon);
        }
    }
}
