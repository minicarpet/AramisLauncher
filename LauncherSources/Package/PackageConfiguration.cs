using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace AramisLauncher.Package
{
    public partial class PackageConfiguration
    {
        [JsonProperty("packages")]
        public Package[] Packages { get; set; }
    }

    public partial class Package
    {
        [JsonProperty("packageName")]
        public string PackageName { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("address")]
        public string ServerAddress { get; set; }
    }

    public partial class PackageConfiguration
    {
        public static PackageConfiguration FromJson(string json) => JsonConvert.DeserializeObject<PackageConfiguration>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PackageConfiguration self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
