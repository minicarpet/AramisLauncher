using System;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AramisLauncher.JSON
{

    public partial class AramisPackageJson
    {
        [JsonProperty("arguments")]
        public Arguments Arguments { get; set; }

        [JsonProperty("assetIndex")]
        public AssetIndex AssetIndex { get; set; }

        [JsonProperty("assets")]
        public string Assets { get; set; }

        [JsonProperty("downloads")]
        public AramisPackageJsonDownloads Downloads { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("libraries")]
        public Library[] Libraries { get; set; }

        [JsonProperty("logging")]
        public Logging Logging { get; set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }

        [JsonProperty("minimumLauncherVersion")]
        public long MinimumLauncherVersion { get; set; }

        [JsonProperty("releaseTime")]
        public DateTimeOffset ReleaseTime { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class AramisPackageJsonDownloads
    {
        [JsonProperty("client")]
        public ServerClass Client { get; set; }

        [JsonProperty("client_mappings")]
        public Mappings ClientMappings { get; set; }

        [JsonProperty("server")]
        public ServerClass Server { get; set; }

        [JsonProperty("server_mappings")]
        public Mappings ServerMappings { get; set; }
    }

    public partial class AramisPackageJson
    {
        public static AramisPackageJson FromJson(string json) => JsonConvert.DeserializeObject<AramisPackageJson>(json, AramisLauncher.JSON.Converter.Settings);
    }
}
