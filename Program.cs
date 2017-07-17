using System;
using System.Threading.Tasks;

namespace dotnettalk
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hai!");
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            Console.WriteLine("httpd status: ");
            Console.WriteLine(await Systemctl.GetServiceStatus("httpd.service"));
            await Systemctl.RestartService("httpd.service");
            Console.WriteLine(await Systemctl.GetServiceStatus("httpd.service"));

            await Task.Run(() => {
                Console.WriteLine("Press any key to close the application.");
                Console.Read();
            });
        }
    }
}
