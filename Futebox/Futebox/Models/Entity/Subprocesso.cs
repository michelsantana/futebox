//using Futebox.DB;
//using Futebox.Models.Enums;
//using Newtonsoft.Json.Converters;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace Futebox.Models
//{
//    public abstract class SubProcesso : BaseEntity
//    {
//        public string processoId { get; set; }

//        [JsonConverter(typeof(StringEnumConverter))]
//        public RedeSocialFinalidade redeSocial { get; set; }

//        public string linkDaImagemDoVideo { get; set; }
//        public int larguraVideo { get; set; }
//        public int alturaVideo { get; set; }

//        public string nomeDoArquivoAudio { get; set; }
//        public string nomeDoArquivoImagem { get; set; }
//        public string nomeDoArquivoVideo { get; set; }
//        public string nomeDoArquivoRoteiro { get; set; }

//        public string pastaDoArquivo { get; set; }

//        public string linkPostagem { get; set; }
//        public string roteiro { get; set; }

//        [JsonConverter(typeof(StringEnumConverter))]
//        public StatusProcesso status { get; set; }
//        [JsonConverter(typeof(StringEnumConverter))]
//        public CategoriaVideo categoriaVideo { get; set; }

//        public string args { get; set; }

//        public abstract string obterTitulo();
//        public abstract string obterDescricao();
//        public abstract string obterLegenda();

//        private IProcessoArgs _args;
//        public T ToArgs<T>() where T : IProcessoArgs
//        {   
//            if(_args == null) _args = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(this.args);
//            return (T)_args;
//        }
//    }

//    public abstract class YoutubeSubProcessoBase : SubProcesso
//    {
//        public string tituloVideo { get; set; }
//        public string descricaoVideo { get; set; }
//        public string playlist { get; set; }
//    }

//    public abstract class InstagramSubProcessoBase : SubProcesso
//    {
//        public string legendaPostagem { get; set; }
//    }

//    public class SubProcessoYoutubeShort : YoutubeSubProcessoBase
//    {
//        public const RedeSocialFinalidade social = RedeSocialFinalidade.YoutubeShorts;
//        public static SubProcessoYoutubeShort New(string processoId,
//            CategoriaVideo categoria,
//#warning remover
//            string linkImagemVideo,
//            string roteiro,
//            string titulo,
//            string descricao,
//            string playlist,
//            string args = null
//            )
//        {
//            var id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
//            return new SubProcessoYoutubeShort()
//            {
//                id = id,
//                processoId = processoId,
//                redeSocial = RedeSocialFinalidade.YoutubeShorts,
//                categoriaVideo = categoria,
//                //linkDaImagemDoVideo = linkImagemVideo,
//                linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}Miniaturas?t={categoria}&q={args}&w={SocialMediaUtils.Width(social)}&h={SocialMediaUtils.Height(social)}",
//                alturaVideo = SocialMediaUtils.Height(social),
//                larguraVideo = SocialMediaUtils.Width(social),
//                nomeDoArquivoAudio = $"{processoId}.mp3",
//                nomeDoArquivoImagem = $"{social}.png",
//                nomeDoArquivoVideo = $"{social}.mp4",
//                pastaDoArquivo = Path.Combine(Settings.ApplicationsRoot, "Arquivos", processoId),
//                roteiro = roteiro,
//                tituloVideo = titulo,
//                descricaoVideo = descricao,
//                playlist = playlist,
//                status = StatusProcesso.Criado,
//                args = args,
//            };
//        }

//        public override string obterDescricao()
//        {
//            return this.descricaoVideo;
//        }

//        public override string obterLegenda()
//        {
//            return null;
//        }

//        public override string obterTitulo()
//        {
//            return this.tituloVideo;
//        }
//    }

//    public class SubProcessoYoutubeVideo : YoutubeSubProcessoBase
//    {
//        public const RedeSocialFinalidade social = RedeSocialFinalidade.YoutubeVideo;
        
//        public static SubProcessoYoutubeVideo New(string processoId,
//            CategoriaVideo categoria,
//#warning remover
//            string linkImagemVideo,
//            string roteiro,
//            string titulo,
//            string descricao,
//            string playlist,
//            string args = null)
//        {
//            var id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
//            return new SubProcessoYoutubeVideo()
//            {
//                id = id,
//                processoId = processoId,
//                redeSocial = social,
//                categoriaVideo = categoria,
//                //linkDaImagemDoVideo = linkImagemVideo,
//                linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}Miniaturas?t={categoria}&q={args}&w={SocialMediaUtils.Width(social)}&h={SocialMediaUtils.Height(social)}",
//                alturaVideo = SocialMediaUtils.Width(social),
//                larguraVideo = SocialMediaUtils.Height(social),
//                nomeDoArquivoAudio = $"{processoId}.mp3",
//                nomeDoArquivoImagem = $"{social}.png",
//                nomeDoArquivoVideo = $"{social}.mp4",
//                pastaDoArquivo = Path.Combine(Settings.ApplicationsRoot, "Arquivos", processoId),
//                roteiro = roteiro,
//                tituloVideo = titulo,
//                descricaoVideo = descricao,
//                playlist = playlist,
//                status = StatusProcesso.Criado,
//                args = args,
//            };
//        }

//        public override string obterDescricao()
//        {
//            return this.descricaoVideo;
//        }

//        public override string obterLegenda()
//        {
//            return null;
//        }

//        public override string obterTitulo()
//        {
//            return this.tituloVideo;
//        }
//    }

//    public class SubProcessoInstagramVideo : InstagramSubProcessoBase
//    {
//        public const RedeSocialFinalidade social = RedeSocialFinalidade.InstagramVideo;

//        public static SubProcessoInstagramVideo New(string processoId,
//            CategoriaVideo categoria,
//#warning remover
//            string linkImagemVideo,
//            string roteiro,
//            string legenda,
//            string args = null)
//        {
//            var id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
//            return new SubProcessoInstagramVideo()
//            {
//                id = id,
//                processoId = processoId,
//                redeSocial = social,
//                categoriaVideo = categoria,
//                //linkDaImagemDoVideo = linkImagemVideo,
//                linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}Miniaturas?t={categoria}&q={args}&w={SocialMediaUtils.Width(social)}&h={SocialMediaUtils.Height(social)}",
//                alturaVideo = SocialMediaUtils.Height(social),
//                larguraVideo = SocialMediaUtils.Width(social),
//                nomeDoArquivoAudio = $"{processoId}.mp3",
//                nomeDoArquivoImagem = $"{social}.png",
//                nomeDoArquivoVideo = $"{social}.mp4",
//                pastaDoArquivo = Path.Combine(Settings.ApplicationsRoot, "Arquivos", processoId),
//                roteiro = roteiro,
//                legendaPostagem = legenda,
//                status = StatusProcesso.Criado,
//                args = args,
//            };
//        }

//        public override string obterDescricao()
//        {
//            return null;
//        }

//        public override string obterLegenda()
//        {
//            return this.legendaPostagem;
//        }

//        public override string obterTitulo()
//        {
//            return null;
//        }
//    }
//}
