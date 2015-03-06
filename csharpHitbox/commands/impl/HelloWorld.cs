using csharpHitbox.client;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.commands.impl
{
    public static class HelloWorld
    {
        public static void Execute(Client client, int rights, string sender, string @params, bool sub, bool follow)
        {
            if (rights < Rights.User) return;
            MessageHandler.SendMessage(client, "Hello World! Subscriber? " + sub + " Follower? " + follow);
        }
    }
}