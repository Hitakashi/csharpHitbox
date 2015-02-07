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

namespace csharpHitbox
{
    public class MessageHandler
    {
        private Client client;
        private const String BaseMessage = "5:::{\"name\":\"message\",\"args\":[{";

        public MessageHandler(Client client)
        {
            this.client = client;
        }

        public void handle(String data)
        {
            if (data.Equals("2::"))
            {
                client.Send(data);
            }
            else if (data.Equals("0::"))
            {
                // We've been kicked from the server.
                Bot.Destroy();
                Console.WriteLine("The server has terminated the connection.");
            }
            else if (data.StartsWith("5:::"))
            {
                var jsonObj = JObject.Parse(data.Substring(4));
                var args = JObject.Parse(jsonObj.GetValue("args").First.ToString());
                var method = args.GetValue("method");
                var paramsObject = args.GetValue("params");

                switch (method.ToString())
                {
                    case "loginMsg":
                        // Successfully authenticated
                        break;
                    case "chatMsg":
                        if (paramsObject.SelectToken("buffer") != null) return;
                        Console.WriteLine(paramsObject);
                        break;
                    case "infoMsg":
                        break;
                    case "chatLog":
                        break;
                    #region List of other methods sent from the server
                    //case "userList":
                    //    break;
                    //case "userInfo":
                    //    break;
                                            
                    //case "slowMsg":
                    //    break;

                    // // Raffle
                    //case "raffleMsg":
                    //    break;
                    //case "winnerRaffle":
                    //    break;
                    //// Poll
                    //case "pollMsg":
                    //    break;
                    //// MOTD
                    //case "motdMsg":
                    //    break;

                    //// Server Msg
                    //case "serverMsg":
                    //    // Title or Game changed or host mode changed.
                    //    break;
                    #endregion
                    default:
                        Console.WriteLine(paramsObject);
                        break;
                }
            }
        }

        #region Connection
        public void SendJoinRequest()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":\"joinChannel\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"name\":\"").Append(Settings.USERNAME.ToLower()).Append("\",");
            sb.Append("\"token\":\"").Append(Settings.AUTH);
            sb.Append("\"}}]}");

            Console.WriteLine(
                "[Client@" + client.GetChannel() + "] Sending join request...");
            client.Send(sb.ToString());
        }

        public void SendQuitMessage()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":\"partChannel\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":\"").Append(Settings.USERNAME).Append("\"");
            sb.Append("}}]}");

            Console.WriteLine(
                "[Client@" + client.GetChannel() + "] Sending quit message...");
            client.Send(sb.ToString());
        }
        #endregion

        #region General
        public void SendMessage(String msg)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":\"chatMsg\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":\"").Append(Settings.USERNAME).Append("\",");
            sb.Append("\"nameColor\":").Append("\"").Append(Settings.HEXCOLOR).Append("\",");
            sb.Append("\"text\":\"").Append(msg).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendUserInfoRequest(String user)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"getChannelUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendUserListRequest()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"getChannelUserList\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }
        #endregion 

        #region Giveaway

        // Max Choices is 11
        public void SendCreateRaffle(String question, String prize, string subscriber, string follower, params String[] choices)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"createRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"question\":").Append("\"").Append(question).Append("\",");
            sb.Append("\"prize\":").Append("\"").Append(prize).Append("\",");
            sb.Append("\"choices\":[");
            String last = choices.Last();
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

        public void SendPauseRaffle()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"pauseRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }
        
        public void SendEndRaffle()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"endRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendStartRaffle()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"startRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendHideRaffle()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"hideRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendCleanupRaffle()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"cleanupRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        // Winner must be a number as a string. Winning answer starts at 0.
        public void SendPickWinner(String winner)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"winnerRaffle\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"answer\":").Append("\"").Append(winner).Append("\"");
            sb.Append("}}]}");
            client.Send(sb.ToString());
        }


        #endregion

        #region Poll

        public void SendCreatePoll(String question, String subscriber, String follower, params String[] choices)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"createPoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"question\":").Append("\"").Append(question).Append("\",");
            sb.Append("\"choices\":[");
            String last = choices.Last();
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

        public void SendPausePoll()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"pausePoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendStartPoll()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"startPoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendEndPoll()
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"endPoll\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel().ToLower()).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        #endregion

        #region Moderation

        public void SendKickUser(String user, int time)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"kickUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\",");
            sb.Append("\"timeout\":").Append(time);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendBanUser(String user)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"banUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }
        public void SendUnBanUser(String user)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"unbanUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"name\":").Append("\"").Append(user).Append("\",");
            sb.Append("\"token\":").Append("\"").Append(Settings.AUTH).Append("\"");
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendSlowMode(int time)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"unbanUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"time\":").Append(time);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }

        public void SendSubOnlyMode(String toggle)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
            sb.Append("\"method\":").Append("\"unbanUser\",");
            sb.Append("\"params\":{");
            sb.Append("\"channel\":").Append("\"").Append(client.GetChannel()).Append("\",");
            sb.Append("\"subscriber\":").Append(toggle);
            sb.Append("}}]}");

            client.Send(sb.ToString());
        }
        #endregion

        #region MOTD

        public void SendMotd(String text)
        {
            StringBuilder sb = new StringBuilder(BaseMessage);
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