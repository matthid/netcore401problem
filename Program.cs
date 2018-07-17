using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace testtfs401
{
    class EmptyProxy : IWebProxy {
        public ICredentials Credentials { get { return null; } set { } }
        public Uri GetProxy (Uri uri) => null;
        public bool IsBypassed (Uri host) => true;
    }

    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length <= 0){
                Console.Error.WriteLine("Usage: program <url>");
                return 1;
            }
            
            var handler = new HttpClientHandler();
            handler.UseProxy = true;
            var proxy = new EmptyProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials; // note: does nothing
            handler.Proxy = proxy;
            var client = new HttpClient(handler);
            handler.UseProxy = true;
            handler.UseDefaultCredentials = true;

            var result = client.GetAsync(args[0]).Result;
            if (!result.IsSuccessStatusCode) {
                Console.Error.WriteLine("Retrieved Status Code: " + result.StatusCode);
                return 1;
            }
            var str = result.Content.ReadAsStringAsync().Result;
            File.WriteAllText("out.txt", str);
            return 0;
        }
    }
}
