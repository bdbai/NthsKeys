using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace NthsKeys.KeyCrawler
{
    public class Bulletin : Page
    {
        internal Bulletin(string _Title, string _Url, DateTime _Date)
        {
            Title = _Title;
            Url = _Url;
            Date = _Date;
        }

        public override string Url { get; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<Attachment> GetAttachments()
        {
            // /html/body/table[2]/tbody/tr/td/table[5]/tbody/tr[2]/td/div/p[1]/a
            // /html/body/table[2]/tbody/tr/td/table[5]/tbody/tr[2]/td/div/a
            foreach (HtmlNodeNavigator i in SelectNodes("/html/body/table[2]/tr/td/table[5]/tr[2]/td/div//a[@href]"))
            {
                Attachment attachment = new Attachment();
                attachment.Name = i.Value;
                attachment.Url = Properties.Settings.Default.urlPrefix + i.GetAttribute("href", string.Empty);
                yield return attachment;
            }
        }

    }
}
