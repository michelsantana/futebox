using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Migrations
{
    [Migration(20220130000000)]
    public class DbSchemaCampeonato : Migration
    {
        public const string campeonato = nameof(campeonato);
        public const string id = nameof(id);
        public const string criacao = nameof(criacao);
        public const string alteracao = nameof(alteracao);
        public const string origem_ext_id = nameof(origem_ext_id);
        public const string ativo = nameof(ativo);
        public const string pais = nameof(pais);
        public const string nome = nameof(nome);
        public const string urlLogo = nameof(urlLogo);
        public const string sdeSlug = nameof(sdeSlug);
        public const string temporada = nameof(temporada);
        public const string categoria = nameof(categoria);
        public const string temClassificacao = nameof(temClassificacao);
        public const string temClassificacaoPorGrupo = nameof(temClassificacaoPorGrupo);
        public const string tipoDeColeta = nameof(tipoDeColeta);
        public const string faseAtual = nameof(faseAtual);
        public const string quantidadeDeEquipes = nameof(quantidadeDeEquipes);
        public const string rodadaAtual = nameof(rodadaAtual);
        public const string quantidadeDeRodadas = nameof(quantidadeDeRodadas);
        public const string nomeDaTaca = nameof(nomeDaTaca);
        public const string apelido = nameof(apelido);

        public override void Down()
        {
            Delete.Table(campeonato);
        }

        public override void Up()
        {
            Create.Table(campeonato)
            .WithColumn(id).AsString().NotNullable().PrimaryKey()
            .WithColumn(criacao).AsDateTime().NotNullable()
            .WithColumn(alteracao).AsDateTime().Nullable()
            .WithColumn(origem_ext_id).AsString().Nullable()
            .WithColumn(ativo).AsBoolean().Nullable()
            .WithColumn(pais).AsString().Nullable()
            .WithColumn(nome).AsString().Nullable()
            .WithColumn(urlLogo).AsString().Nullable()
            .WithColumn(sdeSlug).AsString().Nullable()
            .WithColumn(temporada).AsString().Nullable()
            .WithColumn(categoria).AsString().Nullable()
            .WithColumn(temClassificacao).AsBoolean().Nullable()
            .WithColumn(temClassificacaoPorGrupo).AsBoolean().Nullable()
            .WithColumn(tipoDeColeta).AsString().Nullable()
            .WithColumn(faseAtual).AsString().Nullable()
            .WithColumn(quantidadeDeEquipes).AsString().Nullable()
            .WithColumn(rodadaAtual).AsInt32().Nullable()
            .WithColumn(quantidadeDeRodadas).AsInt32().Nullable()
            .WithColumn(nomeDaTaca).AsString().Nullable()
            .WithColumn(apelido).AsString().Nullable()
            ;
        }
    }
}
