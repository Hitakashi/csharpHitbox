using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using csharpHitbox.bot;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.Json;

namespace csharpHitbox.api
{
    public static class ApiServer
    {
        private static readonly Uri uri = new Uri("http://localhost:80/");
        private static readonly HostConfiguration config = new HostConfiguration {RewriteLocalhost = true};
        private static readonly NancyHost host = new NancyHost(config, uri);

        public static void Main()
        {
            try
            {
                host.Start();
                if (Settings.DEBUGGING)
                    StaticConfiguration.DisableErrorTraces = false;

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
                    Console.WriteLine("API Server is running.");
                }
            }
            catch (SocketException)
            {
                // Need to capture a SocketException for Linux/MacOSX if the program isn't ran with sudo
                Console.WriteLine("Linux/OSX: Program must be ran with mono due to using port 80.");
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
                settings = new
                {
                    linkBan = ExampleSetting.GetLinkSetting(channel)
                }
            });

            return json;
        }
    }

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/channel/{channel}"] = _ =>
            {
                var json = Encoding.UTF8.GetBytes(ApiServer.GetChannelSettings(_.channel));
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
                var jsonBytes = Encoding.UTF8.GetBytes(ApiServer.ListChannels());
                return new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
                };
            };
        }
    }
}