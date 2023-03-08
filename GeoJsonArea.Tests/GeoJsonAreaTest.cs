using FluentAssertions;
using GeoJSON.Net.Geometry;

namespace GeoJsonArea.Tests
{
    public class GeoJsonAreaTest
    {
        [Theory]
        [GeometryCollection("./Data/poland.json", 322_575_000_000, 0.05f)]
        [GeometryCollection("./Data/warsaw.json", 517_200_000, 0.02f)]
        [GeometryCollection("./Data/berlin.json", 891_800_000, 0.02f)]
        public void GeoJsonAreaGeometry_ForValidArguments_ReturnsCorrectArea(GeometryCollection geoJson, double expectedArea, float precision)
        {
            // act
            double area = geoJson.Area();
            // assert
            area.Should().BeInRange(expectedArea * (1 - precision), expectedArea * (1 + precision));
        }
    }
}