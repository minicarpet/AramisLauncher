using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.JSON
{
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
        public static PackageConfigurationJson FromJson(string json) => JsonConvert.DeserializeObject<PackageConfigurationJson>(json, AramisLauncher.JSON.PackageConfigurationJsonConverter.Settings);
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
