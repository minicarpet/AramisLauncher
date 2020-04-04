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
    public partial class AramisConfigurationJson
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

    public partial class AramisConfigurationJson
    {
        public static AramisConfigurationJson FromJson(string json) => JsonConvert.DeserializeObject<AramisConfigurationJson>(json, AramisLauncher.JSON.AramisConfigurationJsonConverter.Settings);
    }

    internal static class AramisConfigurationJsonConverter
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
