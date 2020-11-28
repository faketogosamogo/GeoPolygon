using GeoPolygon.Models.GeoServices;
using System.Threading.Tasks;

namespace GeoPolygon.IPolygonServices
{
    /// <summary>
    /// Интерфейс сохранения полигона в файл
    /// </summary>
    public interface IPolygonFileSaver
    {
        /// <summary>
        /// Сохраняет полигон в файл, если файл с именем имеется, он будет перезаписан
        /// </summary>
        /// <param name="polygon">Полигон для сохранения</param>
        /// <param name="path">Путь сохранения</param>
        /// <returns></returns>
        Task SaveToFileAsync(Polygon polygon, string path);
    }
}
