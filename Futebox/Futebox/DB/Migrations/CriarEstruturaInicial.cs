using FluentMigrator;
using FluentMigrator.Runner.Processors.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Migrations
{
    [Migration(20210926000000)]
    public class CriarEstruturaInicial : Migration
    {
        public override void Down()
        {
            Delete.Table("time");
        }

        public override void Up()
        {
            Create.Table("time")
            .WithColumn("id").AsString().NotNullable().PrimaryKey()
            .WithColumn("criacao").AsDateTime().NotNullable()
            .WithColumn("alteracao").AsDateTime().Nullable()
            .WithColumn("nome").AsString().NotNullable()
            .WithColumn("sigla").AsString().Nullable()
            .WithColumn("urlLogo").AsString().Nullable()
            .WithColumn("origemDado").AsString().NotNullable()
            .WithColumn("origem_ext_id").AsString().NotNullable()
            .WithColumn("origem_ext_equipe_id").AsString().NotNullable()
            .WithColumn("nomeAdaptadoWatson").AsString().Nullable()
            .WithColumn("logoBin").AsString().Nullable()

            .WithColumn("selecao").AsBoolean().Nullable()
            .WithColumn("torcedorNoSingular").AsString().Nullable()
            .WithColumn("torcedorNoPlural").AsString().Nullable()
            .WithColumn("timeFantasia").AsBoolean().Nullable()
            .WithColumn("cidade").AsString().Nullable()
            .WithColumn("estado").AsString().Nullable()
            .WithColumn("pais").AsString().Nullable()
            .WithColumn("grupo").AsString().Nullable()
            .WithColumn("isTimeGrande").AsBoolean().Nullable()
            .WithColumn("hasScout").AsBoolean().Nullable()
            .WithColumn("idTecnico").AsInt32().Nullable()
            .WithColumn("tecnico").AsString().Nullable();
        }
    }
}
