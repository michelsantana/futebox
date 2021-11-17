using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Futebox.Services
{
    public class FutebotService : IFutebotService
    {
        public bool VerificarConfiguracaoYoutubeBrowser()
        {
            string commandArgs = $"command=youtube";
            ExecutarCMD(commandArgs, new object[] { });

            return true;
        }


        private bool ExecutarCMD(params object[] args)
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
            processInfo.CreateNoWindow = true;

            Process batchProcess = new Process();
            batchProcess.StartInfo = processInfo;
            RobotResult datareceived = null;
            batchProcess.OutputDataReceived += (args, b) =>
            {
                var command = b?.Data;
                if (command.StartsWith('!'))
                {
                    datareceived = new RobotResult(command);
                }
            };
            batchProcess.Start();
            batchProcess.BeginOutputReadLine();

            batchProcess.WaitForExit();
            var result = batchProcess.StandardOutput.ReadToEndAsync().Result;
            Console.WriteLine(result);
            return true;
        }

        
    }
}
