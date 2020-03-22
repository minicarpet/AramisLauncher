using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AramisLauncher.Minecraft.Authenticator;
using static AramisLauncher.Minecraft.LauncherProfile;

namespace AramisLauncher.Common
{
    class CommonData
    {
        public static string aramisFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace('\\', '/') + "/.aramis/";
        public static string nativeFolder = aramisFolder + "bin/natives/";
        public static string launcherProfileFilePath = aramisFolder + "launcher_profile.json";
        public static LauncherProfileJson launcherProfileJson = new LauncherProfileJson();

        public static void saveLauncherProfile()
        {
            if(Directory.Exists(aramisFolder))
            {
                Directory.CreateDirectory(aramisFolder);
            }

            if(File.Exists(launcherProfileFilePath))
            {
                File.Delete(launcherProfileFilePath);
            }

            File.WriteAllText(launcherProfileFilePath, JsonConvert.SerializeObject(launcherProfileJson));
        }

        public static void setAuthenticateProfile(AuthResponse authResponse)
        {
            launcherProfileJson.authenticationDatabase = authResponse;
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
