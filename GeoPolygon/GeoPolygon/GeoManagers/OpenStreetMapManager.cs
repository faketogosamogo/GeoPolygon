using GeoPolygon.GeoManagers.Exceptions;
using GeoPolygon.Models.GeoServices;
using GeoPolygon.Models.GeoServices.GeoStreetMap;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GeoPolygon.GeoManagers
{
    
    public interface IOpenStreetMapManager : IGeoServiceManager { }
    /// <summary>
    /// Реализация IGeoServiceManager для OpenStreetMap
    /// </summary>
    public class OpenStreetMapManager : IOpenStreetMapManager
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfigurationSection _configurationSection;
        private ILogger<OpenStreetMapManager> _logger;
        public OpenStreetMapManager(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<OpenStreetMapManager> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configurationSection = configuration.GetSection("OpenStreetMap");
            _logger = logger;

        }

        private async Task<string> getPolygonsAsync(string address)
        {
            var searchUrl = _configurationSection.GetValue<string>("SearchURL");
            var userAgent = _configurationSection.GetValue<string>("UserAgent");
            var queryString = new Dictionary<string, string>()
            {
                { "q", address },
                { "format", "json" },
                { "polygon_geojson", "1" },
                { "limit", "1" },
            };

            using var client = _httpClientFactory.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString(searchUrl, queryString));
            request.Headers.UserAgent.ParseAdd(userAgent);

            using var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Polygon> GetPolygonAsync(string address)
        {
            _logger.LogInformation($"start getpolygon: address: {address}");
            
            var polygon = new Polygon();
            var pol = JArray.Parse(await getPolygonsAsync(address));

            if (pol.Count == 0) throw new PolygonNotFoundException($"Полигон по адресу: {address} не найден");

            var coordinates = pol[0]["geojson"]["coordinates"];

            if (pol[0]["geojson"]["type"].ToString() == "Polygon")
            {
                polygon.Coordinates = coordinates.ToObject<decimal[][][]>();
            }
            else if(pol[0]["geojson"]["type"].ToString() == "MultiPolygon")
            {
                var multyCoordinates = coordinates.ToObject<decimal[][][][]>();
                polygon.Coordinates = multyCoordinates[0];//Не понял точно как из мультиполигона переводить в полигон, поэтому беру первый элемент координат
            }

            return polygon;
        }
    }
}