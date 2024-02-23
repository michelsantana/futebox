using FluentMigrator;
using FluentMigrator.Runner.Processors.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Migrations
{
    [Migration(20210926000000)]
    public class DbSchemaTime : Migration
    {
        public const string time = nameof(time);

        public const string id = nameof(id);
        public const string criacao = nameof(criacao);
        public const string alteracao = nameof(alteracao);
        public const string nome = nameof(nome);
        public const string sigla = nameof(sigla);
        public const string urlLogo = nameof(urlLogo);
        public const string origemDado = nameof(origemDado);
        public const string origem_ext_id = nameof(origem_ext_id);
        public const string origem_ext_equipe_id = nameof(origem_ext_equipe_id);
        public const string nomeAdaptadoWatson = nameof(nomeAdaptadoWatson);
        public const string logoBin = nameof(logoBin);
        public const string selecao = nameof(selecao);
        public const string torcedorNoSingular = nameof(torcedorNoSingular);
        public const string torcedorNoPlural = nameof(torcedorNoPlural);
        public const string timeFantasia = nameof(timeFantasia);
        public const string cidade = nameof(cidade);
        public const string estado = nameof(estado);
        public const string pais = nameof(pais);
        public const string grupo = nameof(grupo);
        public const string isTimeGrande = nameof(isTimeGrande);
        public const string hasScout = nameof(hasScout);
        public const string idTecnico = nameof(idTecnico);
        public const string tecnico = nameof(tecnico);
        public const string logoEspecifico = nameof(logoEspecifico);

        public override void Down()
        {
            Delete.Table(time);
        }

        public override void Up()
        {
            Create.Table(time)
            .WithColumn(id).AsString().NotNullable().PrimaryKey()
            .WithColumn(criacao).AsDateTime().NotNullable()
            .WithColumn(alteracao).AsDateTime().Nullable()
            .WithColumn(nome).AsString().NotNullable()
            .WithColumn(sigla).AsString().Nullable()
            .WithColumn(urlLogo).AsString().Nullable()
            .WithColumn(origemDado).AsString().NotNullable()
            .WithColumn(origem_ext_id).AsString().NotNullable()
            .WithColumn(origem_ext_equipe_id).AsString().NotNullable()
            .WithColumn(nomeAdaptadoWatson).AsString().Nullable()
            .WithColumn(logoBin).AsString().Nullable()

            .WithColumn(selecao).AsBoolean().Nullable()
            .WithColumn(torcedorNoSingular).AsString().Nullable()
            .WithColumn(torcedorNoPlural).AsString().Nullable()
            .WithColumn(timeFantasia).AsBoolean().Nullable()
            .WithColumn(cidade).AsString().Nullable()
            .WithColumn(estado).AsString().Nullable()
            .WithColumn(pais).AsString().Nullable()
            .WithColumn(grupo).AsString().Nullable()
            .WithColumn(isTimeGrande).AsBoolean().Nullable()
            .WithColumn(hasScout).AsBoolean().Nullable()
            .WithColumn(idTecnico).AsInt32().Nullable()
            .WithColumn(tecnico).AsString().Nullable()
            .WithColumn(logoEspecifico).AsString().Nullable()
            ;
        }
    }
}
