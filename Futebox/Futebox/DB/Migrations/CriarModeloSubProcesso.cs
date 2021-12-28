using FluentMigrator;
using FluentMigrator.Runner.Processors.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.DB.Migrations
{
    [Migration(20211201000000)]
    public class CriarModeloSubProcesso : Migration
    {
        public override void Down()
        {
            Delete.Table("subprocesso");
        }

        public override void Up()
        {
            Create.Table("subprocesso")
            .WithColumn("id").AsString().NotNullable().PrimaryKey()
            .WithColumn("criacao").AsDateTime().NotNullable()
            .WithColumn("alteracao").AsDateTime().Nullable()

            .WithColumn("processoId").AsString().NotNullable()
            .WithColumn("redeSocial").AsString().NotNullable()
            .WithColumn("linkDaImagemDoVideo").AsString().Nullable()

            .WithColumn("larguraVideo").AsInt32().NotNullable()
            .WithColumn("alturaVideo").AsInt32().NotNullable()

            .WithColumn("nomeDoArquivoAudio").AsString().Nullable()
            .WithColumn("nomeDoArquivoImagem").AsString().Nullable()
            .WithColumn("nomeDoArquivoVideo").AsString().Nullable()
            .WithColumn("pastaDoArquivo").AsString().Nullable()
            .WithColumn("linkPostagem").AsString().Nullable()
            .WithColumn("roteiro").AsDateTime().Nullable()
            .WithColumn("status").AsInt32().NotNullable()

            .WithColumn("categoriaVideo").AsString().Nullable()
            .WithColumn("tituloVideo").AsString().Nullable()
            .WithColumn("tituloVideo").AsString().Nullable()
            .WithColumn("descricaoVideo").AsString().Nullable()
            .WithColumn("playlist").AsString().Nullable()
            .WithColumn("legendaPostagem").AsString().Nullable() 
            ;
        }
    }
}
