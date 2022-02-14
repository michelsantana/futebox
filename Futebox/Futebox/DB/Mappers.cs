using Dapper.FluentMap.Dommel.Mapping;
using Futebox.DB.Migrations;
using Futebox.Models;

namespace Futebox.DB.Mappers
{
    public class TimeMap : DommelEntityMap<Time>
    {
        public TimeMap()
        {
            ToTable(DbSchemaTime.time);
            Map(x => x.id).ToColumn(DbSchemaTime.id);
            Map(x => x.criacao).ToColumn(DbSchemaTime.criacao);
            Map(x => x.alteracao).ToColumn(DbSchemaTime.alteracao);

            Map(x => x.nome).ToColumn(DbSchemaTime.nome);
            Map(x => x.sigla).ToColumn(DbSchemaTime.sigla);
            Map(x => x.urlLogo).ToColumn(DbSchemaTime.urlLogo);
            Map(x => x.origemDado).ToColumn(DbSchemaTime.origemDado);
            Map(x => x.origem_ext_id).ToColumn(DbSchemaTime.origem_ext_id);
            Map(x => x.origem_ext_equipe_id).ToColumn(DbSchemaTime.origem_ext_equipe_id);
            Map(x => x.nomeAdaptadoWatson).ToColumn(DbSchemaTime.nomeAdaptadoWatson);
            Map(x => x.logoBin).ToColumn(DbSchemaTime.logoBin);

            Map(x => x.selecao).ToColumn(DbSchemaTime.selecao);
            Map(x => x.torcedorNoSingular).ToColumn(DbSchemaTime.torcedorNoSingular);
            Map(x => x.torcedorNoPlural).ToColumn(DbSchemaTime.torcedorNoPlural);
            Map(x => x.timeFantasia).ToColumn(DbSchemaTime.timeFantasia);
            Map(x => x.cidade).ToColumn(DbSchemaTime.cidade);
            Map(x => x.estado).ToColumn(DbSchemaTime.estado);
            Map(x => x.pais).ToColumn(DbSchemaTime.pais);
            Map(x => x.grupo).ToColumn(DbSchemaTime.grupo);
            Map(x => x.isTimeGrande).ToColumn(DbSchemaTime.isTimeGrande);
            Map(x => x.hasScout).ToColumn(DbSchemaTime.hasScout);
            Map(x => x.idTecnico).ToColumn(DbSchemaTime.idTecnico);
            Map(x => x.tecnico).ToColumn(DbSchemaTime.tecnico);
            Map(x => x.logoEspecifico).ToColumn(DbSchemaTime.logoEspecifico);
        }
    }

    public class ProcessoMap : DommelEntityMap<Processo>
    {
        public ProcessoMap()
        {
            ToTable(DbSchemaProcesso.processo);
            Map(x => x.id).ToColumn(DbSchemaProcesso.id);
            Map(x => x.criacao).ToColumn(DbSchemaProcesso.criacao);
            Map(x => x.alteracao).ToColumn(DbSchemaProcesso.alteracao);

            Map(x => x.agendado).ToColumn(DbSchemaProcesso.agendado);
            Map(x => x.agendamento).ToColumn(DbSchemaProcesso.agendamento);
            Map(x => x.campeonatoId).ToColumn(DbSchemaProcesso.campeonatoId);
            Map(x => x.categoria).ToColumn(DbSchemaProcesso.categoria);
            Map(x => x.nome).ToColumn(DbSchemaProcesso.nome);
            Map(x => x.notificacao).ToColumn(DbSchemaProcesso.notificacao);
            Map(x => x.partidaId).ToColumn(DbSchemaProcesso.partidaId);
            Map(x => x.rodadaId).ToColumn(DbSchemaProcesso.rodadaId);
            Map(x => x.status).ToColumn(DbSchemaProcesso.status);
            Map(x => x.statusMensagem).ToColumn(DbSchemaProcesso.statusMensagem);
        }
    }

    public class YoutubeSubProcessoMap<T> : DommelEntityMap<T> where T : YoutubeSubProcessoBase
    {
        public YoutubeSubProcessoMap()
        {
            ToTable(DbSchemaSubProcesso.subprocesso);
            Map(x => x.id).ToColumn(DbSchemaSubProcesso.id);
            Map(x => x.criacao).ToColumn(DbSchemaSubProcesso.criacao);
            Map(x => x.alteracao).ToColumn(DbSchemaSubProcesso.alteracao);

            Map(x => x.alturaVideo).ToColumn(DbSchemaSubProcesso.alturaVideo);
            Map(x => x.categoriaVideo).ToColumn(DbSchemaSubProcesso.categoriaVideo);
            Map(x => x.descricaoVideo).ToColumn(DbSchemaSubProcesso.descricaoVideo);
            Map(x => x.larguraVideo).ToColumn(DbSchemaSubProcesso.larguraVideo);
            Map(x => x.linkDaImagemDoVideo).ToColumn(DbSchemaSubProcesso.linkDaImagemDoVideo);
            Map(x => x.linkPostagem).ToColumn(DbSchemaSubProcesso.linkPostagem);

            Map(x => x.nomeDoArquivoAudio).ToColumn(DbSchemaSubProcesso.nomeDoArquivoAudio);
            Map(x => x.nomeDoArquivoImagem).ToColumn(DbSchemaSubProcesso.nomeDoArquivoImagem);
            Map(x => x.nomeDoArquivoVideo).ToColumn(DbSchemaSubProcesso.nomeDoArquivoVideo);

            Map(x => x.pastaDoArquivo).ToColumn(DbSchemaSubProcesso.pastaDoArquivo);
            Map(x => x.playlist).ToColumn(DbSchemaSubProcesso.playlist);
            Map(x => x.processoId).ToColumn(DbSchemaSubProcesso.processoId);
            Map(x => x.redeSocial).ToColumn(DbSchemaSubProcesso.redeSocial);
            Map(x => x.roteiro).ToColumn(DbSchemaSubProcesso.roteiro);
            Map(x => x.status).ToColumn(DbSchemaSubProcesso.status);
            Map(x => x.tituloVideo).ToColumn(DbSchemaSubProcesso.tituloVideo);
        }
    }

