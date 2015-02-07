using System;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.commands
{
    public class CommandData
    {
        private String data;
        private int rights;

        public CommandData(String data, int rights)
        {
            this.data = data;
            this.rights = rights;
        }

        public String getData()
        {
            return this.data;
        }

        public int getRights()
        {
            return this.rights;
        }
    }
}