using Futebox.Services.Interfaces;
using PuppeteerSharp;
using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class BrowserService : IBrowserService 
    {

        protected Browser browser { get; set; }
        protected BrowserState? browserState { get; set; }

        public enum TypingKeyword
        {
            Escape,
        }
        public enum BrowserState
        {
            closed,
            open,
            oldInstanceOpen
        }
        public IEnumerable<TypingKeyword> keywords = new List<TypingKeyword>()
        {
            {TypingKeyword.Escape},
        };

        public BrowserService()
        {
            InitialBrowser();
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

            await page.Keyboard.TypeAsync(text, new PuppeteerSharp.Input.TypeOptions() { Delay = 800 });
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

        protected async Task OpenBrowser(bool headless = false, bool isRetrying = false)
        {
            var userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            if (browser != null) await browser?.CloseAsync();

            try
            {
                browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    ExecutablePath = $"{userProfile}\\AppData\\Local\\Google\\Chrome SxS\\Application\\chrome.exe",
                    Headless = headless,
                    UserDataDir = $"{userProfile}\\AppData\\Local\\Google\\Chrome SxS\\User Data",
                    DefaultViewport = GetDefaultViewPort(),
                    Args = new string[] {
                    $"--user-data-dir={userProfile}\\AppData\\Local\\Google\\Chrome SxS\\User Data\\",
                    $"--profile-directory=\"Profile 1\"",
                    $"--remote-debugging-port=21233",
                }
                });
            } catch (Exception ex)
            {
                browser = await Puppeteer.ConnectAsync(new ConnectOptions() {
                    BrowserURL = "http://127.0.0.1:21233",
                });
            }
            browser.Disconnected += async (Object sender, EventArgs eargs) =>
            {
                await OpenBrowser();
            };
        }

        protected async Task TryAttatch()
        {
            try
            {
                this.browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserURL = "http://127.0.0.1:21233"
                });
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                this.browser = null;
            }
        }

        protected async Task CloseBrowser()
        {
            await this.browser?.CloseAsync();
            this.browser = null;
        }

        public string JsFunction(string fn, params string[] args)
        {
            var script = File.ReadAllText($"{Settings.ApplicationRoot}/js/puppeteer.js");
            var fnArgs = args?.Length > 0 ? args.Select(_ => $"'{_}'") : new string[] { };
            var fnArgsStr = string.Join(",", fnArgs);
            var fnExecutionStr = $"\n\n{fn}({fnArgsStr})";
            return $"{script}{fnExecutionStr}";
        }

        public async Task<Page> NewPage()
        {
            return await browser.NewPageAsync();
        }

        public async Task RedigitarTextoCampo(string seletor, string texto, Page page)
        {
            await page.ClickAsync(seletor);
            WaitForMs(700);

            await page.Keyboard.DownAsync("Control");
            WaitForMs(700);

            await page.Keyboard.PressAsync("A");
            WaitForMs(700);

            await page.Keyboard.UpAsync("Control");
            WaitForMs(700);

            await page.Keyboard.PressAsync("Backspace");
            WaitForMs(700);

            //var textofinal = new List<string>();

            //var caracteres = texto
            //    .ToCharArray()
            //    .Select(s => s.ToString())
            //    .ToList();

            WinClipboard.SetText(texto);

            await page.ClickAsync(seletor);
            await page.Keyboard.DownAsync("Control");
            WaitForMs(700);

            await page.Keyboard.PressAsync("V");
            WaitForMs(700);

            await page.Keyboard.UpAsync("Control");
            WaitForMs(700);

            //caracteres.ForEach(_ =>
            //{
            //    var escape = _ == $"";
            //    if (escape)
            //    {
            //        WaitForMs(300);
            //        page.Keyboard.PressAsync("Escape").Wait();
            //        WaitForMs(300);
            //    }
            //    else page.Keyboard.TypeAsync(_).Wait();
            //    WaitForMs(12);
            //});

            // await page.Keyboard.TypeAsync(texto);

            WaitForMs(700);
        }

        public void Dispose()
        {
            Task.WhenAll(Task.Run(this.CloseBrowser));
        }
    }
}
