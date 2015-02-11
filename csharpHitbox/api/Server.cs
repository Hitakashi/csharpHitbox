using System;
using System.Collections.Generic;
using System.Text;
using csharpHitbox.bot;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.Json;

namespace csharpHitbox.api
{
    public static class Program
    {
        // The name for these variable will show in the API.
        public static Dictionary<string, string> settings = new Dictionary<string, string>();

        public static void Main()
        {
            try
            {
                host.Start();
                Console.WriteLine("API Server is running.");
            }
            catch (AutomaticUrlReservationCreationFailureException)
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    if (config.RewriteLocalhost)
                    {
                        NetSh.AddUrlAcl("http://+:" + uri.Port + "/", "Everyone");
                    }
                    else
                    {
                        NetSh.AddUrlAcl(uri.OriginalString, "Everyone");
                    }
                    host.Start();
                }
                else
                {
                    Console.WriteLine("Non-Windows System found. Please restart with elevated permissions.");
                }
            }
        }

        public static string ListChannels()
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                status = "success",
                total_channels = Bot.Clients.Count
            });

            return json;
        }

        public static string GetChannelSettings(String channel)
        {
            var channelExists = Bot.Clients.ContainsKey(channel);
            var json = new JavaScriptSerializer().Serialize(new
            {
                status = channelExists ? "success" : "not found",
                channel = new
                {
                    name = channel
                },
                settings
            });

            return json;
        }

        private static readonly Uri uri = new Uri("http://localhost:80/");
        private static readonly HostConfiguration config = new HostConfiguration {RewriteLocalhost = true};
        private static readonly NancyHost host = new NancyHost(config, uri);
    }

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/channel/{channel}"] = _ =>
            {
                var json = Encoding.UTF8.GetBytes(Program.GetChannelSettings(_.channel));
                var re = new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(json, 0, json.Length)
                };

                if (!Bot.Clients.ContainsKey(_.channel))
                    re.StatusCode = HttpStatusCode.NotFound;

                return re;
            };

            Get["/channel/count"] = _ =>
            {
                var jsonBytes = Encoding.UTF8.GetBytes(Program.ListChannels());
                return new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
                };
            };
        }
    }
}