using AramisLauncher.Common;
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

        private class ValidateRequest
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

        public static void ValidateToken(string accessToken, string clientToken)
        {
            ValidateRequest validateRequest = new ValidateRequest(accessToken, clientToken);

            HttpWebRequest httpValidateWebRequest = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/validate");
            httpValidateWebRequest.ContentType = "application/json";
            httpValidateWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(httpValidateWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(validateRequest);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                try
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)httpValidateWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                        MainWindow.ChangeConnectionState(true);
                    }
                }
                catch (Exception validateException)
                {
                    RefreshToken(accessToken, clientToken);
                }
            }
        }

        public static void RefreshToken(string accessToken, string clientToken)
        {
            ValidateRequest validateRequest = new ValidateRequest(accessToken, clientToken);

            /* token is not more valid, ask to revalidate it */
            HttpWebRequest httpRefreshWebRequest = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/refresh");
            httpRefreshWebRequest.ContentType = "application/json";
            httpRefreshWebRequest.Method = "POST";

            using (StreamWriter refreshStreamWriter = new StreamWriter(httpRefreshWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(validateRequest);

                refreshStreamWriter.Write(json);
                refreshStreamWriter.Flush();
                refreshStreamWriter.Close();

                try
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)httpRefreshWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                        RefreshResponse refreshResponse = JsonConvert.DeserializeObject<RefreshResponse>(result);
                        CommonData.launcherProfileJson.authenticationDatabase.accessToken = refreshResponse.accessToken;
                        MainWindow.ChangeConnectionState(true);
                    }
                }
                catch (Exception refreshException)
                {
                    /* ERROR to revalidate the token */
                    MainWindow.ChangeConnectionState(false);
                }
            }
        }

        public static void AuthenticateToMinecraft(string username, string password)
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
                        CommonData.setAuthenticateProfile(authResponse);
                        GetPropertiesProfile(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.id);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public class Property
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class ProfileResponse
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("properties")]
            public Property[] Properties { get; set; }
        }

        public static void GetPropertiesProfile(string uuid)
        {
            ProfileResponse profileResponse = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://sessionserver.mojang.com/session/minecraft/profile/" + uuid);
            httpWebRequest.Method = "GET";

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    profileResponse = JsonConvert.DeserializeObject<ProfileResponse>(result);
                    CommonData.setPropertyProfile(profileResponse.Properties[0]);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
