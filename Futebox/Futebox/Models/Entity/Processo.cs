using Futebox.DB;
using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;

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
            return this.tituloVideo;
        }


        public Processo()
        {

        }

        public Processo(ProcessoRodadaArgs rodadaArgs)
        {
            var json = JsonConvert.SerializeObject(rodadaArgs);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(30);
            this.alturaVideo = SocialMediaUtils.Height(rodadaArgs.social);
            this.args = json;
            this.categoria = CategoriaVideo.rodada;
            this.larguraVideo = SocialMediaUtils.Width(rodadaArgs.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}Miniaturas?t={categoria}&q={json}&w={SocialMediaUtils.Width(rodadaArgs.social)}&h={SocialMediaUtils.Height(rodadaArgs.social)}";
            this.nome = $"{categoria} - {rodadaArgs.rodada} - {rodadaArgs.campeonato}";
            this.nomeDoArquivoAudio = $"{rodadaArgs.social}.mp3";
            this.nomeDoArquivoImagem = $"{rodadaArgs.social}.png";
            this.nomeDoArquivoVideo = $"{rodadaArgs.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = rodadaArgs.social;
            this.status = StatusProcesso.Criado;
        }

        public Processo(ProcessoClassificacaoArgs classificacaoArgs)
        {
            var json = JsonConvert.SerializeObject(classificacaoArgs);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(30);
            this.alturaVideo = SocialMediaUtils.Height(classificacaoArgs.social);
            this.args = json;
            this.categoria = CategoriaVideo.rodada;
            this.larguraVideo = SocialMediaUtils.Width(classificacaoArgs.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}Miniaturas?t={categoria}&q={json}&w={SocialMediaUtils.Width(classificacaoArgs.social)}&h={SocialMediaUtils.Height(classificacaoArgs.social)}";
            this.nome = $"{categoria} - {classificacaoArgs.campeonato}";
            this.nomeDoArquivoAudio = $"{classificacaoArgs.social}.mp3";
            this.nomeDoArquivoImagem = $"{classificacaoArgs.social}.png";
            this.nomeDoArquivoVideo = $"{classificacaoArgs.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = classificacaoArgs.social;
            this.status = StatusProcesso.Criado;
        }

        public Processo(ProcessoPartidaArgs partidaArgs)
        {
            var json = JsonConvert.SerializeObject(partidaArgs);
            this.agendado = false;
            this.agendamento = DateTime.Now.AddMinutes(30);
            this.alturaVideo = SocialMediaUtils.Height(partidaArgs.social);
            this.args = json;
            this.categoria = CategoriaVideo.rodada;
            this.larguraVideo = SocialMediaUtils.Width(partidaArgs.social);
            this.linkDaImagemDoVideo = $"{Settings.ApplicationHttpBaseUrl}Miniaturas?t={categoria}&q={json}&w={SocialMediaUtils.Width(partidaArgs.social)}&h={SocialMediaUtils.Height(partidaArgs.social)}";
            this.nome = $"{categoria} - {partidaArgs.campeonato}";
            this.nomeDoArquivoAudio = $"{partidaArgs.social}.mp3";
            this.nomeDoArquivoImagem = $"{partidaArgs.social}.png";
            this.nomeDoArquivoVideo = $"{partidaArgs.social}.mp4";
            this.pasta = $"{Settings.ApplicationRoot}/arquivos/{Guid.NewGuid()}";
            this.social = partidaArgs.social;
            this.status = StatusProcesso.Criado;
        }
    }
}
