using Dapper.FluentMap.Dommel.Mapping;
using Futebox.Models;

namespace Futebox.DB.Mappers
{
    public class TimeMap : DommelEntityMap<Time>
    {
        public TimeMap()
        {
            ToTable("time");
            Map(x => x.id).ToColumn("id");
            Map(x => x.criacao).ToColumn("criacao");
            Map(x => x.alteracao).ToColumn("alteracao");

            Map(x => x.nome).ToColumn("nome");
            Map(x => x.sigla).ToColumn("sigla");
            Map(x => x.urlLogo).ToColumn("urlLogo");
            Map(x => x.origemDado).ToColumn("origemDado");
            Map(x => x.origem_ext_id).ToColumn("origem_ext_id");
            Map(x => x.origem_ext_equipe_id).ToColumn("origem_ext_equipe_id");
            Map(x => x.nomeAdaptadoWatson).ToColumn("nomeAdaptadoWatson");
            Map(x => x.logoBin).ToColumn("logoBin");

            Map(x => x.selecao).ToColumn("selecao");
            Map(x => x.torcedorNoSingular).ToColumn("torcedorNoSingular");
            Map(x => x.torcedorNoPlural).ToColumn("torcedorNoPlural");
            Map(x => x.timeFantasia).ToColumn("timeFantasia");
            Map(x => x.cidade).ToColumn("cidade");
            Map(x => x.estado).ToColumn("estado");
            Map(x => x.pais).ToColumn("pais");
            Map(x => x.grupo).ToColumn("grupo");
            Map(x => x.isTimeGrande).ToColumn("isTimeGrande");
            Map(x => x.hasScout).ToColumn("hasScout");
            Map(x => x.idTecnico).ToColumn("idTecnico");
            Map(x => x.tecnico).ToColumn("tecnico");
        }
    }

    public class ProcessoMap : DommelEntityMap<Processo>
    {
        public ProcessoMap()
        {
            ToTable("processo");
            Map(x => x.id).ToColumn("id");
            Map(x => x.criacao).ToColumn("criacao");
            Map(x => x.alteracao).ToColumn("alteracao");

            Map(x => x.agendado).ToColumn("agendado");
            Map(x => x.agendamento).ToColumn("agendamento");
            Map(x => x.campeonatoId).ToColumn("campeonatoId");
            Map(x => x.categoria).ToColumn("categoria");
            Map(x => x.nome).ToColumn("nome");
            Map(x => x.notificacao).ToColumn("notificacao");
            Map(x => x.partidaId).ToColumn("partidaId");
            Map(x => x.rodadaId).ToColumn("rodadaId");
            Map(x => x.status).ToColumn("status");
            Map(x => x.statusMensagem).ToColumn("statusMensagem");
        }

        //public class SubProcessoMap : DommelEntityMap<SubProcesso>
        //{
        //    public SubProcessoMap()
        //    {
        //        ToTable("subprocesso");
        //        Map(x => x.id).ToColumn("id");
        //        Map(x => x.criacao).ToColumn("criacao");
        //        Map(x => x.alteracao).ToColumn("alteracao");

        //        Map(x => x.alturaVideo).ToColumn("alteracao");
        //        Map(x => x.categoriaVideo).ToColumn("categoriaVideo");
        //        Map(x => x.larguraVideo).ToColumn("larguraVideo");
        //        Map(x => x.linkDaImagemDoVideo).ToColumn("linkDaImagemDoVideo");
        //        Map(x => x.linkPostagem).ToColumn("linkPostagem");
        //        Map(x => x.nomeDoArquivo).ToColumn("nomeDoArquivo");
        //        Map(x => x.pastaDoArquivo).ToColumn("pastaDoArquivo");
        //        Map(x => x.processoId).ToColumn("processoId");
        //        Map(x => x.redeSocial).ToColumn("redeSocial");
        //        Map(x => x.roteiro).ToColumn("roteiro");
        //        Map(x => x.status).ToColumn("status");
        //    }
        //}

        public class YoutubeSubProcessoMap<T> : DommelEntityMap<T> where T : YoutubeSubProcessoBase
        {
            public YoutubeSubProcessoMap()
            {
                ToTable("subprocesso");
                Map(x => x.id).ToColumn("id");
                Map(x => x.criacao).ToColumn("criacao");
                Map(x => x.alteracao).ToColumn("alteracao");

                Map(x => x.alturaVideo).ToColumn("alturaVideo");
                Map(x => x.categoriaVideo).ToColumn("categoriaVideo");
                Map(x => x.descricaoVideo).ToColumn("descricaoVideo");
                Map(x => x.larguraVideo).ToColumn("larguraVideo");
                Map(x => x.linkDaImagemDoVideo).ToColumn("linkDaImagemDoVideo");
                Map(x => x.linkPostagem).ToColumn("linkPostagem");

                Map(x => x.nomeDoArquivoAudio).ToColumn("nomeDoArquivoAudio");
                Map(x => x.nomeDoArquivoImagem).ToColumn("nomeDoArquivoImagem");
                Map(x => x.nomeDoArquivoVideo).ToColumn("nomeDoArquivoVideo");

                Map(x => x.pastaDoArquivo).ToColumn("pastaDoArquivo");
                Map(x => x.playlist).ToColumn("playlist");
                Map(x => x.processoId).ToColumn("processoId");
                Map(x => x.redeSocial).ToColumn("redeSocial");
                Map(x => x.roteiro).ToColumn("roteiro");
                Map(x => x.status).ToColumn("status");
                Map(x => x.tituloVideo).ToColumn("tituloVideo");
            }
        }

        public class InstagramSubProcessoMap<T> : DommelEntityMap<T> where T : InstagramSubProcessoBase
        {
            public InstagramSubProcessoMap()
            {
                ToTable("subprocesso");
                Map(x => x.id).ToColumn("id");
                Map(x => x.criacao).ToColumn("criacao");
                Map(x => x.alteracao).ToColumn("alteracao");

                Map(x => x.alturaVideo).ToColumn("alturaVideo");
                Map(x => x.categoriaVideo).ToColumn("categoriaVideo");
                Map(x => x.larguraVideo).ToColumn("larguraVideo");
                Map(x => x.legendaPostagem).ToColumn("legendaPostagem");
                Map(x => x.linkDaImagemDoVideo).ToColumn("linkDaImagemDoVideo");
                Map(x => x.linkPostagem).ToColumn("linkPostagem");
                
                Map(x => x.nomeDoArquivoAudio).ToColumn("nomeDoArquivoAudio");
                Map(x => x.nomeDoArquivoImagem).ToColumn("nomeDoArquivoImagem");
                Map(x => x.nomeDoArquivoVideo).ToColumn("nomeDoArquivoVideo");
                
                Map(x => x.pastaDoArquivo).ToColumn("pastaDoArquivo");
                Map(x => x.processoId).ToColumn("processoId");
                Map(x => x.redeSocial).ToColumn("redeSocial");
                Map(x => x.roteiro).ToColumn("roteiro");
                Map(x => x.status).ToColumn("status");
            }
        }
    }
}