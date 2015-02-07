using System;
using System.Diagnostics;
using csharpHitbox.utils;
using WebSocket4Net;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.client
{
    public class Client
    {
        private String channel = null;
        private WebSocket _webSocket;
        private MessageHandler messageHandler;

        public Client(string channel)
        {
            _webSocket = new WebSocket(Settings.Links.WS_CON + Utils.FetchWsAuth());
            this.channel = channel;
            _webSocket.Opened += _webSocket_Opened;
            _webSocket.MessageReceived += _webSocket_MessageReceived;
            _webSocket.Error += _webSocket_Error;
            _webSocket.Closed += _webSocket_Closed;
            messageHandler = new MessageHandler(this);
        }

        void _webSocket_Opened(object sender, EventArgs e)
        {
            Debug.WriteLine("Opened");
            messageHandler.SendJoinRequest();
        }

        void _webSocket_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine("Closed: " + e);
        }

        void _webSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            messageHandler.handle(e.Message);
        }

        void _webSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.WriteLine("Error: " + e.Exception);
        }

        public void Send(String data)
        {
            _webSocket.Send(data);
            Console.WriteLine("[Send@" + channel + "] " + data);
        }

        public String GetChannel()
        {
            return channel;
        }

        public MessageHandler GetMessageHandler()
        {
            return messageHandler;
        }

        public void Connect()
        {
            _webSocket.Open();
            _webSocket.EnableAutoSendPing = false;
        }

        public void Close()
        {
            _webSocket.Close();
        }
    }
}