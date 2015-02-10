using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using csharpHitbox.bot;
using csharpHitbox.commands.impl;
using csharpHitbox.utils;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.client
{
    public static class CommandHandler
    {


        public static void Handle(Client client, String sender, String rights, String data)
        {
            string cmd, @params = "null";
            data = stripCommandData(data);

            if (data.Contains(" "))
            {
                cmd = data.Substring(1, data.IndexOf(" ")).Trim();
                @params = data.Substring(data.IndexOf(" ")).Trim();
            }
            else
            {
                cmd = data.Substring(1);
            }

            var infoBuilder = new StringBuilder();
            infoBuilder.Append("[CommandHandler@").Append(client.GetChannel()).Append("] ");
            infoBuilder.Append("cmd=[").Append(cmd).Append("],");
            infoBuilder.Append("params=[").Append(@params).Append("],");
            infoBuilder.Append("user=[name=[").Append(sender).Append("],rights=[");
            infoBuilder.Append(Utils.GetRightsForString(rights)).Append("]]");

            Bot.getLogger().Info(infoBuilder.ToString());

            switch (cmd.ToLower())
            {
                case "helloworld":
                    new HelloWorld().Execute(client, Utils.GetRightsForString(rights), sender, @params);
                    break;
                default:
                    Console.WriteLine("Unknown Command: " + cmd.ToLower());
                    break;
            }
        }

        public static String stripCommandData(String data)
        {
            List<Regex> reg = new List<Regex>();
            reg.Add(new Regex("<a [^\\s]+ [^\\s>]+>([^<]+)</a>", RegexOptions.IgnoreCase));
            reg.Add(new Regex("<div class=\"image\"><img src=\"([^\"]+)\" /></div>", RegexOptions.IgnoreCase));
            reg.Add(new Regex("<div class=\"video\">[^/]+([^\"]+)[^>]+>[^>]+>[^>]+>[^>]+>", RegexOptions.IgnoreCase));

            foreach (Regex VARIABLE in reg)
            {
                var x = VARIABLE.Replace(data, "$1");
                data = x;
            }
            data = data.Replace("//www.youtube.com/embed/", "https://www.youtube.com/watch?v=");
            return data;
        }
    }
}