    public class InstagramSubProcessoMap<T> : DommelEntityMap<T> where T : InstagramSubProcessoBase
    {
        public InstagramSubProcessoMap()
        {
            ToTable(DbSchemaSubProcesso.subprocesso);
            Map(x => x.id).ToColumn(DbSchemaSubProcesso.id);
            Map(x => x.criacao).ToColumn(DbSchemaSubProcesso.criacao);
            Map(x => x.alteracao).ToColumn(DbSchemaSubProcesso.alteracao);

            Map(x => x.alturaVideo).ToColumn(DbSchemaSubProcesso.alturaVideo);
            Map(x => x.categoriaVideo).ToColumn(DbSchemaSubProcesso.categoriaVideo);
            Map(x => x.larguraVideo).ToColumn(DbSchemaSubProcesso.larguraVideo);
            Map(x => x.legendaPostagem).ToColumn(DbSchemaSubProcesso.legendaPostagem);
            Map(x => x.linkDaImagemDoVideo).ToColumn(DbSchemaSubProcesso.linkDaImagemDoVideo);
            Map(x => x.linkPostagem).ToColumn(DbSchemaSubProcesso.linkPostagem);

            Map(x => x.nomeDoArquivoAudio).ToColumn(DbSchemaSubProcesso.nomeDoArquivoAudio);
            Map(x => x.nomeDoArquivoImagem).ToColumn(DbSchemaSubProcesso.nomeDoArquivoImagem);
            Map(x => x.nomeDoArquivoVideo).ToColumn(DbSchemaSubProcesso.nomeDoArquivoVideo);

            Map(x => x.pastaDoArquivo).ToColumn(DbSchemaSubProcesso.pastaDoArquivo);
            Map(x => x.processoId).ToColumn(DbSchemaSubProcesso.processoId);
            Map(x => x.redeSocial).ToColumn(DbSchemaSubProcesso.redeSocial);
            Map(x => x.roteiro).ToColumn(DbSchemaSubProcesso.roteiro);
            Map(x => x.status).ToColumn(DbSchemaSubProcesso.status);
        }
    }

    public class CampeonatoMap : DommelEntityMap<Campeonato>
    {
        public CampeonatoMap()
        {
            ToTable(DbSchemaCampeonato.campeonato);
            Map(x => x.id).ToColumn(DbSchemaCampeonato.id);
            Map(x => x.criacao).ToColumn(DbSchemaCampeonato.criacao);
            Map(x => x.alteracao).ToColumn(DbSchemaCampeonato.alteracao);
            Map(x => x.origem_ext_id).ToColumn(DbSchemaCampeonato.origem_ext_id);
            Map(x => x.ativo).ToColumn(DbSchemaCampeonato.ativo);
            Map(x => x.pais).ToColumn(DbSchemaCampeonato.pais);
            Map(x => x.nome).ToColumn(DbSchemaCampeonato.nome);
            Map(x => x.urlLogo).ToColumn(DbSchemaCampeonato.urlLogo);
            Map(x => x.sdeSlug).ToColumn(DbSchemaCampeonato.sdeSlug);
            Map(x => x.temporada).ToColumn(DbSchemaCampeonato.temporada);
            Map(x => x.categoria).ToColumn(DbSchemaCampeonato.categoria);
            Map(x => x.temClassificacao).ToColumn(DbSchemaCampeonato.temClassificacao);
            Map(x => x.temClassificacaoPorGrupo).ToColumn(DbSchemaCampeonato.temClassificacaoPorGrupo);
            Map(x => x.tipoDeColeta).ToColumn(DbSchemaCampeonato.tipoDeColeta);
            Map(x => x.faseAtual).ToColumn(DbSchemaCampeonato.faseAtual);
            Map(x => x.quantidadeDeEquipes).ToColumn(DbSchemaCampeonato.quantidadeDeEquipes);
            Map(x => x.rodadaAtual).ToColumn(DbSchemaCampeonato.rodadaAtual);
            Map(x => x.quantidadeDeRodadas).ToColumn(DbSchemaCampeonato.quantidadeDeRodadas);
            Map(x => x.nomeDaTaca).ToColumn(DbSchemaCampeonato.nomeDaTaca);
            Map(x => x.apelido).ToColumn(DbSchemaCampeonato.apelido);
        }
    }
}