namespace AramisLauncher.Manifest
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Globalization;
    using System.Collections.Generic;

    public partial class ForgeV31InstallationProfile
    {
        [JsonProperty("_comment_")]
        public string[] Comment { get; set; }

        [JsonProperty("profile")]
        public string Profile { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("json")]
        public string Json { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("minecraft")]
        public string Minecraft { get; set; }

        [JsonProperty("welcome")]
        public string Welcome { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("processors")]
        public Processor[] Processors { get; set; }

        [JsonProperty("libraries")]
        public List<Library> Libraries { get; set; }
    }

    public partial class Library
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("downloads")]
        public Downloads Downloads { get; set; }
    }

    public partial class ForgeV31InstallationProfile
    {
        public static ForgeV31InstallationProfile FromJson(string json) => JsonConvert.DeserializeObject<ForgeV31InstallationProfile>(json, ForgeV31InstallationProfileConverter.Settings);
    }

    public static class ForgeV31InstallationProfileSerialize
    {
        public static string ToJson(this ForgeV31InstallationProfile self) => JsonConvert.SerializeObject(self, ForgeV31InstallationProfileConverter.Settings);
    }

    internal static class ForgeV31InstallationProfileConverter
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
