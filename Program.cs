using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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
            CliAsync();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<WebStartup>()
                .UseUrls("http://localhost:5000")
                .Build();
            host.Run();
        }

        public class WebStartup
        {
            public void Configure(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    string serviceName = "httpd.service";

                    switch( context.Request.Path )
                    {
                        case "/status":
                            await context.Response.WriteAsync(await Systemctl.GetServiceStatus(serviceName));
                            break;
                        case "/restart":
                            await context.Response.WriteAsync("Restarting " + serviceName);
                            await Systemctl.RestartService(serviceName);
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
            try
            {
                await Task.Delay(300);
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
