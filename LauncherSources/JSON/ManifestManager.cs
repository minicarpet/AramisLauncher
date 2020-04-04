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
        public static AramisPackageJson aramisPackageJson = new AramisPackageJson();
        public static AramisConfigurationJson aramisConfigurationJson = new AramisConfigurationJson();

        public static List<Version> minecraftVersions = new List<Version>();
        public static Uri minecrafetVersionAssets;
        public static List<AssetInformation> assetsInformation = new List<AssetInformation>();
        public static MinecraftVersionJson minecraftVersionJson = new MinecraftVersionJson();
        public static ForgeVersionJson forgeVersionJson = new ForgeVersionJson();
        public static string manifestVersionData;
        
        private static WebClient webClient = new WebClient();
        private static  string mojangMinecraftManifestUrl = "https://launchermeta.mojang.com/mc/game/version_manifest.json";

        public ManifestManager()
        {
            MainWindow.downloadDescriptor.Content = "Récupération des manifests...";
            AramisManifest();

            string manifestData = webClient.DownloadString(mojangMinecraftManifestUrl);
            MojangMinecraftManifest mojangMinecraftManifest = JsonConvert.DeserializeObject<MojangMinecraftManifest>(manifestData);

            /* Record each available version */
            foreach (Version version in mojangMinecraftManifest.Versions)
            {
                if(version.Type == TypeEnum.Release)
                {
                    minecraftVersions.Add(version);
                }
            }

            MainWindow.versionModpack.Content += " " + aramisPackageJson.Manifest.Version;

            MainWindow.downloadDescriptor.Content = "Manifests récupérés";
        }

        public static void GetManifestVersion()
        {
            Version versionFound = minecraftVersions.Find(version => version.Id == aramisPackageJson.BaseModLoader.MinecraftVersion);

            /* Get manifest for specified version */
            manifestVersionData = webClient.DownloadString(versionFound.Url);
            minecraftVersionJson = MinecraftVersionJson.FromJson(manifestVersionData);

            /* get assets json */
            minecrafetVersionAssets = minecraftVersionJson.AssetIndex.Url;

            GetAssetsManifest();
        }

        public static void GetAssetsManifest()
        {
            /* Download assets manifest */
            string assetsManifest = webClient.DownloadString(minecrafetVersionAssets);
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

        public static void AramisManifest()
        {
            /* To get the recommended version */
            //https://files.minecraftforge.net/maven/net/minecraftforge/forge/promotions_slim.json

            aramisPackageJson = AramisPackageJson.FromJson(webClient.DownloadString("https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/downloadInfos/aramisPackage.json"));
            aramisConfigurationJson = AramisConfigurationJson.FromJson(webClient.DownloadString("https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/downloadInfos/config.json"));
            forgeVersionJson = ForgeVersionJson.FromJson(aramisPackageJson.BaseModLoader.VersionJson);
        }
    }
}
