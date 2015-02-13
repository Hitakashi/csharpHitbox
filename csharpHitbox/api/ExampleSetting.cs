using System;
using System.Collections.Generic;

namespace csharpHitbox.api
{
    public static class ExampleSetting
    {
        // Lets say this setting is for banning links. Channels are Hitakashi and Hitakashi-Test. One is true other is false.
        // I'll store the 'settings' in a Dictionary but you should probably use a databse.
        private static Dictionary<String, Boolean> linkBan = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);

        // This will run ONE TIME once the class is called from anywhere.
        static ExampleSetting()
        {
            // You would probably want to replace the Dictionary and the following code with code to grab the setting via a database, a json file, or anything else.
            linkBan.Add("Hitakashi", true);
            linkBan.Add("Hitakashi-Test", false);
        }

        public static Boolean GetLinkSetting(String channel)
        {
            // DB Example would be something like
            // return DB.GetInstance().GetSetting("linkBan");

            Boolean a;
            linkBan.TryGetValue(channel, out a);
            return a;
        }

        public static void UpdateLinkSetting(String channel, Boolean val)
        {
            linkBan[channel] = val;
        }
    }
}