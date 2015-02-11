using System;
using System.Collections.Concurrent;
using csharpHitbox.api;
using csharpHitbox.client;
using csharpHitbox.utils;

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
        public static Logger logger = new Logger();
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

        public void RestartClient(String channel) {
            RemoveClient(channel);
            AddClient(channel);
        }

        public static void AddClient(String channel)
        {
            try
            {
                Logger.Log("Creating Client@" + channel);
                Client client = new Client(channel.ToLower());
                Clients.TryAdd(channel.ToLower(), client);
                try
                {
                    client.Connect();
                    //if (httpServer != null) return;
                    //httpServer = new MyHttpServer(80);
                    
                    //Thread thread = new Thread((httpServer.listen));
                    //thread.Start();
                    
                    //httpServer = new Listener();
                    if (Settings.APISERVER)
                        Program.Main();

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
        public static void RemoveClient(String channel)
        {
            if (Clients.ContainsKey(channel))
            {
                Client c;
                Clients.TryGetValue(channel, out c);
                if (c != null) MessageHandler.SendQuitMessage(c);
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
                    MessageHandler.SendQuitMessage(client);
                    client.Close();
                }
                catch (Exception ignored)
                {

                }
            }
        }

        public static void RestartBot()
        {
            foreach (Client client in Clients.Values)
            {
                try
                {
                    MessageHandler.SendQuitMessage(client);
                    RemoveClient(client.GetChannel());
                    AddClient(client.GetChannel());
                }
                catch (Exception ignoredException)
                {
                    
                }
            }
        }
    }
}