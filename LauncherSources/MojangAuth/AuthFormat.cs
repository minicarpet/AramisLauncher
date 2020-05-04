using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.MojangAuth
{
    class AuthFormat
    {
        public class AuthAgent
        {
            public static AuthAgent MINECRAFT = new AuthAgent("Minecraft", 1);
            public static AuthAgent SCROLLS = new AuthAgent("Scrolls", 1);
            public string name;
            public int version;

            public AuthAgent(string authname, int authversion)
            {
                name = authname;
                version = authversion;
            }
        }
        public class AuthRequest
        {
            public AuthAgent agent = AuthAgent.MINECRAFT;
            public string username;
            public string password;
            public AuthRequest(string authUserName, string authPassword)
            {
                username = authUserName;
                password = authPassword;
            }
        }

        public class ValidateRequest
        {
            public string accessToken;
            public string clientToken;
            public ValidateRequest(string validateAccessToken, string validateClientToken)
            {
                accessToken = validateAccessToken;
                clientToken = validateClientToken;
            }
        }

        public class AuthProfile
        {
            public string name;
            public string id;
        }

        public class AuthResponse
        {
            public string accessToken;
            public string clientToken;
            public AuthProfile[] availableProfiles;
            public AuthProfile selectedProfile;
        }

        public class RefreshResponse
        {
            public string accessToken;
            public string clientToken;
            public AuthProfile selectedProfile;
        }
    }
}
