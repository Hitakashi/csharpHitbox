using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
        public static void Handle(Client client, String sender, String rights, String data, bool sub, bool follow)
        {
            string cmd, @params = "null";
            data = Utils.StripCommandData(data);

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

            Logger.Info(infoBuilder.ToString());

            switch (cmd.ToLower())
            {
                case "helloworld":
                    HelloWorld.Execute(client, Utils.GetRightsForString(rights), sender, @params, sub, follow);
                    break;
                default:
                    Console.WriteLine("Unknown Command: " + cmd.ToLower());
                    break;
            }
        }
    }

    public abstract class Rights
    {
        public const int User = 0;
        public int Mod = 1;
        public int Broadcaster = 2;
    }
}