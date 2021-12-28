using Futebox.DB;
using Futebox.Models.Enums;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public abstract class SubProcesso : BaseEntity
    {
        public string processoId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RedeSocialFinalidade redeSocial { get; set; }

        public string linkDaImagemDoVideo { get; set; }
        public int larguraVideo { get; set; }
        public int alturaVideo { get; set; }

        public string nomeDoArquivoAudio { get; set; }
        public string nomeDoArquivoImagem { get; set; }
        public string nomeDoArquivoVideo { get; set; }
        public string nomeDoArquivoRoteiro { get; set; }

        public string pastaDoArquivo { get; set; }

        public string linkPostagem { get; set; }
        public string roteiro { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StatusProcesso status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CategoriaVideo categoriaVideo { get; set; }
    }

    public abstract class YoutubeSubProcessoBase : SubProcesso
    {
        public string tituloVideo { get; set; }
        public string descricaoVideo { get; set; }
        public string playlist { get; set; }
    }

    public abstract class InstagramSubProcessoBase : SubProcesso
    {
        public string legendaPostagem { get; set; }
    }

    public class SubProcessoYoutubeShort : YoutubeSubProcessoBase
    {
        public static SubProcessoYoutubeShort New(string processoId,
            CategoriaVideo categoria,
            string linkImagemVideo,
            string roteiro,
            string titulo,
            string descricao,
            string playlist
            )
        {
            return new SubProcessoYoutubeShort()
            {
                processoId = processoId,
                redeSocial = RedeSocialFinalidade.YoutubeShorts,
                categoriaVideo = categoria,
                linkDaImagemDoVideo = linkImagemVideo,
                alturaVideo = 1920,
                larguraVideo = 1080,
                nomeDoArquivoAudio = $"{processoId}.mp3",
                nomeDoArquivoImagem = $"{RedeSocialFinalidade.YoutubeShorts}.png",
                nomeDoArquivoVideo= $"{RedeSocialFinalidade.YoutubeShorts}.mp4",
                pastaDoArquivo = $"{Settings.ApplicationsRoot}/Arquivos/{processoId}/",
                roteiro = roteiro,
                tituloVideo = titulo,
                descricaoVideo = descricao,
                playlist = playlist,
                status = StatusProcesso.Criado
            };
        }
    }

    public class SubProcessoYoutubeVideo : YoutubeSubProcessoBase
    {
        public static SubProcessoYoutubeVideo New(string processoId,
            CategoriaVideo categoria,
            string linkImagemVideo,
            string roteiro,
            string titulo,
            string descricao,
            string playlist)
        {
            return new SubProcessoYoutubeVideo()
            {
                processoId = processoId,
                redeSocial = RedeSocialFinalidade.YoutubeVideo,
                categoriaVideo = categoria,
                linkDaImagemDoVideo = linkImagemVideo,
                alturaVideo = 1080,
                larguraVideo = 1920,
                nomeDoArquivoAudio = $"{processoId}.mp3",
                nomeDoArquivoImagem = $"{RedeSocialFinalidade.YoutubeVideo}.png",
                nomeDoArquivoVideo = $"{RedeSocialFinalidade.YoutubeVideo}.mp4",
                pastaDoArquivo = $"{Settings.ApplicationsRoot}/Arquivos/{processoId}/",
                roteiro = roteiro,
                tituloVideo = titulo,
                descricaoVideo = descricao,
                playlist = playlist,
                status = StatusProcesso.Criado
            };
        }
    }

    public class SubProcessoInstagramVideo : InstagramSubProcessoBase
    {
        public static SubProcessoInstagramVideo New(string processoId,
            CategoriaVideo categoria,
            string linkImagemVideo,
            string roteiro,
            string legenda)
        {
            return new SubProcessoInstagramVideo()
            {
                processoId = processoId,
                redeSocial = RedeSocialFinalidade.InstagramVideo,
                categoriaVideo = categoria,
                linkDaImagemDoVideo = linkImagemVideo,
                alturaVideo = 1350,
                larguraVideo = 1080,
                nomeDoArquivoAudio = $"{processoId}.mp3",
                nomeDoArquivoImagem = $"{RedeSocialFinalidade.InstagramVideo}.png",
                nomeDoArquivoVideo = $"{RedeSocialFinalidade.InstagramVideo}.mp4",
                pastaDoArquivo = $"{Settings.ApplicationsRoot}/Arquivos/{processoId}/",
                roteiro = roteiro,
                legendaPostagem = legenda,
                status = StatusProcesso.Criado
            };
        }
    }
}
