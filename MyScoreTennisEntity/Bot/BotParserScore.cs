using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyScoreTennisEntity.Bot
{
    public class BotParserScore
    {
        private bool completed;
        public BotParserScore()
        {
            completed = false;
        }

        public void Start()
        {

            Uri uri = new Uri("http://www.myscore.ru/tennis/");

            runBrowserThread(uri);
        }
        
        private void runBrowserThread(Uri url)
        {
            var th = new Thread(() =>
            {
                var br = new WebBrowser();
                br.DocumentCompleted += browser_StartCompleted;
                br.ScriptErrorsSuppressed = true;
                br.Navigate(url);
                Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        void browser_StartCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var br = sender as WebBrowser;
            if (br.Url == e.Url)
            {
                //Log(e.Url.ToString());
                //Log(br.Document.Body.OuterText);
                try
                {
                    Application.ExitThread();
                    // Application.ExitThread();   // Stops the thread

                }
                catch (Exception ex)
                {

                }

            }
            completed = true;
        }

        void browser_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var br = sender as WebBrowser;
            if (br.Url == e.Url)
            {
                //BorshchForecastRiver(br.Document.Body.OuterText);
                
            }
        }
    }
}
