using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace scraper
{
    class Program
    {
        private static string npckUrl = "https://www.npck.org";
        static void Main(string[] args)
        {
            var str = ScrapeWebsite(npckUrl, "marquee").GetAwaiter().GetResult();
            var priceDict = ExtractPrices(str);
            Console.WriteLine($"Scraped string: {str}");
            foreach (KeyValuePair<string, string> item in priceDict)
            {
                Console.WriteLine(item.Key + "\t=> " + item.Value);
            }
        }
        static async Task<string> ScrapeWebsite(string url, string nodeName)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(url);
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);
            IElement node = document.QuerySelector(nodeName);
            return node.Text();
        }

        static Dictionary<string, string> ExtractPrices(string str)
        {
            // Extracts the county prices from the provided string
            var prices = str.Split(">>>")[1];
            var countyPrices = prices.Split("|");
            Dictionary<string, string> priceDict = new Dictionary<string, string>();
            foreach (String s in countyPrices)
            {
                var countyPrice = s.Trim().Split("Ksh.", StringSplitOptions.RemoveEmptyEntries);
                if (countyPrice.Length > 1)
                {
                    priceDict[countyPrice[0].Trim()] = countyPrice[1].Trim();
                }
            }
            return priceDict;

        }
    }
}