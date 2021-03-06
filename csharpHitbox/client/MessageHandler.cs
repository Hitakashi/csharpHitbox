﻿using System;
using System.Linq;
using System.Text;
using csharpHitbox.bot;
using csharpHitbox.utils;
using Newtonsoft.Json.Linq;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.client
{
    public static class MessageHandler
    {
        private const String BaseMessage = "5:::{\"name\":\"message\",\"args\":[{";

        public static void handle(Client client, String data)
        {
            if (data.Equals("2::"))
            {
                client.Send(data);
            }
            else if (data.Equals("0::"))
            {
                // We've been kicked from the server.
                Bot.Destroy();
                Logger.Error("The server has terminated the connection.");
            }
            else if (data.StartsWith("5:::"))
            {
                var jsonObj = JObject.Parse(data.Substring(4));
                var args = JObject.Parse(jsonObj.GetValue("args").First.ToString());
                var method = args.GetValue("method");
                var paramsObject = args.GetValue("params");

                // We don't care about buffer messages - Well, I don't.  
                // This will ignore chat logs. If you don't want buffer chatMsgs move this inside the switch for chatMsg
                if (paramsObject.SelectToken("buffer") != null) return;

                switch (method.ToString())
                {
                    case "loginMsg":
                        // Successfully authenticated
                        break;
                    case "chatMsg":
                        var sender = paramsObject.SelectToken("name").ToString();
                        var message = paramsObject.SelectToken("text").ToString();
                        var rights = paramsObject.SelectToken("role").ToString();
                        var sub = paramsObject.SelectToken("isSubscriber").ToObject<bool>();
                        var follow = paramsObject.SelectToken("isFollower").ToObject<bool>();

                        if (sender.ToLower().Equals(Settings.USERNAME.ToLower())) return;

                        if (message.StartsWith("!") && message.Length > 1)
                        {
                            CommandHandler.Handle(client, sender, rights, message.Trim(), sub, follow);
                        }
                        break;
                    case "infoMsg":
                        break;
                    case "chatLog":
                        break;

                    #region List of other methods sent from the server

                    case "userList":
                        break;
                    case "userInfo":
                        break;

                    case "slowMsg":
                        break;

                    // Raffle
                    case "raffleMsg":
                        break;
                    case "winnerRaffle":
                        break;
                    // Poll
                    case "pollMsg":
                        break;
                    // MOTD
                    case "motdMsg":
                        break;

                    // Server Msg
                    case "serverMsg":
                        // Title or Game changed or host mode changed.
                        break;

                    #endregion

                    default:
                        Logger
                            .Info("Warning! Unknown command from server! Method: " + method + " Params: " + paramsObject);
                        break;
                }
            }
        }
        #region Connection

        public static void SendJoinRequest(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":\"joinChannel\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":\"").Append(client.GetChannel().ToLower()).Append("\",");
            //sb.Append("\"name\":\"").Append(Settings.USERNAME.ToLower()).Append("\",");
            //sb.Append("\"token\":\"").Append(Settings.AUTH);
            //sb.Append("\"}}]}");

            //Logger.Info("[Client@" + client.GetChannel() + "] Sending join request...");

            //client.Send(sb.ToString());

            Message message = new Message("joinChannel");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", Settings.USERNAME.ToLower());
            message.Params.Add("token", Settings.AUTH);

            Logger.Info("[Client@" + client.GetChannel() + "] Sending join request...");
            client.Send(message.ToString());
        }

        public static void SendQuitMessage(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":\"partChannel\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"name\":\"").Append(Settings.USERNAME).Append("\"");
            //sb.Append("}}]}");

            //Logger.Info("[Client@" + client.GetChannel() + "] Sending quit message...");
            //client.Send(sb.ToString());

            Message message = new Message("joinChannel");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", Settings.USERNAME.ToLower());

            Logger.Info("[Client@" + client.GetChannel() + "] Sending quit message...");
            client.Send(message.ToString());
        }

        #endregion

        #region General

        public static void SendMessage(Client client, String msg)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":\"chatMsg\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"name\":\"").Append(Settings.USERNAME).Append("\",");
            //sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\",");
            //sb.Append("\"text\":\"").Append(msg).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("chatMsg");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", Settings.USERNAME);
            message.Params.Add("nameColor", Settings.HEXCOLOR);
            message.Params.Add("text", msg);
            client.Send(message.ToString());
        }

        public static void SendUserInfoRequest(Client client, String user)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"getChannelUser\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            //sb.Append("\"name\":").Append("\"").Append(user).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("getChannelUser");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", user);
            client.Send(message.ToString());
        }

        public static void SendUserListRequest(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"getChannelUserList\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("getChannelUserList");
            message.Params.Add("channel", client.GetChannel().ToLower());

            client.Send(message.ToString());
        }

        #endregion

        #region Giveaway

        // Max Choices is 11
        public static void SendCreateRaffle(Client client, String question, String prize, string subscriber,
            string follower, params String[] choices)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"createRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"question\":").Append("\"").Append(question).Append("\",");
            sb.Append("\"prize\":").Append("\"").Append(prize).Append("\",");
            sb.Append("\"choices\":[");
            var last = choices.Last();
            foreach (var c in choices)
            {
                sb.Append("{");
                sb.Append("\"text\":").Append("\"").Append(c).Append("\"");
                sb.Append("}");
                if (!last.Equals(c))
                    sb.Append(",");
            }
            sb.Append("],");
            sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\",");
            sb.Append("\"subscriberOnly\":").Append(subscriber).Append(",");
            sb.Append("\"followerOnly\":").Append(follower);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendPauseRaffle(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"pauseRaffle\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("pauseRaffle");
            message.Params.Add("channel", client.GetChannel().ToLower());
            client.Send(message.ToString());
        }

        public static void SendEndRaffle(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"endRaffle\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("endRaffle");
            message.Params.Add("channel", client.GetChannel().ToLower());
            client.Send(message.ToString());
        }

        public static void SendStartRaffle(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"startRaffle\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("startRaffle");
            message.Params.Add("channel", client.GetChannel().ToLower());
            client.Send(message.ToString());
        }

        public static void SendHideRaffle(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"hideRaffle\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("hideRaffle");
            message.Params.Add("channel", client.GetChannel().ToLower());
            client.Send(message.ToString());
        }

        public static void SendCleanupRaffle(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"cleanupRaffle\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("cleanupRaffle");
            message.Params.Add("channel", client.GetChannel().ToLower());
            client.Send(message.ToString());
        }

        // Winner must be a number as a string. Winning answer starts at 0.
        public static void SendPickWinner(Client client, String winner)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"winnerRaffle\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            //sb.Append("\"answer\":").Append("\"").Append(winner).Append("\"");
            //sb.Append("}}]}");
            //client.Send(sb.ToString());

            Message message = new Message("winnerRaffle");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("answer", winner);
            client.Send(message.ToString());
        }

        #endregion

        #region Poll

        // Max choices is 11
        public static void SendCreatePoll(Client client, String question, String subscriber, String follower,
            params String[] choices)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"createPoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"question\":").Append("\"").Append(question).Append("\",");
            sb.Append("\"choices\":[");
            var last = choices.Last();
            foreach (var c in choices)
            {
                sb.Append("{");
                sb.Append("\"text\":").Append("\"").Append(c).Append("\"");
                sb.Append("}");
                if (!last.Equals(c))
                    sb.Append(",");
            }
            sb.Append("],");
            sb.Append("\"subscriberOnly\":").Append(subscriber).Append(",");
            sb.Append("\"followerOnly\":").Append(follower).Append(",");
            sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendPausePoll(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"pausePoll\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            //sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());


            Message message = new Message("pausePoll");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("token", Settings.AUTH);
            client.Send(message.ToString());
        }

        public static void SendStartPoll(Client client)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"startPoll\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            //sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("startPoll");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("token", Settings.AUTH);
            client.Send(message.ToString());
        }

        public static void SendEndPoll(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"endPoll\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            //sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("endPoll");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("token", Settings.AUTH);
            client.Send(message.ToString());
        }

        #endregion

        #region Moderation

        public static void SendKickUser(Client client, String user, int time)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"kickUser\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"name\":").Append("\"").Append(user).Append("\",");
            //sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\",");
            //sb.Append("\"timeout\":").Append(time);
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("kickUser");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", user);
            message.Params.Add("token", Settings.AUTH);
            message.Params.Add("timeout", time);
            client.Send(message.ToString());
        }

        public static void SendBanUser(Client client, String user)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"banUser\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"name\":").Append("\"").Append(user).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("banUser");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", user);
            client.Send(message.ToString());
        }

        public static void SendUnBanUser(Client client, String user)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"unbanUser\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"name\":").Append("\"").Append(user).Append("\",");
            //sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("unbanUser");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", user);
            message.Params.Add("token", Settings.AUTH);
            client.Send(message.ToString());
        }

        public static void SendSlowMode(Client client, int time)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"slowMode\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"time\":").Append(time);
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("slowMode");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("time", time);
            client.Send(message.ToString());
        }

        public static void SendSubOnlyMode(Client client, int time, String toggle)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"slowMode\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"subscriber\":").Append(toggle).Append(",");
            //sb.Append("\"rate\":").Append(time);
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("slowMode");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("subscriber", toggle);
            message.Params.Add("rate", time);
            client.Send(message.ToString());
        }

        #endregion

        #region MOTD

        public static void SendMotd(Client client, String text)
        {
            //var sb = new StringBuilder(BaseMessage);
            //sb.Append("\"method\":").Append("\"motdMsg\",");
            //sb.Append("\"params\":{");
            //sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            //sb.Append("\"name\":").Append("\"").Append(Settings.USERNAME).Append("\",");
            //sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\",");
            //sb.Append("\"text\":").Append("\"").Append(text).Append("\"");
            //sb.Append("}}]}");

            //client.Send(sb.ToString());

            Message message = new Message("motdMsg");
            message.Params.Add("channel", client.GetChannel().ToLower());
            message.Params.Add("name", Settings.USERNAME);
            message.Params.Add("nameColor", Settings.HEXCOLOR);
            message.Params.Add("text", text);
            client.Send(message.ToString());
        }

        #endregion

        private class Message
        {
            private readonly JObject _root;

            public readonly JObject Params;

            public Message(String method)
            {
                _root = new JObject();
                var argsRoot = new JObject();
                Params = new JObject();
                var args = new JArray();

                _root.Add("name", "message");
                _root.Add("args", args);
                argsRoot.Add("method", method);
                argsRoot.Add("params", Params);
                args.Add(argsRoot);
            }

            public override string ToString()
            {
                return "5:::" + _root;
            }
        }
    }
}