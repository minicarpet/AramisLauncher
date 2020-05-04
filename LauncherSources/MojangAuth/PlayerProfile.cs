using AramisLauncher.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.MojangAuth
{
    class PlayerProfile
    {
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
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CommonData.playerSessionURL + uuid);
            httpWebRequest.Method = "GET";

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    ProfileResponse profileResponse = JsonConvert.DeserializeObject<ProfileResponse>(result);
                    CommonData.launcherProfileJson.userProperty = profileResponse.Properties[0];
                }
            }
            catch (Exception)
            {
                CommonData.launcherProfileJson.userProperty = null;
            }
        }
    }
}
