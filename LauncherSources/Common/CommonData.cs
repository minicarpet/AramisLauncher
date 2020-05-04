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
    class CommonData
    {
        public static string aramisFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace('\\', '/') + "/.aramis/";
        public static string nativeFolder = aramisFolder + "bin/natives/";
        public static string launcherProfileFilePath = aramisFolder + "launcher_profile.json";
        public static LauncherProfileJson launcherProfileJson = new LauncherProfileJson();

        /* Custom link */
        public static string actualityURL = "https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/actualites/actualites.rtf";
        public static string packageInfoJsonURL = "https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/downloadInfos/packageInfos.json";
        public static string packageInfoBaseURL = "https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/downloadInfos/";

        /* Mojang api links */
        public static string authServerValidateURL = "https://authserver.mojang.com/validate";
        public static string authServerRefreshURL = "https://authserver.mojang.com/refresh";
        public static string authServerAuthenticateURL = "https://authserver.mojang.com/authenticate";

        public static string playerSessionURL = "https://sessionserver.mojang.com/session/minecraft/profile/";

        public class LauncherProfileJson
        {
            public AuthResponse authenticationDatabase;
            public Property userProperty;
            public string installedPackageVersion;
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
