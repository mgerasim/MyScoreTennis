using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreTennisEntity.Helper
{
    static public class Core
    {
        static public void ParserScore(string theHtml)
        {
            

                var doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlNode.ElementsFlags["br"] = HtmlAgilityPack.HtmlElementFlag.Empty;
                doc.LoadHtml(theHtml);

                string xpathTBody = "//ul[@class='ifmenu live-menu']";
                var tagTBody = doc.DocumentNode.SelectSingleNode(xpathTBody);
                if (tagTBody == null)
                {
                    throw new Exception("Не обнаружен tbody");
                }
                
                

            
        }
    }
}
