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
    public partial class ForgeInstallationProfile
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
        public NewForgeLibrary[] Libraries { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("MAPPINGS")]
        public Binpatch Mappings { get; set; }

        [JsonProperty("BINPATCH")]
        public Binpatch Binpatch { get; set; }

        [JsonProperty("MC_SLIM")]
        public Binpatch McSlim { get; set; }

        [JsonProperty("MC_SLIM_SHA")]
        public Binpatch McSlimSha { get; set; }

        [JsonProperty("MC_EXTRA")]
        public Binpatch McExtra { get; set; }

        [JsonProperty("MC_EXTRA_SHA")]
        public Binpatch McExtraSha { get; set; }

        [JsonProperty("MC_SRG")]
        public Binpatch McSrg { get; set; }

        [JsonProperty("PATCHED")]
        public Binpatch Patched { get; set; }

        [JsonProperty("PATCHED_SHA")]
        public Binpatch PatchedSha { get; set; }

        [JsonProperty("MCP_VERSION")]
        public Binpatch McpVersion { get; set; }
    }

    public partial class Binpatch
    {
        [JsonProperty("client")]
        public string Client { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }
    }

    public partial class Processor
    {
        [JsonProperty("jar")]
        public string Jar { get; set; }

        [JsonProperty("classpath")]
        public string[] Classpath { get; set; }

        [JsonProperty("args")]
        public string[] Args { get; set; }
    }

    public partial class ForgeInstallationProfile
    {
        public static ForgeInstallationProfile FromJson(string json) => JsonConvert.DeserializeObject<ForgeInstallationProfile>(json, AramisLauncher.JSON.Converter.Settings);
    }

    public static class SerializeInstallationProfile
    {
        public static string ToJson(this ForgeInstallationProfile self) => JsonConvert.SerializeObject(self, AramisLauncher.JSON.Converter.Settings);
    }
}
