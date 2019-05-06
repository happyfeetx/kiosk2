#region USING DIRECTIVES

using System;
using System.Reflection;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Kiosk.Core.Common;

#endregion USING DIRECTIVES

namespace Kiosk.Core
{
    internal static class Kiosk
    {
        public static string ApplicationName { get; } = "Kiosk Core";
        public static string ApplicationVersion { get; } = "v2.0.0";
        public static string ApplicationAuthor { get; } = "Wonderfull Group";
        public static int ApplicationRevision { get; } = 2;

        public static SharedData Shared { get; set; }

        private static async Task Main(string[] args)
        {
            try
            {
                PrintBuildInformation();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.SystemDefault;

                await Startup.RunAsync();

                try
                {
                    await Task.Delay(Timeout.Infinite, Shared.MainLoopCts.Token);
                } catch (TaskCanceledException)
                {
                    Console.WriteLine("\rShutdown signal received!          ");
                }
            } catch (Exception e)
            {
                Console.WriteLine($"{e.GetType()} :\n{e.Message}            ");
                if (!(e.InnerException is null))
                {
                    Console.WriteLine($"{e.GetType()} :\n{e.InnerException.Message}");
                }
            }

            
            Console.WriteLine("\rShutting down!                             ");
            Console.ReadKey();
        }

        private static void PrintBuildInformation()
        {
            var a = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(a.Location);

            Console.WriteLine($"{ApplicationName} {ApplicationVersion} [{ApplicationRevision}]\n- {ApplicationAuthor}");
            Console.WriteLine();
        }
    }
}
