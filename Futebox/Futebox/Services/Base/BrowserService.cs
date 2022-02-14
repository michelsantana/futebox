﻿using Futebox.Services.Interfaces;
using PuppeteerSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class BrowserService : IBrowserService
    {

        protected Browser _browser;

        public BrowserService()
        {
            InitialBrowser();
        }

        ~BrowserService()
        {
            Dispose();
        }

        private void InitialBrowser()
        {
            Task.WaitAll(Task.Run(() => OpenBrowser()));
        }

        protected async Task ClearInputThenWriteText(Page page, string selector, string text)
        {
            await page.ClickAsync(selector);
            WaitForMs(344);
            await page.Keyboard.DownAsync("ControlLeft");
            WaitForMs(344);
            await page.Keyboard.PressAsync("A");
            WaitForMs(344);
            await page.Keyboard.UpAsync("ControlLeft");
            WaitForMs(344);

            await page.Keyboard.DownAsync("Backspace");
            WaitForMs(344);
            await page.Keyboard.UpAsync("Backspace");
            WaitForMs(344);

            await page.Keyboard.TypeAsync(text, new PuppeteerSharp.Input.TypeOptions() { Delay = 138 });
        }

        public void WaitFor(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        public void WaitForMs(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        protected ViewPortOptions GetDefaultViewPort()
        {
            return new ViewPortOptions() { Width = 1080, Height = 1080 };
        }

        protected async Task<Browser> OpenBrowser(bool headless = false)
        {
            if (this._browser == null) await TryAttatch();
            if (this._browser == null)
            {
                var userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
                this._browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    ExecutablePath = $"{userProfile}\\AppData\\Local\\Google\\Chrome SxS\\Application\\chrome.exe",
                    Headless = headless,
                    UserDataDir = $"{userProfile}\\AppData\\Local\\Google\\Chrome SxS\\User Data",
                    DefaultViewport = GetDefaultViewPort(),
                    Args = new string[] {
                    $"--user-data-dir={userProfile}\\AppData\\Local\\Google\\Chrome SxS\\User Data\\",
                    $"--profile-directory=\"Profile 1\"",
                    $"--remote-debugging-port=21223",
                    //$"--window-position=0,1080",
                }
                });
            }

            return this._browser;
        }

        protected async Task TryAttatch()
        {
            try
            {
                this._browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserURL = "http://127.0.0.1:21223"
                });
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                this._browser = null;
            }
        }

        protected async Task CloseBrowser()
        {
            await this._browser?.CloseAsync();
            this._browser = null;
        }

        public string JsFunction(string fn, params string[] args)
        {
            var script = File.ReadAllText("js/puppeteer.js");
            var fnArgs = args?.Length > 0 ? args.Select(_ => $"'{_}'") : new string[] { };
            var fnArgsStr = string.Join(",", fnArgs);
            var fnExecutionStr = $"\n\n{fn}({fnArgsStr})";
            return $"{script}{fnExecutionStr}";
        }

        public async Task<Page> NewPage()
        {
            return await _browser.NewPageAsync();
        }

        public void Dispose()
        {
            Task.WhenAll(Task.Run(this.CloseBrowser));
        }
    }
}