using WebCrawlerConsole;

Console.WriteLine("Hello, World!");

var htmlParser = new MyHtmlParser();
var webCrawler = new WebCrawler.WebCrawler(htmlParser); 

string startUrl = "http://news.yahoo.com/";
List<string> allowedHostnames = new List<string> { "news.yahoo.com", "news.google.com" }; // Add more allowed hostnames if needed

IList<string> visitedUrls = await webCrawler.Crawl(startUrl, allowedHostnames);

// Process the list of visited URLs (e.g., print them to the console)
foreach (string url in visitedUrls)
{
    Console.WriteLine(url);
}

startUrl = "http://news.google.com/";

visitedUrls = await webCrawler.Crawl(startUrl, allowedHostnames);

// Process the list of visited URLs (e.g., print them to the console)
foreach (string url in visitedUrls)
{
    Console.WriteLine(url);
}