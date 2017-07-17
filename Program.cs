using System;
using System.Threading.Tasks;

namespace dotnettalk
{
    class Program
    {
        protected const string HelpText = "Usage:\n" +
                                          "  help    - display this reference\n" +
                                          "  status  - display status of the httpd service\n" +
                                          "  restart - restart the httpd service";

        static void Main(string[] args)
        {
            CliAsync().Wait();
        }

        static async Task CliAsync()
        {
            string input = "";
            string serviceName = "httpd.service";
            string helpText = HelpText + "\n  quit    - exit the cli interface";

            Console.WriteLine(helpText);

            while( (input = Console.ReadLine()) != "quit" )
            {
                Console.WriteLine("");
                switch(input)
                {
                    case "help":
                        Console.WriteLine(helpText);
                        break;
                    case "status":
                        Console.WriteLine(await Systemctl.GetServiceStatus(serviceName));
                        break;
                    case "restart":
                        await Systemctl.RestartService(serviceName);
                        Console.WriteLine("Restarting " + serviceName);
                        break;
                    default:
                        Console.WriteLine("Invalid command. Use `help` for help.");
                        break;
                }
                Console.WriteLine("");
            }
        }
    }
}
