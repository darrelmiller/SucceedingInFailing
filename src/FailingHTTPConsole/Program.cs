using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FailingHTTPApi;
using Microsoft.Owin.Hosting;

namespace FailingHTTPConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostUrl = "http://127.0.0.1:5001";
            Console.WriteLine("Listening on {0}",hostUrl);

            using (WebApp.Start<Startup>(hostUrl))
            {
                Console.ReadLine();
            }

        }
    }
}
