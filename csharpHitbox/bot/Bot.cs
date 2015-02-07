using System;
using System.Collections.Concurrent;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.bot
{
    public class Bot
    {
        public static ConcurrentDictionary<String, Client> Clients = new ConcurrentDictionary<String, Client>();
        private static Bot instance;

        public Bot()
        {
            foreach (var channel in Settings.CHANNEL)
            {
                AddClient(channel);
            }
        }

        public static Bot GetInstance()
        {
            if (instance == null)
                instance = new Bot();
            return instance;
        }

        public void AddClient(String channel)
        {
            try
            {
                Console.WriteLine("Creating Client@" + channel);
                Client client = new Client(channel.ToLower());
                Clients.TryAdd(channel.ToLower(), client);
                try
                {
                    client.Connect();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
        public void RemoveClient(String channel)
        {
            if (Clients.ContainsKey(channel))
            {
                Client c;
                Clients.TryGetValue(channel, out c);
                if (c != null) c.GetMessageHandler().SendQuitMessage();
                if (c != null) c.Close();
                Clients.TryRemove(channel, out c);
            }
        }

        public static void Destroy()
        {
            foreach (Client client in Clients.Values)
            {
                try
                {
                    client.GetMessageHandler().SendQuitMessage();
                    client.Close();
                }
                catch (Exception ignored)
                {

                }
            }
        }
    }
}