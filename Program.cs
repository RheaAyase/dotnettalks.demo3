using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace dotnettalk
{
    class Program
    {
        protected const string ServiceName = "httpd.service";
        protected const string HelpText = "Usage:\n" +
                                          "  help    - display this reference\n" +
                                          "  status  - display status of the httpd service\n" +
                                          "  restart - restart the httpd service";

        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<WebStartup>()
                .UseUrls("http://localhost:5000")
                .Build();
            host.RunAsync().ContinueWith(t => PrintException(t.Exception));

            CliAsync().Wait();
        }

        public class WebStartup
        {
            public void Configure(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    switch( context.Request.Path )
                    {
                        case "/status":
                            await context.Response.WriteAsync(await Systemctl.GetServiceStatus(ServiceName));
                            break;
                        case "/restart":
                            await context.Response.WriteAsync("Restarting " + ServiceName);
                            await Systemctl.RestartService(ServiceName);
                            break;
                        default:
                            await context.Response.WriteAsync(HelpText);
                            break;

                    }
                });
            }
        }

        static async Task CliAsync()
        {
            await Task.Delay(300);
            string input = "";
            string helpText = HelpText + "\n  quit    - exit the cli interface";

            Console.WriteLine(helpText);
            Console.WriteLine("");

            while( (input = Console.ReadLine()) != "quit" )
            {
                Console.WriteLine("");
                switch(input)
                {
                    case "help":
                        Console.WriteLine(helpText);
                        break;
                    case "status":
                        Console.WriteLine(await Systemctl.GetServiceStatus(ServiceName));
                        break;
                    case "restart":
                        await Systemctl.RestartService(ServiceName);
                        Console.WriteLine("Restarting " + ServiceName);
                        break;
                    default:
                        Console.WriteLine("Invalid command. Use `help` for help.");
                        break;
                }
                Console.WriteLine("");
            }
        }

        protected static void PrintException(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}
