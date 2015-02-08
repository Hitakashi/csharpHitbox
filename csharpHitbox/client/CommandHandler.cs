using System;
using System.Text;
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
    public class CommandHandler
    {
        private readonly Client client;

        public CommandHandler(Client client)
        {
            this.client = client;
        }

        public void Handle(String sender, String rights, String data)
        {
            string cmd, @params = "null";

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
    }
}