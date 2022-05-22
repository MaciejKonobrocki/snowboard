using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;

namespace snowboard
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                
                var html = await client.GetStringAsync("https://snowboardowy.pl/62-buty-snowboardowe-meskie");
                
                var config = Configuration.Default;
                using var context = BrowsingContext.New(config);
                using var doc = await context.OpenAsync(req => req.Content(html));

                var list = doc.QuerySelector("#js-product-list div")
                .Children
                .Select(x => 
                (
                x.QuerySelector(".product-title a").InnerHtml, 
                x.QuerySelector(".regular-price")?.InnerHtml.Replace("&nbsp;", "").Replace("zł",""),
                x.QuerySelector(".price")?.InnerHtml.Replace("&nbsp;", "").Replace("zł","")
                )
                );


                Console.WriteLine(string.Join("\r\n", list));                
            }
        }
    }
}
