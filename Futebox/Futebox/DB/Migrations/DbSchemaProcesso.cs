using FluentMigrator;

namespace Futebox.DB.Migrations
{
    [Migration(20210930000000)]
    public class DbSchemaProcesso : Migration
    {
        public const string processo = nameof(processo);
        public const string id = nameof(id);
        public const string criacao = nameof(criacao);
        public const string alteracao = nameof(alteracao);
        public const string nome = nameof(nome);
        public const string categoria = nameof(categoria);
        public const string status = nameof(status);
        public const string statusMensagem = nameof(statusMensagem);
        public const string notificacao = nameof(notificacao);
        public const string agendamento = nameof(agendamento);
        public const string agendado = nameof(agendado);
        public const string partidaId = nameof(partidaId);
        public const string campeonatoId = nameof(campeonatoId);
        public const string rodadaId = nameof(rodadaId);
        public const string pastaDosArquivos = nameof(pastaDosArquivos);

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
            .WithColumn(categoria).AsString().Nullable()
            .WithColumn(status).AsInt32().NotNullable()
            .WithColumn(statusMensagem).AsString().Nullable()
            .WithColumn(notificacao).AsString().Nullable()
            .WithColumn(agendamento).AsDateTime().Nullable()
            .WithColumn(agendado).AsBoolean().NotNullable()

            .WithColumn(partidaId).AsString().Nullable()
            .WithColumn(campeonatoId).AsString().Nullable()
            .WithColumn(rodadaId).AsString().Nullable()
            .WithColumn(pastaDosArquivos).AsString().Nullable()
            ;
        }
    }
}
