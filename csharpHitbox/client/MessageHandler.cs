using System;
using System.Linq;
using System.Text;
using csharpHitbox.bot;
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
                Bot.getLogger().Error("The server has terminated the connection.");
            }
            else if (data.StartsWith("5:::"))
            {
                var jsonObj = JObject.Parse(data.Substring(4));
                var args = JObject.Parse(jsonObj.GetValue("args").First.ToString());
                var method = args.GetValue("method");
                var paramsObject = args.GetValue("params");

                // We don't care about buffer messages - Well, I don't.
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

                        //if (sender.ToLower().Equals(Settings.USERNAME.ToLower())) return;

                        if (message.StartsWith("!") && message.Length > 1)
                        {
                            CommandHandler.Handle(client, sender, rights, message.Trim());
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
                        Bot.getLogger()
                            .Info("Warning! Unknown command from server! Method: " + method + " Params: " + paramsObject);
                        break;
                }
            }
        }

        #region Connection

        public static void SendJoinRequest(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":\"joinChannel\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"name\":\"").Append(Settings.USERNAME.ToLower()).Append("\",");
            sb.Append("\"token\":\"").Append(Settings.AUTH);
            sb.Append("\"}}]}");

            Bot.getLogger().Info(
                "[Client@" + client.GetChannel() + "] Sending join request...");

            client.Send(sb.ToString());
        }

        public static void SendQuitMessage(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":\"partChannel\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":\"").Append(Settings.USERNAME).Append("\"");
            sb.Append("}}]}");

            Bot.getLogger().Info(
                "[Client@" + client.GetChannel() + "] Sending quit message...");
            client.Send(sb.ToString());
        }

        #endregion

        #region General

        public static void SendMessage(Client client, String msg)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":\"chatMsg\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":\"").Append(Settings.USERNAME).Append("\",");
            sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\",");
            sb.Append("\"text\":\"").Append(msg).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendUserInfoRequest(Client client, String user)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"getChannelUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendUserListRequest(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"getChannelUserList\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        #endregion

        #region Giveaway

        // Max Choices is 11
        public static void SendCreateRaffle(Client client, String question, String prize, string subscriber, string follower,
            params String[] choices)
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
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"pauseRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendEndRaffle(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"endRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendStartRaffle(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"startRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendHideRaffle(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"hideRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendCleanupRaffle(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"cleanupRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        // Winner must be a number as a string. Winning answer starts at 0.
        public static void SendPickWinner(Client client, String winner)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"winnerRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"answer\":").Append("\"").Append(winner).Append("\"");
            sb.Append("}}]}");
            client.Send(sb.ToString());
        }

        #endregion

        #region Poll

        // Max choices is 11
        public static void SendCreatePoll(Client client, String question, String subscriber, String follower, params String[] choices)
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
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"pausePoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendStartPoll(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"startPoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendEndPoll(Client client)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"endPoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        #endregion

        #region Moderation

        public static void SendKickUser(Client client, String user, int time)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"kickUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\",");
            sb.Append("\"timeout\":").Append(time);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendBanUser(Client client, String user)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"banUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendUnBanUser(Client client, String user)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"unbanUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendSlowMode(Client client, int time)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"slowMode\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"time\":").Append(time);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public static void SendSubOnlyMode(Client client, String toggle)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"slowMode\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"subscriber\":").Append(toggle);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        #endregion

        #region MOTD

        public static void SendMotd(Client client, String text)
        {
            var sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"motdMsg\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(Settings.USERNAME).Append("\",");
            sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\",");
            sb.Append("\"text\":").Append("\"").Append(text).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        #endregion
    }
}