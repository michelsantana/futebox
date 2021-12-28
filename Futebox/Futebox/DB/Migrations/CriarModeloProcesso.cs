using FluentMigrator;
using FluentMigrator.Runner.Processors.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Migrations
{
    [Migration(20210930000000)]
    public class CriarModeloProcesso : Migration
    {
        public override void Down()
        {
            Delete.Table("processo");
        }

        public override void Up()
        {
            Create.Table("processo")
            .WithColumn("id").AsString().NotNullable().PrimaryKey()
            .WithColumn("criacao").AsDateTime().NotNullable()
            .WithColumn("alteracao").AsDateTime().Nullable()

            .WithColumn("nome").AsString().NotNullable()
            .WithColumn("categoria").AsString().Nullable()
            .WithColumn("status").AsInt32().NotNullable()
            .WithColumn("statusMensagem").AsString().Nullable()
            .WithColumn("notificacao").AsString().Nullable()
            .WithColumn("agendamento").AsDateTime().Nullable()
            .WithColumn("agendado").AsBoolean().NotNullable()

            .WithColumn("partidaId").AsString().Nullable()
            .WithColumn("campeonatoId").AsString().Nullable()
            .WithColumn("rodadaId").AsString().Nullable()
            .WithColumn("pastaDosArquivos").AsString().Nullable()
            ;
        }
    }
}
