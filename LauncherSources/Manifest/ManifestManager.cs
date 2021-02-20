namespace AramisLauncher.Manifest
{
    using System.Net;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using AramisLauncher.Minecraft;
    using System;
    using AramisLauncher.Common;

    class ManifestManager
    {
        public static PackageJson packageJson = new PackageJson();
        public static PackageConfigurationJson packageConfigurationJson = new PackageConfigurationJson();

        public static List<Version> minecraftVersions = new List<Version>();
        public static List<AssetInformation> assetsInformation = new List<AssetInformation>();
        public static MinecraftVersionJson minecraftVersionJson = new MinecraftVersionJson();
        public static ForgeVersionJson forgeVersionJson = new ForgeVersionJson();
        public static NewForgeVersionJson newForgeVersionJson = new NewForgeVersionJson();
        public static ForgeV31InstallationProfile forgeV31InstallationProfile = new ForgeV31InstallationProfile();
        public static ForgeInstallationProfile forgeInstallationProfile = new ForgeInstallationProfile();
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
            int result = packageJson.BaseModLoader.MinecraftVersion.CompareTo("1.13.0");
            if (result >= 0)
            {
                if(packageJson.BaseModLoader.ForgeVersion.CompareTo("31.2.45") >= 0)
                {
                    forgeVersionJson = null;
                    newForgeVersionJson = null;
                    forgeV31InstallationProfile = ForgeV31InstallationProfile.FromJson(packageJson.BaseModLoader.InstallProfileJson);
                }
                else
                {
                    newForgeVersionJson = NewForgeVersionJson.FromJson(packageJson.BaseModLoader.VersionJson);
                    forgeInstallationProfile = ForgeInstallationProfile.FromJson(packageJson.BaseModLoader.InstallProfileJson);
                    forgeVersionJson = null;
                }
            }
            else
            {
                forgeVersionJson = ForgeVersionJson.FromJson(packageJson.BaseModLoader.VersionJson);
                newForgeVersionJson = null;
                forgeInstallationProfile = null;
            }
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
