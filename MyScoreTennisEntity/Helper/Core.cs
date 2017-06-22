using HtmlAgilityPack;
using MyScoreTennisEntity.Models;
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

        static public string ParserScoreMatch(string theHtml, Sethistory theSet)
        {
            string res = "";

            var doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNode.ElementsFlags["br"] = HtmlAgilityPack.HtmlElementFlag.Empty;
            doc.LoadHtml(theHtml);

            string xpathTable = "//table[@class='parts-first']";
            var tagTable = doc.DocumentNode.SelectSingleNode(xpathTable);
            if (tagTable == null)
            {
                throw new Exception("Не обнаружен table");
            }

            var tagTBody = tagTable.ChildNodes[0];

            for (int i = 1; i < tagTBody.ChildNodes.Count; i++)
            {
                var tagMatchHistoryScore = tagTBody.ChildNodes[i].ChildNodes[2];

                var tagHighlightLeft  = tagMatchHistoryScore.ChildNodes[0];
                var tagHighlightRight = tagMatchHistoryScore.ChildNodes[2];

                string HighlightLeft = tagHighlightLeft.InnerText;
                string HighlightRight = tagHighlightRight.InnerText;

                int Highlight = 0;

                if (tagHighlightLeft.HasAttributes) {
                    if (tagHighlightLeft.Attributes["class"].Value == "score-highlight") 
                    {
                        Highlight = 1;
                    }
                }

                if (tagHighlightRight.HasAttributes) 
                {
                    if (tagHighlightRight.Attributes["class"].Value == "score-highlight")
                    {
                        Highlight = 2;
                    }
                }

                if (Highlight == 0) 
                {
                    throw new Exception("Не определён гол");
                }
                                
                Score theScore = new Score();
                theScore.HighlightLeft = Convert.ToInt32(HighlightLeft);
                theScore.HighlightRight = Convert.ToInt32(HighlightRight);
                theScore.Highlight = Highlight;
                theScore.Set = theSet;

                theScore.Save();
                i++;
            }

                return res;
        }
        static public string ParserScore(string theHtml)
        {
                string res = "";

                var doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlNode.ElementsFlags["br"] = HtmlAgilityPack.HtmlElementFlag.Empty;
                doc.LoadHtml(theHtml);

                string xpathTBody = "//div[@class='table-main']";
                var tagTBody = doc.DocumentNode.SelectSingleNode(xpathTBody);
                if (tagTBody == null)
                {
                    throw new Exception("Не обнаружен tbody");
                }

                var allElementsWithClassTennis =
                    doc.DocumentNode.SelectNodes("//tr[contains(@class,'stage-finished')]");

                foreach(var item in allElementsWithClassTennis) 
                {
                    if (item.ChildNodes[2].InnerText != "Завершен")
                    {
                        continue;
                    }

                    if (item.Attributes["id"].Value.Split(new char[] {'_'}, StringSplitOptions.RemoveEmptyEntries)[0] == "x") 
                    {
                        continue;
                    }

                    string ss = item.Attributes["id"].Value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[2];
                    res += ss + ",";

                    MyScoreTennisEntity.Models.Match theMatch = Models.Match.GetByNumber(ss);

                    if (theMatch == null)
                    {
                        theMatch = new Models.Match();
                        
                        theMatch.Number = ss;
                        theMatch.Status = 1;

                        theMatch.Save();
                    }


                    //String urlAddress = String.Format("http://www.myscore.ru/match/{0}/#point-by-point;1", ss);

                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //if (response.StatusCode == HttpStatusCode.OK)
                    //{
                    //    Stream receiveStream = response.GetResponseStream();
                    //    StreamReader readStream = null;

                    //    if (response.CharacterSet == null)
                    //    {
                    //        readStream = new StreamReader(receiveStream);
                    //    }
                    //    else
                    //    {
                    //        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    //    }

                    //    string data = readStream.ReadToEnd();

                    //    response.Close();
                    //    readStream.Close();
                    //}


                    //var NextChild = item.Ne
                }

                string matchesAllHtml = tagTBody.ChildNodes[0].InnerHtml;
                return res;                
        }
    }
}
