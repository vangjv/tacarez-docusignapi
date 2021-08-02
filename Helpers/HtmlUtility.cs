using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace TacarEZDocusignAPI
{
    class HtmlUtility
    {
        public static string LoadHtmlFile(string htmlPath)
        {
            var doc = new HtmlDocument();
            doc.Load(htmlPath);
            return doc.Text;
        }
    }
}
