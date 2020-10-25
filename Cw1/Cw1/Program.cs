using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string websiteAddress;

            try
            {
                websiteAddress = args[0];
            }
            catch(Exception)
            {
                throw new ArgumentNullException();
            }

            var websiteAddressRegex = new Regex("https://[a-z.]+");
            MatchCollection matchesWebsiteAddress = websiteAddressRegex.Matches(websiteAddress);

            if(matchesWebsiteAddress.Count > 0)
            {
                var httpClient = new HttpClient();
                HttpResponseMessage response;

                try
                {
                    response = await httpClient.GetAsync(args[0]);

                    if (response.IsSuccessStatusCode)
                    {
                        var html = await response.Content.ReadAsStringAsync();
                        var emailRegex = new Regex("[a-z0-9]+@[a-z.]+");

                        var emailMatches = emailRegex.Matches(html);
                        if (emailMatches.Count > 0)
                        {
                            var uniqueEmailMatches = emailMatches.OfType<Match>().Select(m => m.Value).Distinct();
                            foreach (var i in uniqueEmailMatches)
                            {
                                Console.WriteLine(i);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nie znaleziono adresów email");
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Błąd w czasie pobierania strony");
                }

                httpClient.Dispose();
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
