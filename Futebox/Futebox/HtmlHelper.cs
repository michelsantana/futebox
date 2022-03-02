using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox
{
    public static class HtmlHelper
    {
        static Dictionary<string, Tuple<DateTime, string>> stylesContents = new Dictionary<string, Tuple<DateTime, string>>();

        public static IHtmlContent ScopeStyle(this IHtmlHelper _, string scope, string style)
        {
            var exists = stylesContents.ContainsKey(style);
            var stylePath = Path.Combine(Settings.ApplicationsRoot, "wwwroot", "css", style);

            if (!exists && File.Exists(stylePath)) stylesContents.Add(style, Tuple.Create(DateTime.Now.AddSeconds(20), File.ReadAllText(stylePath)));
            var styleContent = stylesContents[style];
            if(styleContent.Item1 < DateTime.Now)
            {
                stylesContents.Remove(style);
                return ScopeStyle(_, scope, style);
            }

            return _.Raw($"<style>{styleContent.Item2.Replace("#scope", scope)}</style>");
        }
    }
}
