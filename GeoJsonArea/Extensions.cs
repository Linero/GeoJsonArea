using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoJsonArea
{
    public static class Extensions
    {
        public static double Area(this IGeometryObject target)
        {
            return GeoJsonArea.Geometry(target);
        }
    }
}
