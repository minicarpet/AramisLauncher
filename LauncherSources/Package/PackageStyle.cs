using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace AramisLauncher.Package
{
    public partial class PackageStyle
    {
        [JsonProperty("progressBarColor", NullValueHandling = NullValueHandling.Ignore)]
        public byte[] ProgressBarColor { get; set; }

        [JsonProperty("downloadButtonColor", NullValueHandling = NullValueHandling.Ignore)]
        public byte[] DownloadButtonColor { get; set; }

        [JsonProperty("downloadButtonText", NullValueHandling = NullValueHandling.Ignore)]
        public string DownloadButtonText { get; set; }
    }

    public partial class PackageStyle
    {
        public static PackageStyle FromJson(string json) => JsonConvert.DeserializeObject<PackageStyle>(json, Converter.Settings);
    }

    public static class SerializePackageStyle
    {
        public static string ToJson(this PackageStyle self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
