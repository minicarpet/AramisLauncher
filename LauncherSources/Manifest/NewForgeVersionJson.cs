namespace AramisLauncher.Manifest
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Globalization;

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
        public NewForgeVersionJsonArguments Arguments { get; set; }
    }

    public partial class NewForgeVersionJsonArguments
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

    public partial class NewForgeVersionJson
    {
        public static NewForgeVersionJson FromJson(string json) => JsonConvert.DeserializeObject<NewForgeVersionJson>(json, NewForgeVersionJsonConverter.Settings);
    }

    public static class NewForgeVersionJsonSerialize
    {
        public static string ToJson(this NewForgeVersionJson self) => JsonConvert.SerializeObject(self, NewForgeVersionJsonConverter.Settings);
    }

    internal static class NewForgeVersionJsonConverter
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
