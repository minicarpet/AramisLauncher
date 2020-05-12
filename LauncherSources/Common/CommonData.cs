using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AramisLauncher.MojangAuth.AuthFormat;
using static AramisLauncher.MojangAuth.PlayerProfile;

namespace AramisLauncher.Common
{
    public class InsalledPackage
    {
        public string packageName;
        public string packageVersion;
    }

    class CommonData
    {
        /* Custom file to record user profile */
        public static LauncherProfileJson launcherProfileJson = new LauncherProfileJson();

        /* Define folders and files */
        public static string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace('\\', '/');
        public static string aramisFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace('\\', '/') + "/.aramis/";
        public static string launcherProfileFilePath = aramisFolder + "launcher_profile.json";

        /* Package folder */
        public static string packageName;
        public static string packageVersion;
        public static string packageFolder;
        public static string nativeFolder;
        public static string libraryFolder;
        public static string modsFolder;
        public static string scriptFolder;
        public static string assetsFolder;
        public static string assetsLogConfigFolder;
        public static string assetsIndexFolder;
        public static string assetsObjectFolder;
        public static string versionFolder;

        /* Custom link */
        public static string packageInfoJsonURL = "https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/downloadInfos/packageInfos.json";
        public static string packageInfoBaseURL = "https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/downloadInfos/";

        /* Mojang api links */
        public static string authServerValidateURL = "https://authserver.mojang.com/validate";
        public static string authServerRefreshURL = "https://authserver.mojang.com/refresh";
        public static string authServerAuthenticateURL = "https://authserver.mojang.com/authenticate";

        public static string playerSessionURL = "https://sessionserver.mojang.com/session/minecraft/profile/";

        /* Manifest link */
        public static string mojangMinecraftManifestUrl = "https://launchermeta.mojang.com/mc/game/version_manifest.json";

        public class LauncherProfileJson
        {
            public AuthResponse authenticationDatabase;
            public Property userProperty;
            public List<InsalledPackage> installedPackageVersion;
        }

        public static void updatePackageName()
        {
            packageFolder = appDataFolder + "/." + packageName + "/";
            nativeFolder = packageFolder + "bin/natives/";
            libraryFolder = packageFolder + "libraries/";
            modsFolder = packageFolder + "mods/";
            scriptFolder = packageFolder + "scripts/";
            assetsFolder = packageFolder + "assets/";
            assetsLogConfigFolder = packageFolder + "assets/log_configs/";
            assetsIndexFolder = packageFolder + "assets/indexes/";
            assetsObjectFolder = packageFolder + "assets/objects/";
            versionFolder = packageFolder + "versions/";
    }

        public static void saveLauncherProfile()
        {
            if (!Directory.Exists(aramisFolder))
            {
                Directory.CreateDirectory(aramisFolder);
            }

            if (File.Exists(launcherProfileFilePath))
            {
                File.Delete(launcherProfileFilePath);
            }

            File.WriteAllText(launcherProfileFilePath, JsonConvert.SerializeObject(launcherProfileJson, Formatting.Indented));
        }

        public static void getLauncherProfile()
        {
            if (File.Exists(launcherProfileFilePath))
            {
                string content = File.ReadAllText(launcherProfileFilePath);
                launcherProfileJson = JsonConvert.DeserializeObject<LauncherProfileJson>(content);
            }
        }
    }
}
