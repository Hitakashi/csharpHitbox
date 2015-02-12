using System;
using System.Text;
using csharpHitbox.bot;
using Nancy;
using Nancy.Json;

namespace csharpHitbox.api
{
    public class SettingModule : NancyModule
    {
        public SettingModule()
        {
            Get["/channel/{channel}"] = _ =>
            {
                var json = Encoding.UTF8.GetBytes(GetChannelSettings(_.channel));
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
                var jsonBytes = Encoding.UTF8.GetBytes(ListChannels());
                return new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
                };
            };
        }

        private static string ListChannels()
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                status = "success",
                total_channels = Bot.Clients.Count
            });

            return json;
        }

        private static string GetChannelSettings(String channel)
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
}