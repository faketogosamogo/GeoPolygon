using GeoPolygon.Models.GeoServices;
using GeoPolygon.Models.GeoServices.GeoStreetMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPolygon.GeoManagers
{
    /// <summary>
    /// Интерфейс для работы с геосерсивами 
    /// </summary>
    public interface IGeoServiceManager
    {
        /// <summary>
        /// Получение полигона по адресу
        /// </summary>
        /// <param name="address">Адрес для поиска</param>
        /// <returns>Найденный полигон</returns>
        Task<Polygon> GetPolygonAsync(string address);
    }
}
