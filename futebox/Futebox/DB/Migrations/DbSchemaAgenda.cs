using FluentMigrator;

namespace Futebox.DB.Migrations
{
    [Migration(20220801000001)]
    public class DbSchemaAgenda : Migration
    {
        public const string agenda = nameof(agenda);
        public const string id = nameof(id);
        public const string processoId = nameof(processoId);
        public const string criacao = nameof(criacao);
        public const string alteracao = nameof(alteracao);
        public const string descricao = nameof(descricao);
        public const string agendamento = nameof(agendamento);
        public const string status = nameof(status);

        public override void Down()
        {
            Delete.Table(agenda);
        }

        public override void Up()
        {
            Create.Table(agenda)
            .WithColumn(id).AsString().NotNullable().PrimaryKey()
            .WithColumn(processoId).AsString().NotNullable()
            .WithColumn(criacao).AsDateTime().NotNullable()
            .WithColumn(alteracao).AsDateTime().Nullable()
            .WithColumn(descricao).AsString().Nullable()
            .WithColumn(agendamento).AsDateTime().Nullable()
            .WithColumn(status).AsString().Nullable()
            ;
        }
    }
}
