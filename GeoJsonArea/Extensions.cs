using GeoJSON.Net.Geometry;

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
