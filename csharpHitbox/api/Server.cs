using System;
using System.Net.Sockets;
using System.Text;
using Nancy;
using Nancy.Hosting.Self;

namespace csharpHitbox.api
{
    public static class ApiServer
    {
        private static readonly Uri uri = new Uri("http://localhost:80/");
        private static readonly HostConfiguration config = new HostConfiguration {RewriteLocalhost = true};
        private static readonly NancyHost host = new NancyHost(config, uri);

        public static void Main()
        {
            try
            {
                host.Start();
                if (Settings.DEBUGGING)
                    StaticConfiguration.DisableErrorTraces = false;

                Console.WriteLine("API Server is running.");
            }
            catch (AutomaticUrlReservationCreationFailureException)
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    if (config.RewriteLocalhost)
                    {
                        NetSh.AddUrlAcl("http://+:" + uri.Port + "/", "Everyone");
                    }
                    else
                    {
                        NetSh.AddUrlAcl(uri.OriginalString, "Everyone");
                    }
                    host.Start();
                    Console.WriteLine("API Server is running.");
                }
            }
            catch (SocketException)
            {
                // Need to capture a SocketException for Linux/MacOSX if the program isn't ran with sudo
                Console.WriteLine("Linux/OSX: Program must be ran with mono due to using port 80.");
            }
        }

        public static Response NotFound()
        {
            var jsonNotFound = Encoding.UTF8.GetBytes("\"status\":\"Channel Not Found\"");
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                Contents = s => s.Write(jsonNotFound, 0, jsonNotFound.Length)
            };
        }
    }
}