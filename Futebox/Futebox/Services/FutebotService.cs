using Futebox.Models;
using Futebox.Services.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class FutebotService : IFutebotService
    {
        public RobotResult VerificarConfiguracaoYoutubeBrowser()
        {
            string commandArgs = $"command=youtube";
            return ExecutarCMD(commandArgs, new object[] { });
        }

        public RobotResult GerarVideo(string processoId)
        {
            string commandArgs = $"command=video";
            string idArgs = $"id={processoId}";
            string datasourceArgs = $"datasource={Settings.ApplicationHttpBaseUrl}api/processo/{processoId}";
            return ExecutarCMD(commandArgs, idArgs, datasourceArgs);
        }

        public RobotResult PublicarVideo(string processoId)
        {
            string commandArgs = $"command=publicar";
            string idArgs = $"id={processoId}";
            string datasourceArgs = $"datasource={Settings.ApplicationHttpBaseUrl}api/processo/{processoId}";
            return ExecutarCMD(commandArgs, idArgs, datasourceArgs);
        }

        public RobotResult AbrirPasta(string processoId)
        {
            string commandArgs = $"command=pasta";
            return ExecutarCMD(commandArgs, new object[] { });
        }

        private RobotResult ExecutarCMD(params object[] args)
        {
            string botFolder = $"{Settings.ApplicationsRoot}/Robot";
            string botBatch = @"integration.bat";
            string argsBuild(object[] arg) => string.Join(" ", arg.Select(_ => $"\"{_}\""));

            string strCmdText = $"{botFolder}/{botBatch}";
            ProcessStartInfo processInfo = new ProcessStartInfo(strCmdText, $"{botFolder} {argsBuild(args)}");
            processInfo.UseShellExecute = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = false;

            Process batchProcess = new Process();
            batchProcess.StartInfo = processInfo;
            RobotResult datareceived = new RobotResult();
            batchProcess.OutputDataReceived += (args, b) =>
            {
                var command = b?.Data;
                datareceived.ReadLine(command);
            };
            batchProcess.ErrorDataReceived += (args, b) =>
            {
                var command = b?.Data;
                datareceived.ReadLine(command);
            };
            batchProcess.Start();
            batchProcess.BeginOutputReadLine();
            batchProcess.BeginErrorReadLine();

            batchProcess.WaitForExit();
            
            return datareceived;
        }
    }
}
