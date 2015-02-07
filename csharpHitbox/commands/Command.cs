using System;
using csharpHitbox.client;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.commands
{
    public abstract class Command
    {
        public abstract void Execute(Client client, int rights, String sender, String @params);

        public static class Rights
        {
            public static int USER = 0, MOD = 1, ADMIN = 2;
        }
    }
}