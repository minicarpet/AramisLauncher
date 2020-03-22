using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AramisLauncher.Minecraft.Authenticator;

namespace AramisLauncher.Minecraft
{
    class LauncherProfile
    {
        public class AppVersion
        {
            public int major = 0;
            public int minor = 1;
            public int build = 0;
        }
        public class LauncherVersion
        {
            public string name = "AramisLauncher";
            public AppVersion verison = new AppVersion();
        }

        public class LauncherProfileJson
        {
            public AuthResponse authenticationDatabase;
            public LauncherVersion launcherVersion = new LauncherVersion();
        }
    }
}
