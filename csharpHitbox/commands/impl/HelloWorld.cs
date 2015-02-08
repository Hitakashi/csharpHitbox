using csharpHitbox.client;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.commands.impl
{
    public class HelloWorld : Command
    {
        public override void Execute(Client client, int rights, string sender, string @params)
        {
            if (rights < Rights.USER) return;
            MessageHandler.SendMessage(client, "Hello World!");
        }
    }
}