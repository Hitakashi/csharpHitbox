using System;
using System.Text;
using csharpHitbox.bot;
using Nancy;
using Nancy.Extensions;
using Nancy.Json;
using Newtonsoft.Json.Linq;

namespace csharpHitbox.api
{
    public class SettingModule : NancyModule
    {
        public SettingModule()
        {
            Get["/channel/{channel}"] = _ =>
            {
                if (!Bot.Clients.ContainsKey(_.channel))
                    ApiServer.NotFound();

                var json = Encoding.UTF8.GetBytes(GetChannelSettings(_.channel));
                return new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(json, 0, json.Length)
                };
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

            Post["/channel/{channel}"] = _ =>
            {
                if (!Bot.Clients.ContainsKey(_.channel))
                    ApiServer.NotFound();

                var json = Encoding.UTF8.GetBytes(UpdateChannelSettings(_.channel, Request.Body.AsString()));
                return new Response
                {
                    ContentType = "application/json",
                    Contents = s => s.Write(json, 0, json.Length)
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

        private static string UpdateChannelSettings(String channel, String postrequest)
        {
            var json2 = JObject.Parse(postrequest);
            var setting2 = json2.SelectToken("settings");

            // For now we're just updating linkBan
            foreach (JProperty VARIABLE in setting2)
            {
                switch (VARIABLE.Name)
                {
                    case "linkBan":
                        ExampleSetting.UpdateLinkSetting(channel, VARIABLE.Value.ToObject<Boolean>());
                        break;
                }
            }

            var json = GetChannelSettings(channel);
            return json;
        }
    }
}