using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.Minecraft
{
    class Authenticator
    {
        private class AuthAgent
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
        private class AuthRequest
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

        public static AuthResponse AuthenticateToMinecraft(string username, string password)
        {
            AuthRequest authRequest = new AuthRequest(username, password);
            AuthResponse authResponse = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/authenticate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(authRequest);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                try
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                        authResponse = JsonConvert.DeserializeObject<AuthResponse>(result);
                    }
                }
                catch (Exception)
                {

                }
            }

            return authResponse;
        }
    }
}
