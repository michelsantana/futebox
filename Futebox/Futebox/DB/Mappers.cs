using Dapper.FluentMap.Dommel.Mapping;
using Futebox.Models;

namespace Futebox.DB
{
    public class Mappers
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
                ToTable("videoprocesso");
                Map(x => x.id).ToColumn("id");
                Map(x => x.criacao).ToColumn("criacao");
                Map(x => x.alteracao).ToColumn("alteracao");

                Map(x => x.nome).ToColumn("nome");
                Map(x => x.link).ToColumn("link");
                Map(x => x.tipoLink).ToColumn("tipoLink");
                Map(x => x.roteiro).ToColumn("roteiro");
                Map(x => x.attrTitulo).ToColumn("attrTitulo");
                Map(x => x.attrDescricao).ToColumn("attrDescricao");
                Map(x => x.status).ToColumn("status");
                Map(x => x.processado).ToColumn("processado");
                Map(x => x.json).ToColumn("json");
            }
        }
    }
}
