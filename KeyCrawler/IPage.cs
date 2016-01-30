using System.Xml.XPath;

namespace NthsKeys.KeyCrawler
{
    public interface IPage
    {
        string Url { get; }

        XPathNavigator GetNavigator();

        XPathNodeIterator SelectNodes(string XPath);
    }
}
