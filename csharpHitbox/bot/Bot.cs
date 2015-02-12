using System;
using System.Collections.Concurrent;
using System.Linq;
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
        private static Bot instance;
        private static Boolean canJoin = true;

        public Bot()
        {
            foreach (var channel in Settings.CHANNEL)
            {
                AddClient(channel);
            }
            if (Settings.APISERVER)
                Program.Main();
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

        // This should ONLY be used on startup or anything else that doesn't run within a channel.
        private static void AddClient(String channel)
        {
            if (!canJoin)
                return;
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


        // This should be used for !join commands or anything similar so we can tell users that they cannot join.
        public static void AddClient(Client client2, String channel)
        {
            if (!canJoin)
            {
                MessageHandler.SendMessage(client2,
                    "I cannot join channels at this time. Please try again in a few minutes.");
                return;
            }

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
                catch (Exception)
                {

                }
            }
            Environment.Exit(0);
        }

        public static void RestartBot()
        {
            // We need to disallow joining to channels until the restart is done.
            canJoin = false;
            // We need to copy the original clients before removing all of them.
            ConcurrentDictionary<String, Client> a = Clients;

            foreach (Client client in Clients.Values)
            {
                try
                {
                    // Let default channels stay in the bot.  Usually the default channel is used for the bots home for join commands.
                    if (Settings.CHANNEL.Contains(client.GetChannel()))
                        continue;
                    MessageHandler.SendQuitMessage(client);
                    RemoveClient(client.GetChannel());
                }
                catch (Exception)
                {
                    
                }
            }

            // Add

            // Now we need to loop the copy of Clients and add them back.

            foreach (String clients in a.Keys)
            {
                if (Settings.CHANNEL.Contains(clients))
                    continue;
                AddClient(clients);
            }

            // Just in case. Want GC to take care of this ASAP
            a.Clear();
            canJoin = true;
        }
    }
}