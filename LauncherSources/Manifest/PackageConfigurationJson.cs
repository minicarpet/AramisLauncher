namespace AramisLauncher.Manifest
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Globalization;

    public partial class PackageConfigurationJson
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("files")]
        public FileProperty[] FileProperties { get; set; }
    }

    public partial class FileProperty
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileSize")]
        public long FileSize { get; set; }
    }

    public partial class PackageConfigurationJson
    {
        public static PackageConfigurationJson FromJson(string json) => JsonConvert.DeserializeObject<PackageConfigurationJson>(json, PackageConfigurationJsonConverter.Settings);
    }

    internal static class PackageConfigurationJsonConverter
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
