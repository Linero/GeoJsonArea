using GeoJSON.Net;
using GeoJSON.Net.Geometry;

namespace GeoJsonArea
{
    public class GeoJsonArea
    {
        private const int EARTH_RADIUS = 6378137;
        private const double FLATTENING_DENOM = 298.257223563;
        private const double FLATTENING = 1 / FLATTENING_DENOM;
        private const double POLAR_RADIUS = EARTH_RADIUS * (1 - FLATTENING);

        public static double Geometry(dynamic geoJson)
        {
            return geoJson.Type switch
            {
                GeoJSONObjectType.Polygon => Geometry((Polygon)geoJson),
                GeoJSONObjectType.MultiPolygon => Geometry((MultiPolygon)geoJson),
                GeoJSONObjectType.GeometryCollection => Geometry((GeometryCollection)geoJson),
                _ => 0,
            };
        }

        public static double Geometry(Polygon geoJson)
        {
            return PolygonArea(geoJson);
        }

        public static double Geometry(MultiPolygon geoJson)
        {
            double area = 0;
            using IEnumerator<Polygon> e = geoJson.Coordinates.GetEnumerator();
            if (e.MoveNext() == false) return 0;
            do area += PolygonArea(e.Current);
            while (e.MoveNext());
            return area;
        }

        public static double Geometry(GeometryCollection geoJson)
        {
            double area = 0;
            using IEnumerator<IGeometryObject> e = geoJson.Geometries.GetEnumerator();
            if (e.MoveNext() == false) return 0;
            do area += Geometry(e.Current);
            while (e.MoveNext());

            return area;
        }

        private static double PolygonArea(Polygon polygon)
        {
            double area = 0;
            using IEnumerator<LineString> e = polygon.Coordinates.GetEnumerator();
            if (e.MoveNext() == false) return 0;
            area += Math.Abs(RingArea(e.Current));
            while (e.MoveNext()) area -= Math.Abs(RingArea(e.Current));
            return area;
        }

        /// <summary>
        /// Calculate the approximate area of the polygon were it projected onto
        /// the earth. Note that this area will be positive if ring is oriented
        /// clockwise, otherwise it will be negative.
        /// 
        /// Reference:
        /// Robert. G. Chamberlain and William H. Duquette, "Some Algorithms for
        /// Polygons on a Sphere", JPL Publication 07-03, Jet Propulsion
        /// Laboratory, Pasadena, CA, June 2007 <see href="https://hdl.handle.net/2014/41271"/>
        /// </summary>
        /// <returns>The approximate signed geodesic area of the polygon in square
        /// meters.</returns>
        ///
        private static double RingArea(LineString lineString)
        {
            double area = 0;
            int lowerIndex, middleIndex, upperIndex;

            var coords = lineString.Coordinates.ToArray();

            var coordsLength = coords.Length;
            if (coordsLength > 2)
            {
                for (var i = 0; i < coordsLength; i++)
                {
                    if (i == coordsLength - 2)
                    {
                        // i = N-2
                        lowerIndex = coordsLength - 2;
                        middleIndex = coordsLength - 1;
                        upperIndex = 0;

                    }
                    else if (i == coordsLength - 1)
                    {
                        // i = N-1
                        lowerIndex = coordsLength - 1;
                        middleIndex = 0;
                        upperIndex = 1;
                    }
                    else
                    {
                        // i = 0 to N-3
                        lowerIndex = i;
                        middleIndex = i + 1;
                        upperIndex = i + 2;
                    }
                    var p1 = coords[lowerIndex];
                    var p2 = coords[middleIndex];
                    var p3 = coords[upperIndex];

                    area += (Radius(p3.Longitude) - Radius(p1.Longitude)) * Math.Sin(Radius(p2.Latitude));
                }
                area = (area * POLAR_RADIUS * POLAR_RADIUS) / 2;
            }
            return area;
        }

        private static double Radius(double value)
        {
            return (value * Math.PI) / 180;
        }

    }
}