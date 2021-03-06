﻿using System;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox
{
    public static class Settings
    {
        // Default Channels
        public static String[] CHANNEL = {"Hitakashi"};

        // Bot Username & Password
        public static String USERNAME = "";
        public static String PASSWORD = "";
        public static String HEXCOLOR = "FF00FF";

        // Don't fill in
        public static String AUTH = "";
        public static String WS_IP = "";

        // Bot Admins, This isn't implemented in this 'example', but you can use this or something external...like a database.
        public static String[] ADMINS = { "CHANGEME" };

        public static Boolean DEBUGGING = true;
        
        // Setting this to true will require admin elevation to add a urlacl for Windows.
        // You MAY have to run as su/sudo in Linux with Mono. Testing that soon.
        public static Boolean APISERVER = false;

        public static class Links
        {
            public static String WS_CON = "ws://" + WS_IP + "/socket.io/1/websocket/";
            public static String WS_AUTH = "http://" + WS_IP + "/socket.io/1/";
        }

        public static class API
        {
            public static String AUTH_TOKEN = "https://www.hitbox.tv/api/auth/token";
            public static String CHAT_SERVERS = "http://www.hitbox.tv/api/chat/servers.json?redis=true";
        }
    }
}
