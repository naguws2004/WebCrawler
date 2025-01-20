using System.Collections.Concurrent;

namespace WebCrawler
{
    public class WebCrawler
    {
        private readonly HtmlParser _htmlParser;
        private readonly ConcurrentBag<string> _visitedUrls = new ConcurrentBag<string>();
        private readonly HashSet<string> _allowedHostnames = new HashSet<string>();

        public WebCrawler(HtmlParser htmlParser)
        {
            _htmlParser = htmlParser;
        }

        public async Task<IList<string>> Crawl(string startUrl, IEnumerable<string> allowedHostnames)
        {
            _allowedHostnames.UnionWith(allowedHostnames);

            if (!_allowedHostnames.Contains(GetHostname(startUrl)))
            {
                throw new ArgumentException($"Hostname '{GetHostname(startUrl)}' is not allowed.");
            }

            await CrawlAsync(startUrl);

            return _visitedUrls.ToList();
        }

        private async Task CrawlAsync(string url)
        {
            if (!_visitedUrls.Contains(url))
            {
                _visitedUrls.Add(url);
                var tasks = _htmlParser.GetUrls(url)
                    .Where(u => _allowedHostnames.Contains(GetHostname(u)))
                    .Select(u => CrawlAsync(u));

                await Task.WhenAll(tasks);
            }
        }

        private static string GetHostname(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Host;
            }
            catch (UriFormatException)
            {
                return string.Empty; // Handle invalid URL format
            }
        }
    }

    public interface HtmlParser
    {
        IEnumerable<string> GetUrls(string url);
    }
}
