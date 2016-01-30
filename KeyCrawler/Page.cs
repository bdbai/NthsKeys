using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace NthsKeys.KeyCrawler
{
    public abstract class Page : IPage
    {
        public virtual string Url { get; }

        public virtual XPathNavigator GetNavigator()
        {
            HtmlWeb web = new HtmlWeb();
            var document = web.Load(Url);
            return document.CreateNavigator();
        }

        public virtual XPathNodeIterator SelectNodes(string XPath)
        {
            return GetNavigator().Select(XPath);
        }
    }
}
