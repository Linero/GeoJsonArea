using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System.Reflection;
using Xunit.Sdk;

namespace GeoJsonArea.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class GeometryCollectionAttribute : DataAttribute
    {
        private readonly string _jsonPath;
        private readonly double _expectedArea;
        private readonly float _tolerance;

        public GeometryCollectionAttribute(string jsonPath, double expectedArea, float tolerance)
        {
            _jsonPath = jsonPath;
            _expectedArea = expectedArea;
            _tolerance = tolerance;
        }
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) throw new ArgumentNullException(nameof(testMethod));

            var dir = Directory.GetCurrentDirectory();
            var path = Path.GetRelativePath(dir, _jsonPath);
            if (!File.Exists(path)) throw new Exception($"File not exists {path}");
            var data = File.ReadAllText(path);

            var obj = JsonConvert.DeserializeObject<GeometryCollection>(data);
            var attributes = new object[] { obj!, _expectedArea, _tolerance };
            return new List<object[]> { attributes };
        }
    }
}
