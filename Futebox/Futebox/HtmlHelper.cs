using Futebox.Models;
using Futebox.Models.Enums;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

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
            if (styleContent.Item1 < DateTime.Now)
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
                //StatusProcesso.Agendado => _.Raw($"<span class='badge bg-primary text-white'>🗓{s}</span>"),

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

        public static IHtmlContent IconHelper(this IHtmlHelper _, RedeSocialFinalidade s, int sizePx)
        {
            return s switch
            {
                RedeSocialFinalidade.InstagramVideo => _.Raw($"<img width='{sizePx}px' src='https://icongr.am/simple/instagram.svg?size=64&color=currentColor&colored=true' />"),
                RedeSocialFinalidade.YoutubeShorts => _.Raw($"<img width='{sizePx}px' src='https://icongr.am/simple/youtubemusic.svg?size=64&color=currentColor&colored=true' />"),
                RedeSocialFinalidade.YoutubeVideo => _.Raw($"<img width='{sizePx}px' src='https://icongr.am/simple/youtube.svg?size=64&color=currentColor&colored=true' />"),
                RedeSocialFinalidade.NENHUMA => _.Raw($"<img width='{sizePx}px' src='https://icongr.am/simple/csharp.svg?size=64&color=currentColor&colored=false' />"),
            };
        }

        public static IHtmlContent IconHelper(this IHtmlHelper _, string collection, string icon, int size)
        {
            return _.Raw($"<img src='https://icongr.am/{collection}/{icon}.svg?size={size}&color=currentColor&colored=false' />");
        }
        public static IHtmlContent IconSimpleHelper(this IHtmlHelper _, string icon, int size)
        {
            return IconHelper(_, "simple", icon, size);
        }
        public static IHtmlContent IconMaterialHelper(this IHtmlHelper _, string icon, int size)
        {
            return IconHelper(_, "material", icon, size);
        }
        public static IHtmlContent IconEntypoHelper(this IHtmlHelper _, string icon, int size)
        {
            return IconHelper(_, "entypo", icon, size);
        }

        public static iconBuilder material(this IHtmlHelper _)
        {
            return iconBuilder.material(_);
        }
        public static iconBuilder simple(this IHtmlHelper _)
        {
            return iconBuilder.simple(_);
        }
        public static iconBuilder entypo(this IHtmlHelper _)
        {
            return iconBuilder.entypo(_);
        }

        public class iconBuilder
        {
            IHtmlHelper _ { get; set; }
            string collection { get; set; }
            string icon { get; set; }
            int size { get; set; }
            string color { get; set; }

            public iconBuilder(IHtmlHelper _, string collection)
            {
                this._ = _;
                this.collection = collection;
            }

            public static iconBuilder material(IHtmlHelper _)
            {
                return new iconBuilder(_, "material");
            }
            public static iconBuilder simple(IHtmlHelper _)
            {
                return new iconBuilder(_, "simple");
            }
            public static iconBuilder entypo(IHtmlHelper _)
            {
                return new iconBuilder(_, "entypo");
            }

            public iconBuilder i(string i)
            {
                this.icon = i;
                return this;
            }
            public iconBuilder s(int i)
            {
                this.size = i;
                return this;
            }
            public iconBuilder c(string i)
            {
                this.color = i;
                return this;
            }
            public IHtmlContent build()
            {
                return _.Raw($"<img src='https://icongr.am/{this.collection}/{this.icon}.svg?size={this.size}&color={this.color}&colored=true' />");
            }
        }
    }
}
