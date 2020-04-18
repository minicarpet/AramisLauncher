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
        public class LauncherProfileJson
        {
            public AuthResponse authenticationDatabase;
            public Property property;
            public string installedVersion;
        }
    }
}
