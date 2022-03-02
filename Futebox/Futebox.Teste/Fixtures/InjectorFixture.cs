using Futebox.DB;
using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace Futebox.Teste
{

    [CollectionDefinition(nameof(InjectorCollection))]
    public class InjectorCollection : ICollectionFixture<InjectorFixture>
    { }

    public class InjectorFixture : IDisposable
    {
        public string AppRootFolder = "D:/Notebook/Workspace/pessoal/FuteBox/git/futebox/Futebox/Futebox";
        public static Process server;
        public static IHttpHandler _http = new HttpHandler();
        
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

        public void StartupServer()
        {
            if (server != null) KillServer();
            server = new Process();
            server.StartInfo.FileName = $"{Path.Combine(Settings.ApplicationsRoot, "handle-server-for-tests")}";
            server.StartInfo.WorkingDirectory = Settings.ApplicationsRoot;
            server.StartInfo.UseShellExecute = true;
            server.Start();

            var waitingForServerCount = 0;
            var serverIsOnline = false;
            do
            {
                var result = _http.Get(Settings.ApplicationHttpBaseUrl);
                if (result.StatusCode == System.Net.HttpStatusCode.OK) serverIsOnline = true;
                else
                {
                    waitingForServerCount++;
                    Thread.Sleep(3000);
                }
            } while (waitingForServerCount < 10 && !serverIsOnline);
        }

        public void KillServer()
        {
            try
            {
                if(server != null) server.Kill(true);
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
            }
        }

        public void Dispose() {
            KillServer();
        }
    }
}
