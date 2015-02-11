using System;

/**
 * Originally created by BitOBytes in Java
 * Converted to C# by Hitakashi
 * Version: 0.1
 */

namespace csharpHitbox.utils
{
    public class Logger
    {
         public static void Log(String message) {
        if (Settings.DEBUGGING)
            Console.WriteLine("[" + Date() + "] -> " + message);
    }

        public static void Parent(String message) {
        if (Settings.DEBUGGING)
            Console.WriteLine("\t\t- " + message);
    }

        public static void Info(String message) {
        if (Settings.DEBUGGING)
            Console.WriteLine("[" + Date() + "] -> " + message);
    }

        public static void Error(String message)
        {
            Console.Error.WriteLine("[" + Date() + "] -> " + message);
        }

        public static void Error(Exception message)
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(message, true);

            Console.Error.WriteLine("[" + Date() + "] -> (" + trace.GetFrame(0).GetMethod().ReflectedType.FullName + ") Reason: " + message.Message + " at line: " + trace.GetFrame(0).GetFileLineNumber());
            message.GetBaseException();
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
