using Futebox.Models;
using Futebox.Models.Enums;
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
            var stylePath = Path.Combine(Settings.ApplicationRoot, "wwwroot", "css", style);

            if (!exists && File.Exists(stylePath)) stylesContents.Add(style, Tuple.Create(DateTime.Now.AddSeconds(20), File.ReadAllText(stylePath)));
            var styleContent = stylesContents[style];
            if(styleContent.Item1 < DateTime.Now)
            {
                stylesContents.Remove(style);
                return ScopeStyle(_, scope, style);
            }

            return _.Raw($"<style>{styleContent.Item2.Replace("#scope", scope)}</style>");
        }

        public static IHtmlContent SocialBadge(this IHtmlHelper _, int s)
        {
            return SocialBadge(_, (RedeSocialFinalidade)s);
        }
        public static IHtmlContent SocialBadge(this IHtmlHelper _, RedeSocialFinalidade s)
        {
            return s switch
            {
                RedeSocialFinalidade.InstagramVideo => _.Raw($"<span class='badge bg-pink text-white'>{s}</span>"),
                RedeSocialFinalidade.YoutubeShorts => _.Raw($"<span class='badge bg-darkred text-white'>{s}</span>"),
                RedeSocialFinalidade.YoutubeVideo => _.Raw($"<span class='badge bg-red text-white'>{s}</span>"),
                RedeSocialFinalidade.NENHUMA => _.Raw($"<span class='badge bg-black text-white'>{s}</span>"),
            };
        }

        public static IHtmlContent StatusBadge(this IHtmlHelper _, StatusProcesso? s)
        {
            if (!s.HasValue) return _.Raw("");
            return s switch
            {
                StatusProcesso.Criado => _.Raw($"<span class='badge bg-primary text-white'>{s}</span>"),
                StatusProcesso.Agendado => _.Raw($"<span class='badge bg-primary text-white'>🗓{s}</span>"),

                StatusProcesso.ImagemOK => _.Raw($"<span class='badge bg-success text-white'>✔{s}</span>"),
                StatusProcesso.AudioOK => _.Raw($"<span class='badge bg-success text-white'>✔{s}</span>"),
                StatusProcesso.VideoOK => _.Raw($"<span class='badge bg-success text-white'>✔{s}</span>"),
                StatusProcesso.PublicandoOK => _.Raw($"<span class='badge bg-success text-white'>✔{s}</span>"),

                StatusProcesso.GerandoImagem => _.Raw($"<span class='badge bg-black text-white'>{s}</span>"),
                StatusProcesso.GerandoAudio => _.Raw($"<span class='badge bg-black text-white'>{s}</span>"),
                StatusProcesso.GerandoVideo => _.Raw($"<span class='badge bg-black text-white'>{s}</span>"),
                StatusProcesso.Publicando => _.Raw($"<span class='badge bg-black text-white'>{s}</span>"),

                StatusProcesso.ImagemErro => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),
                StatusProcesso.AudioErro => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),
                StatusProcesso.PublicandoErro => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),
                StatusProcesso.Erro => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),
                StatusProcesso.VideoErro => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),

                
            };
        }
        public static IHtmlContent StatusBadge(this IHtmlHelper _, Agenda.Status? s)
        {
            if (!s.HasValue) return _.Raw("");
            return s switch
            {
                Agenda.Status.criado => _.Raw($"<span class='badge bg-primary text-white'>{s}</span>"),
                Agenda.Status.agendado => _.Raw($"<span class='badge bg-primary text-white'>🗓{s}</span>"),

                Agenda.Status.concluido => _.Raw($"<span class='badge bg-success text-white'>✔{s}</span>"),
                Agenda.Status.executando => _.Raw($"<span class='badge bg-black text-white'>{s}</span>"),

                Agenda.Status.cancelado => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),
                Agenda.Status.falha => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),
                Agenda.Status.erro => _.Raw($"<span class='badge bg-warning text-white'>⚠{s}</span>"),


            };
        }
    }
}
