using Futebox.DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Futebox.Teste
{

    [CollectionDefinition(nameof(DataBaseCollection))]
    public class DataBaseCollection : ICollectionFixture<DatabaseFixture>
    { }

    public class DatabaseFixture : IDisposable
    {
        public string AppRootFolder = "D:/Notebook/Workspace/pessoal/FuteBox/git/futebox/Futebox/Futebox";

        public DatabaseConfig GerarConfiguracaoDoBancoDeDados()
        {
            var db = new DatabaseConfig($"Data Source={AppRootFolder}/DB/futebox.sqlite");
            return db;
        }

        public void DefinirSettings()
        {
            DotEnv.Load(Path.Combine($"{AppRootFolder}", ".env"));
            Settings.BackendRoot = Directory.GetCurrentDirectory();
            Settings.ApplicationsRoot = AppRootFolder;
            Settings.ApplicationHttpBaseUrl = "http://localhost:5001/";
            Settings.RobotEndpointBaseUrl = "http://localhost:5002/";
            Settings.TelegramBotToken = DotEnv.Get("TELEGRAM_BOT_TOKEN");
            Settings.TelegramNotifyUserId = DotEnv.Get("TELEGRAM_NOTIFY_USERID");
            Settings.DEBUGMODE = true;
        }

        public void Dispose() { }
    }
}
