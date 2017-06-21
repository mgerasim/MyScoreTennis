﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyScoreTennis
{
    public partial class FormMain : Form
    {
        ulong CountStarted = 0;
        WebBrowser webBrowser = new WebBrowser();
        public FormMain()
        {
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.ObjectForScripting = true;
            SetFeatureBrowserEmulation();
            InitializeComponent();
        }      

        // navigate and download 
        async Task<string> LoadDynamicPage(string url, CancellationToken token)
        {
            // navigate and await DocumentCompleted
            var tcs = new TaskCompletionSource<bool>();
            WebBrowserDocumentCompletedEventHandler handler = (s, arg) =>
                tcs.TrySetResult(true);

            using (token.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: true))
            {
                this.webBrowser.DocumentCompleted += handler;
                try
                {
                    this.webBrowser.Navigate(url);
                    await tcs.Task; // wait for DocumentCompleted
                }
                finally
                {
                    this.webBrowser.DocumentCompleted -= handler;
                }
            }

            // get the root element
            var documentElement = this.webBrowser.Document.GetElementsByTagName("html")[0];

            // poll the current HTML for changes asynchronosly
            var html = documentElement.OuterHtml;
            while (true)
            {
                // wait asynchronously, this will throw if cancellation requested
                await Task.Delay(500, token);

                // continue polling if the WebBrowser is still busy
                if (this.webBrowser.IsBusy)
                    continue;

                var htmlNow = documentElement.OuterHtml;
                if (html == htmlNow)
                    break; // no changes detected, end the poll loop

                html = htmlNow;
            }

            // consider the page fully rendered 
            token.ThrowIfCancellationRequested();
            return html;
        }

        // enable HTML5 (assuming we're running IE10+)
        // more info: https://stackoverflow.com/a/18333982/1768303
        static void SetFeatureBrowserEmulation()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                appName, 10000, RegistryValueKind.DWord);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.label2.Text = "";
            Entity.Common.NHibernateHelper.UpdateSchema();
            this.buttonStop.Enabled = false;
        }

        async private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timer1.Stop();
                this.CountStarted++;
                this.label2.Text = this.CountStarted.ToString();

                var cts = new CancellationTokenSource(10000); // cancel in 10s
                var html = await LoadDynamicPage("http://www.myscore.ru/tennis/", cts.Token);

                this.textBox1.Text = html;
                MyScoreTennisEntity.Helper.Core.ParserScore(html);

                foreach(var match in MyScoreTennisEntity.Models.Match.GetAllByStatus(1))
                {
                    var ctsMatch = new CancellationTokenSource(10000); // cancel in 10s
                    string urlMatch = String.Format("http://www.myscore.ru/match/{0}/#point-by-point;1", match.Number);
                    var htmlMatch = await LoadDynamicPage(urlMatch, ctsMatch.Token);

                    MyScoreTennisEntity.Helper.Core.ParserScoreMatch(htmlMatch);
                }

            }
            catch (Exception ex)
            {
                this.textBox1.Text = ex.Message;
            }
            finally
            {
                this.timer1.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.buttonStop.Enabled = false;
            this.buttonStart.Enabled = true;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
            this.buttonStop.Enabled = true;
            this.buttonStart.Enabled = false;
        }

    }
}
