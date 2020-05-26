using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.Manifest
{
    public partial class NewForgeVersionJson
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("releaseTime")]
        public DateTimeOffset ReleaseTime { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("minimumLauncherVersion")]
        public long MinimumLauncherVersion { get; set; }

        [JsonProperty("inheritsFrom")]
        public string InheritsFrom { get; set; }

        [JsonProperty("jar")]
        public string Jar { get; set; }

        [JsonProperty("libraries")]
        public NewForgeLibrary[] Libraries { get; set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }

        [JsonProperty("arguments")]
        public Arguments Arguments { get; set; }
    }

    public partial class Arguments
    {
        [JsonProperty("game")]
        public string[] Game { get; set; }
    }

    public partial class NewForgeLibrary
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("downloads")]
        public Downloads Downloads { get; set; }
    }

    public partial class Downloads
    {
        [JsonProperty("artifact")]
        public Artifact Artifact { get; set; }
    }

    public partial class Artifact
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }

    public partial class NewForgeVersionJson
    {
        public static NewForgeVersionJson FromJson(string json) => JsonConvert.DeserializeObject<NewForgeVersionJson>(json, AramisLauncher.JSON.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NewForgeVersionJson self) => JsonConvert.SerializeObject(self, AramisLauncher.JSON.Converter.Settings);
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
