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
                    if (item.Attributes["id"].Value.Split(new char[] {'_'}, StringSplitOptions.RemoveEmptyEntries)[0] == "x") 
                    {
                        continue;
                    }

                    string ss = item.Attributes["id"].Value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[2];
                    res += ss + ",";

                    String urlAddress = String.Format("http://www.myscore.ru/match/{0}/#point-by-point;1", ss);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;

                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        string data = readStream.ReadToEnd();

                        response.Close();
                        readStream.Close();


                    }


                    //var NextChild = item.Ne
                }

                string matchesAllHtml = tagTBody.ChildNodes[0].InnerHtml;


                
                

                return res;    

            
        }
    }
}
