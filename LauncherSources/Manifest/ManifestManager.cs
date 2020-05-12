using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AramisLauncher.Minecraft;
using System;
using System.IO;
using System.Text;
using AramisLauncher.Common;

namespace AramisLauncher.JSON
{
    class ManifestManager
    {
        public static PackageJson packageJson = new PackageJson();
        public static PackageConfigurationJson packageConfigurationJson = new PackageConfigurationJson();

        public static List<Version> minecraftVersions = new List<Version>();
        public static List<AssetInformation> assetsInformation = new List<AssetInformation>();
        public static MinecraftVersionJson minecraftVersionJson = new MinecraftVersionJson();
        public static ForgeVersionJson forgeVersionJson = new ForgeVersionJson();
        public static string manifestVersionData;
        public static string minecrafetVersionAssetsData;


        private static readonly WebClient webClient = new WebClient();

        public ManifestManager()
        {
            
        }

        public static void GetAllManifests()
        {
            GetPackageManifest();
            GetMinecraftManifest();
            GetManifestVersion();
        }

        private static void GetMinecraftManifest()
        {
            string manifestData = webClient.DownloadString(CommonData.mojangMinecraftManifestUrl);
            MojangMinecraftManifest mojangMinecraftManifest = JsonConvert.DeserializeObject<MojangMinecraftManifest>(manifestData);

            /* Record each available version */
            foreach (Version version in mojangMinecraftManifest.Versions)
            {
                if (version.Type == TypeEnum.Release)
                {
                    minecraftVersions.Add(version);
                }
            }

            /* Download forge manifest */
            forgeVersionJson = ForgeVersionJson.FromJson(packageJson.BaseModLoader.VersionJson);
        }

        public static void GetManifestVersion()
        {
            /* To get the recommended version */
            //https://files.minecraftforge.net/maven/net/minecraftforge/forge/promotions_slim.json
            Version versionFound = minecraftVersions.Find(version => version.Id == packageJson.BaseModLoader.MinecraftVersion);

            /* Get manifest for specified version */
            manifestVersionData = webClient.DownloadString(versionFound.Url);
            minecraftVersionJson = MinecraftVersionJson.FromJson(manifestVersionData);

            GetAssetsManifest();
        }

        public static void GetAssetsManifest()
        {
            /* Download assets manifest */
            string assetsManifest = webClient.DownloadString(minecraftVersionJson.AssetIndex.Url);
            minecrafetVersionAssetsData = assetsManifest;
            JObject jObject = (JObject)JsonConvert.DeserializeObject(assetsManifest);
            JEnumerable<JToken> jTokens = jObject.Children();
            foreach (JToken jToken in jTokens)
            {
                JProperty jProperty = (JProperty)jToken;
                if (jProperty.Name == "objects")
                {
                    /* Get Objects JSON Object */
                    JToken jToken1 = jProperty.Value;
                    foreach (JToken jToken2 in jToken1.Children())
                    {
                        /* Get "objects" children to receive all assets data */
                        JProperty jProperty1 = (JProperty)jToken2;
                        AssetInformation assetInformation = jProperty1.Value.ToObject<AssetInformation>();
                        assetInformation.Title = jProperty1.Name;
                        assetsInformation.Add(assetInformation);
                    }
                }
            }
        }

        public static void GetPackageManifest()
        {
            packageJson = PackageJson.FromJson(webClient.DownloadString(CommonData.packageInfoBaseURL + CommonData.packageName + "/minecraftInstance.json"));
            try
            {
                packageConfigurationJson = PackageConfigurationJson.FromJson(webClient.DownloadString(CommonData.packageInfoBaseURL + CommonData.packageName + "/config.json"));
            }
            catch(Exception)
            {
                /* No configuration */
                packageConfigurationJson = null;
            }
        }
    }
}
