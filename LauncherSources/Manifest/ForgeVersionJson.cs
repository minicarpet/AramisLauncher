﻿namespace AramisLauncher.Manifest
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ForgeVersionJson
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("releaseTime")]
        public string ReleaseTime { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("minecraftArguments")]
        public string MinecraftArguments { get; set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }

        [JsonProperty("inheritsFrom")]
        public string InheritsFrom { get; set; }

        [JsonProperty("jar")]
        public string Jar { get; set; }

        [JsonProperty("logging")]
        public ForgeLogging Logging { get; set; }

        [JsonProperty("libraries")]
        public ForgeLibrary[] Libraries { get; set; }
    }

    public partial class ForgeLibrary
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("serverreq", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Serverreq { get; set; }

        [JsonProperty("checksums", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Checksums { get; set; }

        [JsonProperty("clientreq", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Clientreq { get; set; }
    }

    public partial class ForgeLogging
    {
    }

    public partial class ForgeVersionJson
    {
        public static ForgeVersionJson FromJson(string json) => JsonConvert.DeserializeObject<ForgeVersionJson>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ForgeVersionJson self) => JsonConvert.SerializeObject(self, Converter.Settings);
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

