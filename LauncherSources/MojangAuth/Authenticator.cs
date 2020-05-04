using AramisLauncher.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using static AramisLauncher.MojangAuth.AuthFormat;

namespace AramisLauncher.MojangAuth
{
    class Authenticator
    {
        public static bool ValidateToken(string accessToken, string clientToken)
        {
            bool tokenValidated = false;
            ValidateRequest validateRequest = new ValidateRequest(accessToken, clientToken);

            HttpWebRequest httpValidateWebRequest = (HttpWebRequest)WebRequest.Create(CommonData.authServerValidateURL);
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
                        tokenValidated = true;
                    }
                }
                catch (Exception)
                {
                    tokenValidated = RefreshToken(accessToken, clientToken);
                }
            }

            return tokenValidated;
        }

        public static bool RefreshToken(string accessToken, string clientToken)
        {
            bool tokenRefreshed = false;
            ValidateRequest validateRequest = new ValidateRequest(accessToken, clientToken);

            /* token is not more valid, ask to revalidate it */
            HttpWebRequest httpRefreshWebRequest = (HttpWebRequest)WebRequest.Create(CommonData.authServerRefreshURL);
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
                        /* Connected */
                        string result = streamReader.ReadToEnd();
                        RefreshResponse refreshResponse = JsonConvert.DeserializeObject<RefreshResponse>(result);
                        CommonData.launcherProfileJson.authenticationDatabase.accessToken = refreshResponse.accessToken;
                        tokenRefreshed = true;
                    }
                }
                catch (Exception)
                {
                    /* ERROR to revalidate the token */
                }
            }

            CommonData.saveLauncherProfile();

            return tokenRefreshed;
        }

        public static bool AuthenticateToMinecraft(string username, string password)
        {
            bool connectedResult = false;
            AuthRequest authRequest = new AuthRequest(username, password);
            AuthResponse authResponse = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CommonData.authServerAuthenticateURL);
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
                        CommonData.launcherProfileJson.authenticationDatabase = authResponse;
                        PlayerProfile.GetPropertiesProfile(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.id);
                        connectedResult = true;
                    }
                }
                catch (Exception)
                {
                    CommonData.launcherProfileJson.authenticationDatabase = null;
                }
            }

            CommonData.saveLauncherProfile();

            return connectedResult;
        }
    }
}
