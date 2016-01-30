using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace NthsKeys.KeyCrawler
{
    public class CategoryPage : Page
    {
        public override string Url
        {
            get
            {
                return Properties.Settings.Default.categoryUrl;
            }
        }

        public int GetRecordCount()
        {
            HtmlNodeNavigator node = (HtmlNodeNavigator)SelectNodes("/html/body/table[2]/tr[2]/td[2]/table[2]/tr[2]/td/table[2]/tr/td/table/tr/td/text()").Current;
            Regex regex = new Regex("共有(\\d+)条记录");
            string match = string.Empty;
            try
            {
                match = regex.Match(node.Value).Groups[1].Value;
            }
            catch (Exception ex)
            {
                throw new XPathException("Record text not found.", ex);
            }
            return int.Parse(match);
        }

        public IEnumerable<Bulletin> GetBulletins()
        {
            foreach (HtmlNodeNavigator i in SelectNodes("/html/body/table[2]/tr[2]/td[2]/table[2]/tr[2]/td/table[1]/tr"))
            {
                var textNode = i.SelectSingleNode("td[2]/a");
                string title = textNode.Value;
                string url = Properties.Settings.Default.urlPrefix + textNode.GetAttribute("href", string.Empty).ToString();
                var dateNode = i.SelectSingleNode("td[3]/text()");
                string date = dateNode.Value.Replace("[", "").Replace("]", "");
                yield return new Bulletin(title, url, DateTime.Parse(date));
            }
        }
    }
}
