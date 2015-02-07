using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpHitbox.utils
{
    public class Logger
    {
        public Logger Log(String message) {
        if (Settings.DEBUGGING)
            Console.WriteLine("[" + Date() + "] -> " + message);
        return this;
    }

        public Logger Parent(String message) {
        if (Settings.DEBUGGING)
            Console.WriteLine("\t\t- " + message);
        return this;
    }

        public Logger Info(String message) {
        if (Settings.DEBUGGING)
            Console.WriteLine("[" + Date() + "] -> " + message);
        return this;
    }

        public Logger Error(String message)
        {
            Console.Error.WriteLine("[" + Date() + "] -> " + message);
            return this;
        }

        public Logger Error(Exception message)
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(message, true);

            Console.Error.WriteLine("[" + Date() + "] -> (" + trace.GetFrame(0).GetMethod().ReflectedType.FullName + ") Reason: " + message.Message + " at line: " + trace.GetFrame(0).GetFileLineNumber());
            message.GetBaseException();
            return this;
        }

        public static String Date() {
        try {
            var parsed = DateTime.Now.ToString("h:mm:ss tt");
            return parsed;
        } catch (Exception e) {
            e.GetBaseException();
        }
        return "";
    }
    }
}
