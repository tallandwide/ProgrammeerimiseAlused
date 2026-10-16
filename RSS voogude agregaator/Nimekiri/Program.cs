using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Uudised
{

    public class RssItem
    {
        public string Title { get; }
        public string Link { get; }
        public string? Description { get; }
        public string Source { get; }
        public DateTimeOffset Published { get; }

        public RssItem(string title, string link, string? description, string source, DateTimeOffset published)
        {
            Title = title;
            Link = link;
            Description = description;
            Source = source;
            Published = published;
        }
    }

    public class Program
    {

        static async Task Main(string[] args)
        {

            //Kasutatud uudistevood

            string[] vood =
            {
                "https://www.err.ee/rss",
                "https://www.postimees.ee/rss",
                "https://feeds.feedburner.com/aripaev-rss"
            };



            var andmed = new List<RssItem>();
            using var http = new HttpClient();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("RssAggregator/1.0");

            foreach (string voourl in vood)
            {
                try
                {
                    XDocument doc = XDocument.Parse(await http.GetStringAsync(voourl));
                    XElement channel = doc.Root!.Element("channel")!;
                    string source = channel.Element("title")?.Value ?? voourl;

                    foreach (XElement item in channel.Elements("item"))
                    {
                        string title = item.Element("title")?.Value ?? "";
                        string link = item.Element("link")?.Value ?? "";
                        string description = item.Element("description")?.Value ?? "";
                        description = Regex.Replace(description, "<.*?>", "");   
                        description = WebUtility.HtmlDecode(description).Trim();

                        DateTimeOffset.TryParse(item.Element("pubDate")?.Value, out DateTimeOffset published);

                        
                        andmed.Add(new RssItem(title, link, description, source, published));
                    }

                    Console.WriteLine($"{voourl}: loetud {channel.Elements("item").Count()} uudist");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Viga voos {voourl}: {ex.Message}");
                }
            }


            //20 uudist

            List<RssItem> uusim = andmed
                .OrderByDescending(i => i.Published)
                .Take(20)
                .ToList();



            
            //HTML väljundi loomine

            using (FileStream fs = new FileStream("uudised.html", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(false)))
            {
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html><head><meta charset=\"utf-8\"><title>Uudised</title></head><body>");
                sw.WriteLine("<ul>");
                foreach (RssItem n in uusim)
                {
                    sw.WriteLine("<li>");
                    sw.WriteLine($"<a href=\"{WebUtility.HtmlEncode(n.Link)}\">{WebUtility.HtmlEncode(n.Title)}</a><br />");
                    sw.WriteLine($"{n.Published.ToLocalTime():dd.MM.yyyy HH:mm} | {WebUtility.HtmlEncode(n.Source)}<br />");
                    sw.WriteLine(WebUtility.HtmlEncode(n.Description));
                    sw.WriteLine("</li>");
                }
                sw.WriteLine("</ul>");
                sw.WriteLine("</body></html>");
            }
            Console.WriteLine($"{uusim.Count} uudist salvestatud faili {Path.GetFullPath("uudised.html")}");


        }

    }

    



}