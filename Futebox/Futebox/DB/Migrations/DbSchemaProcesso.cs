using FluentMigrator;

namespace Futebox.DB.Migrations
{
    [Migration(20210930000001)]
    public class DbSchemaProcesso : Migration
    {
        public const string processo = nameof(processo);
        public const string id = nameof(id);
        public const string criacao = nameof(criacao);
        public const string alteracao = nameof(alteracao);
        public const string nome = nameof(nome);
        public const string categoria = nameof(categoria);
        public const string status = nameof(status);
        public const string log = nameof(log);
        public const string agendamento = nameof(agendamento);
        public const string agendado = nameof(agendado);
        public const string pasta = nameof(pasta);
        public const string args = nameof(args);

        public const string linkDaImagemDoVideo = nameof(linkDaImagemDoVideo);
        public const string larguraVideo = nameof(larguraVideo);
        public const string alturaVideo = nameof(alturaVideo);
        public const string nomeDoArquivoAudio = nameof(nomeDoArquivoAudio);
        public const string nomeDoArquivoImagem = nameof(nomeDoArquivoImagem);
        public const string nomeDoArquivoVideo = nameof(nomeDoArquivoVideo);

        public const string roteiro = nameof(roteiro);
        public const string tituloVideo = nameof(tituloVideo);
        public const string descricaoVideo = nameof(descricaoVideo);
        public const string social = nameof(social);
        public const string jobKey = nameof(jobKey);

        public override void Down()
        {
            Delete.Table(processo);
        }

        public override void Up()
        {
            Create.Table(processo)
            .WithColumn(id).AsString().NotNullable().PrimaryKey()
            .WithColumn(criacao).AsDateTime().NotNullable()
            .WithColumn(alteracao).AsDateTime().Nullable()

            .WithColumn(nome).AsString().NotNullable()
            .WithColumn(categoria).AsInt32().Nullable()
            .WithColumn(status).AsInt32().NotNullable()
            .WithColumn(log).AsString().Nullable()
            .WithColumn(agendamento).AsDateTime().Nullable()
            .WithColumn(agendado).AsBoolean().NotNullable()
            .WithColumn(pasta).AsString().Nullable()
            .WithColumn(args).AsString().Nullable()

            .WithColumn(linkDaImagemDoVideo).AsString().Nullable()
            .WithColumn(larguraVideo).AsInt32().Nullable()
            .WithColumn(alturaVideo).AsInt32().Nullable()
            .WithColumn(nomeDoArquivoAudio).AsString().Nullable()
            .WithColumn(nomeDoArquivoImagem).AsString().Nullable()
            .WithColumn(nomeDoArquivoVideo).AsString().Nullable()
            .WithColumn(roteiro).AsString().Nullable()
            .WithColumn(tituloVideo).AsString().Nullable()
            .WithColumn(descricaoVideo).AsString().Nullable()
            .WithColumn(social).AsInt32().Nullable()
            .WithColumn(jobKey).AsString().Nullable()
            ;
        }
    }
}
