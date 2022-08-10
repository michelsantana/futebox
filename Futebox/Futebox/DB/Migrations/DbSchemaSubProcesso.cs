//using FluentMigrator;

//namespace Futebox.DB.Migrations
//{
//    [Migration(20211201000000)]
//    public class DbSchemaSubProcesso : Migration
//    {
//        public const string subprocesso = nameof(subprocesso);
//        public const string id = nameof(id);
//        public const string criacao = nameof(criacao);
//        public const string alteracao = nameof(alteracao);
//        public const string processoId = nameof(processoId);
//        public const string redeSocial = nameof(redeSocial);
//        public const string linkDaImagemDoVideo = nameof(linkDaImagemDoVideo);
//        public const string larguraVideo = nameof(larguraVideo);
//        public const string alturaVideo = nameof(alturaVideo);
//        public const string nomeDoArquivoAudio = nameof(nomeDoArquivoAudio);
//        public const string nomeDoArquivoImagem = nameof(nomeDoArquivoImagem);
//        public const string nomeDoArquivoVideo = nameof(nomeDoArquivoVideo);
//        public const string pastaDoArquivo = nameof(pastaDoArquivo);
//        public const string linkPostagem = nameof(linkPostagem);
//        public const string roteiro = nameof(roteiro);
//        public const string status = nameof(status);
//        public const string categoriaVideo = nameof(categoriaVideo);
//        public const string tituloVideo = nameof(tituloVideo);
//        public const string descricaoVideo = nameof(descricaoVideo);
//        public const string playlist = nameof(playlist);
//        public const string legendaPostagem = nameof(legendaPostagem);
//        public const string args = nameof(args);

//        public override void Down()
//        {
//            Delete.Table(subprocesso);
//        }

//        public override void Up()
//        {
//            Create.Table(subprocesso)
//            .WithColumn(id).AsString().NotNullable().PrimaryKey()
//            .WithColumn(criacao).AsDateTime().NotNullable()
//            .WithColumn(alteracao).AsDateTime().Nullable()

//            .WithColumn(processoId).AsString().NotNullable()
//            .WithColumn(redeSocial).AsString().NotNullable()
//            .WithColumn(linkDaImagemDoVideo).AsString().Nullable()

//            .WithColumn(larguraVideo).AsInt32().NotNullable()
//            .WithColumn(alturaVideo).AsInt32().NotNullable()

//            .WithColumn(nomeDoArquivoAudio).AsString().Nullable()
//            .WithColumn(nomeDoArquivoImagem).AsString().Nullable()
//            .WithColumn(nomeDoArquivoVideo).AsString().Nullable()
//            .WithColumn(pastaDoArquivo).AsString().Nullable()
//            .WithColumn(linkPostagem).AsString().Nullable()
//            .WithColumn(roteiro).AsDateTime().Nullable()
//            .WithColumn(status).AsInt32().NotNullable()

//            .WithColumn(categoriaVideo).AsString().Nullable()
//            .WithColumn(tituloVideo).AsString().Nullable()
//            .WithColumn(descricaoVideo).AsString().Nullable()
//            .WithColumn(playlist).AsString().Nullable()
//            .WithColumn(legendaPostagem).AsString().Nullable()
//            .WithColumn(args).AsString().Nullable()
//            ;
//        }
//    }
//}
