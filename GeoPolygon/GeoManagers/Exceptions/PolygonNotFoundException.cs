using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPolygon.GeoManagers.Exceptions
{
    public class PolygonNotFoundException : Exception
    {
        public PolygonNotFoundException(string mes) : base(mes) { }
    }
}
