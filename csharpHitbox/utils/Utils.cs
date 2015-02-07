using System;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.utils
{
    public class Utils
    {
        public static Boolean CheckSettings()
        {
            if (Settings.CHANNEL.Length != 0
                && !Settings.USERNAME.Equals("", StringComparison.CurrentCultureIgnoreCase)
                && !Settings.PASSWORD.Equals("", StringComparison.CurrentCultureIgnoreCase))
            {
                Settings.AUTH = FetchAuth();
                Settings.WS_IP = FetchWsAddress();
                return true;
            }
            Console.WriteLine("FATAL: Something is missing from your configuration file.");
            return false;
        }

        public static string FetchWsAddress()
        {
            using (var client = new WebClient())
            {
                return
                    JArray.Parse(client.DownloadString(Settings.API.CHAT_SERVERS))
                        .First.SelectToken("server_ip")
                        .ToString();
            }

            //[{"server_ip":"ec2-54-145-39-121.compute-1.amazonaws.com"},{"server_ip":"ec2-54-221-0-139.compute-1.amazonaws.com"}]
        }

        public static String FetchWsAuth()
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(Settings.Links.WS_AUTH);
                client.Dispose();
                return json.Substring(0, json.IndexOf(":", StringComparison.Ordinal));
            }
        }

        public static String FetchAuth()
        {
            if (Settings.USERNAME.Equals("", StringComparison.CurrentCultureIgnoreCase) ||
                Settings.PASSWORD.Equals("", StringComparison.CurrentCultureIgnoreCase))
            {
                Environment.Exit(1);
            }

            var client = new WebClient();
            var json = new JavaScriptSerializer().Serialize(new
            {
                login = Settings.USERNAME,
                pass = Settings.PASSWORD
            });

            var jsonObject = client.UploadString(Settings.API.AUTH_TOKEN, json);
            if (JObject.Parse(jsonObject).SelectToken("authToken") != null)
            {
                client.Dispose();
                return JObject.Parse(jsonObject).SelectToken("authToken").ToString();
            }
            Environment.Exit(1);
            return null;
        }
    }
}