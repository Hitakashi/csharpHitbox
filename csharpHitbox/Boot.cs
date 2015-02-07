using System;
using System.Threading;
using csharpHitbox.bot;
using csharpHitbox.utils;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox
{
    static class Boot
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };
            if (Utils.CheckSettings())
            {
                Bot.GetInstance();

            }
            _quitEvent.WaitOne();
        }
    }
}
