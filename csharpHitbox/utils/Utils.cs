using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using csharpHitbox.bot;
using Newtonsoft.Json.Linq;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.utils
{
    public static class Utils
    {
        public static Boolean CheckSettings()
        {
            Console.WriteLine(Environment.GetEnvironmentVariable("platform"));
            Console.WriteLine(Environment.GetEnvironmentVariable("username"));
            if (Environment.GetEnvironmentVariable("platform") == "heroku")
            {
                String c = Environment.GetEnvironmentVariable("username");
                String p = Environment.GetEnvironmentVariable("pasword");
                if (c != null && p != null &&
                    (!c.Equals("", StringComparison.OrdinalIgnoreCase) ||
                     !p.Equals("", StringComparison.OrdinalIgnoreCase)))
                {
                    Settings.USERNAME = Environment.GetEnvironmentVariable("username");
                    Settings.PASSWORD = Environment.GetEnvironmentVariable("password");
                }
            } else if (Settings.CHANNEL.Length != 0
                && !Settings.USERNAME.Equals("", StringComparison.OrdinalIgnoreCase)
                && !Settings.PASSWORD.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                Settings.AUTH = FetchAuth();
                Settings.WS_IP = FetchWsAddress();
                return true;
            }
            Logger.Error("[SettingsCheck]: Failed. Something is missing from your configuration");
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

        public static int GetRightsForString(String rights)
        {
            switch (rights.ToLower())
            {
                case "user":
                case "mod":
                    return 1;
                case "admin":
                case "broadcaster":
                    return 2;
                default:
                    return 0;
            }
        }

        public static String StripCommandData(String data)
        {
            List<Regex> reg = new List<Regex>();
            reg.Add(new Regex("<a [^\\s]+ [^\\s>]+>([^<]+)</a>", RegexOptions.IgnoreCase));
            reg.Add(new Regex("<div class=\"image\"><img src=\"([^\"]+)\" /></div>", RegexOptions.IgnoreCase));
            reg.Add(new Regex("<div class=\"video\">[^/]+([^\"]+)[^>]+>[^>]+>[^>]+>[^>]+>", RegexOptions.IgnoreCase));

            data = reg.Aggregate(data, (current, variable) => variable.Replace(current, "$1"));
            data = data.Replace("//www.youtube.com/embed/", "https://www.youtube.com/watch?v=");
            return data;
        }


        #region API

        public static object Post(string url, string json)
        {
            try
            {
                WebClient c = new WebClient();
                var responseFromServer = c.UploadString(url, json);

                c.Dispose();
                return responseFromServer;
            }
            catch (WebException exception)
            {
                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
        }

        public static void PostAync(string url, string json)
        {
            try
            {
                WebClient c = new WebClient();
                Uri url2 = new Uri(url);
                c.UploadStringAsync(url2, json);

                c.Dispose();
            }
            catch (WebException exception)
            {
                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                }
            }
        }

        public static object Put(string url, string json)
        {
            try
            {
                WebClient c = new WebClient();
                var responseFromServer = c.UploadString(url, "PUT", json);

                c.Dispose();
                return responseFromServer;
            }
            catch (WebException exception)
            {
                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
        }

        public static void PutAync(string url, string json)
        {
            try
            {
                WebClient c = new WebClient();
                Uri url2 = new Uri(url);
                c.UploadStringAsync(url2, "PUT", json);

                c.Dispose();
            }
            catch (WebException exception)
            {
                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                }
            }
        }

        public static object Get(string url)
        {
            try
            {
                WebClient c = new WebClient();

                var responseFromServer = c.DownloadString(url);

                c.Dispose();
                //Trace.WriteLine(String.Format("GetApi Data: {0}", responseFromServer));
                return responseFromServer;
            }
            catch (WebException exception)
            {
                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
        }

        #endregion
    }
}