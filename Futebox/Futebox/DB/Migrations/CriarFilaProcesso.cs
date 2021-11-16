using FluentMigrator;
using FluentMigrator.Runner.Processors.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Migrations
{
    [Migration(20210930000000)]
    public class CriarEstruturaDeProcessos : Migration
    {
        public override void Down()
        {
            Delete.Table("videoprocesso");
        }

        public override void Up()
        {
            Create.Table("videoprocesso")
            .WithColumn("id").AsString().NotNullable().PrimaryKey()
            .WithColumn("criacao").AsDateTime().NotNullable()
            .WithColumn("alteracao").AsDateTime().Nullable()

            .WithColumn("tipo").AsString().NotNullable() // partida
            .WithColumn("idExterno").AsString().NotNullable() // x
            .WithColumn("nome").AsString().NotNullable() // partida a vs b 

            .WithColumn("link").AsString().Nullable() // http
            .WithColumn("linkThumb").AsString().Nullable() // http
            .WithColumn("tipoLink").AsString().Nullable() // print ou png
            .WithColumn("imgLargura").AsString().Nullable() // tamanho do print 1920 outros 
            .WithColumn("imgAltura").AsString().Nullable() // tamanho do print 1080 outros 

            .WithColumn("roteiro").AsString().Nullable() // blábláblá

            .WithColumn("attrTitulo").AsString().Nullable() // titulo do vdo no yt
            .WithColumn("attrDescricao").AsString().Nullable() // descrição do vdo no yt

            .WithColumn("status").AsInt32().NotNullable() // estado do processo
            .WithColumn("statusMensagem").AsString().Nullable()

            .WithColumn("processado").AsBoolean().NotNullable() // finalizou processo

            .WithColumn("args").AsString().NotNullable() // json com dados da entidade ou informações genéricas 

            .WithColumn("agendado").AsBoolean().NotNullable()
            .WithColumn("notificacao").AsString().Nullable()
            .WithColumn("agendamento").AsDateTime().Nullable()
            .WithColumn("arquivoVideo").AsString().Nullable()
            .WithColumn("portaExecucao").AsString().Nullable();
        }
    }
}
