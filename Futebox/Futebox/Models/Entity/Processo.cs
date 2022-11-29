using Futebox.DB;
using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Futebox.Models
{
    public class Processo : BaseEntity
    {
        public String nome { get; set; }
        public CategoriaVideo categoria { get; set; }
        public StatusProcesso status { get; set; }
        public String log { get; set; }
        public DateTime? agendamento { get; set; }
        public Boolean agendado { get; set; }
        public String pasta { get; set; }
        public String args { get; set; }

        public String linkDaImagemDoVideo { get; set; }
        public Int32 larguraVideo { get; set; }
        public Int32 alturaVideo { get; set; }
        public String nomeDoArquivoAudio { get; set; }
        public String nomeDoArquivoImagem { get; set; }
        public String nomeDoArquivoVideo { get; set; }
        public String roteiro { get; set; }
        public String tituloVideo { get; set; }
        public String descricaoVideo { get; set; }
        public RedeSocialFinalidade social { get; set; }
        public string jobKey { get; set; }

        public IProcessoArgs _args;
        public T ToArgs<T>() where T : IProcessoArgs
        {
            if (_args == null) _args = JsonConvert.DeserializeObject<T>(this.args);
            return (T)_args;
        }

        public string obterTitulo()
        {
            return this.tituloVideo;
        }

        public string obterDescricao()
        {
            return this.descricaoVideo;
        }


        public Processo()
        {

        }

        public Processo(ProcessoRodadaArgs rodadaArgs)
        {
            rodadaArgs.titulo = rodadaArgs.titulo.Replace("{DATA}", DateTime.Now.AddDays(rodadaArgs.dataRelativa ?? 0).ToString("dd/MM/yyyy"));

            var json = JsonConvert.SerializeObject(rodadaArgs);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(1);
            this.alturaVideo = SocialMediaUtils.Height(rodadaArgs.social);
            this.args = json;
            this.categoria = CategoriaVideo.rodada;
            this.larguraVideo = SocialMediaUtils.Width(rodadaArgs.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}/Miniaturas?t={categoria}&q={Uri.EscapeDataString(json)}&w={SocialMediaUtils.Width(rodadaArgs.social)}&h={SocialMediaUtils.Height(rodadaArgs.social)}";
            this.nomeDoArquivoAudio = $"{DateTime.Now.ToString("MMddHHmmss")}-{rodadaArgs.social}.mp3";
            this.nomeDoArquivoImagem = $"{rodadaArgs.social}.png";
            this.nomeDoArquivoVideo = $"{rodadaArgs.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = rodadaArgs.social;
            this.status = StatusProcesso.Criado;
            this.tituloVideo = !string.IsNullOrEmpty(rodadaArgs.titulo) ? rodadaArgs.titulo : this.tituloVideo;
            this.tituloVideo = this.tituloVideo.Replace("{DATA}", DateTime.Now.AddDays(rodadaArgs.dataRelativa ?? 0).ToString("dd/MM/yyyy"));
            this.nome = this.tituloVideo;
        }

        public Processo(ProcessoJogosDiaArgs jogosDia)
        {
            jogosDia.titulo = jogosDia.titulo.Replace("{DATA}", DateTime.Now.AddDays(jogosDia.dataRelativa ?? 0).ToString("dd/MM/yyyy"));

            var json = JsonConvert.SerializeObject(jogosDia);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(1);
            this.alturaVideo = SocialMediaUtils.Height(jogosDia.social);
            this.args = json;
            this.categoria = CategoriaVideo.jogosdia;
            this.larguraVideo = SocialMediaUtils.Width(jogosDia.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}/Miniaturas?t={categoria}&q={Uri.EscapeDataString(json)}&w={SocialMediaUtils.Width(jogosDia.social)}&h={SocialMediaUtils.Height(jogosDia.social)}";
            this.nomeDoArquivoAudio = $"{DateTime.Now.ToString("MMddHHmmss")}-{jogosDia.social}.mp3";
            this.nomeDoArquivoImagem = $"{jogosDia.social}.png";
            this.nomeDoArquivoVideo = $"{jogosDia.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = jogosDia.social;
            this.status = StatusProcesso.Criado;
            this.tituloVideo = !string.IsNullOrEmpty(jogosDia.titulo) ? jogosDia.titulo : this.tituloVideo;
            this.tituloVideo = this.tituloVideo.Replace("{DATA}", DateTime.Now.AddDays(jogosDia.dataRelativa??0).ToString("dd/MM/yyyy"));
            this.nome = this.tituloVideo;
        }

        public Processo(ProcessoClassificacaoArgs classificacaoArgs)
        {
            classificacaoArgs.titulo = classificacaoArgs.titulo.Replace("{DATA}", DateTime.Now.AddDays(classificacaoArgs.dataRelativa ?? 0).ToString("dd/MM/yyyy"));

            var json = JsonConvert.SerializeObject(classificacaoArgs);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(30);
            this.alturaVideo = SocialMediaUtils.Height(classificacaoArgs.social);
            this.args = json;
            this.categoria = CategoriaVideo.classificacao;
            this.larguraVideo = SocialMediaUtils.Width(classificacaoArgs.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}/Miniaturas?t={categoria}&q={Uri.EscapeDataString(json)}&w={SocialMediaUtils.Width(classificacaoArgs.social)}&h={SocialMediaUtils.Height(classificacaoArgs.social)}";
            this.nomeDoArquivoAudio = $"{DateTime.Now.ToString("MMddHHmmss")}-{classificacaoArgs.social}.mp3";
            this.nomeDoArquivoImagem = $"{classificacaoArgs.social}.png";
            this.nomeDoArquivoVideo = $"{classificacaoArgs.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = classificacaoArgs.social;
            this.status = StatusProcesso.Criado;
            this.tituloVideo = !string.IsNullOrEmpty(classificacaoArgs.titulo) ? classificacaoArgs.titulo : this.tituloVideo;
            this.tituloVideo = this.tituloVideo.Replace("{DATA}", DateTime.Now.AddDays(classificacaoArgs.dataRelativa ?? 0).ToString("dd/MM/yyyy"));
            this.nome = this.tituloVideo;
        }

        public Processo(ProcessoPartidaArgs partidaArgs)
        {
            partidaArgs.titulo = partidaArgs.titulo.Replace("{DATA}", DateTime.Now.AddDays(partidaArgs.dataRelativa ?? 0).ToString("dd/MM/yyyy"));
            
            var json = JsonConvert.SerializeObject(partidaArgs);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(30);
            this.alturaVideo = SocialMediaUtils.Height(partidaArgs.social);
            this.args = json;
            this.categoria = CategoriaVideo.partida;
            this.larguraVideo = SocialMediaUtils.Width(partidaArgs.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}/Miniaturas?t={categoria}&q={Uri.EscapeDataString(json)}&w={SocialMediaUtils.Width(partidaArgs.social)}&h={SocialMediaUtils.Height(partidaArgs.social)}";
            this.nomeDoArquivoAudio = $"{DateTime.Now.ToString("MMddHHmmss")}-{partidaArgs.social}.mp3";
            this.nomeDoArquivoImagem = $"{partidaArgs.social}.png";
            this.nomeDoArquivoVideo = $"{partidaArgs.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = partidaArgs.social;
            this.status = StatusProcesso.Criado;
            this.tituloVideo = !string.IsNullOrEmpty(partidaArgs.titulo) ? partidaArgs.titulo : this.tituloVideo;
            this.tituloVideo = this.tituloVideo.Replace("{DATA}", DateTime.Now.AddDays(partidaArgs.dataRelativa ?? 0).ToString("dd/MM/yyyy"));
            this.nome = this.tituloVideo;
        }
    }
}
