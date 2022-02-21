using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IBrowserService
    {
        Task<Page> NewPage();
        void WaitFor(int seconds);
        void WaitForMs(int milliseconds);
        string JsFunction(string fn, params string[] args);
        Task RedigitarTextoCampo(string seletor, string texto, Page page);
    }
}
