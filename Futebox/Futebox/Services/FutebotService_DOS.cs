using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Futebox.Services
{
    [Obsolete("Utilizar copmunicação via api")]
    public class FutebotService_DOS //: IFutebotService
    {
        class CmdArgs
        {
            public Dictionary<string, string> args = new Dictionary<string, string>();
            public void Add(string k, string v)
            {
                this.args.Add(k, v);
            }
            public string ToCommandLine()
            {
                string line = "";
                this.args.Keys.ToList().ForEach(_ =>
                {
                    line += $"\"{_}@{this.args[_]}\" ";
                });
                return line;
            }
            public static CmdArgs New()
            {
                return new CmdArgs();
            }
        }
        public RobotResult VerificarConfiguracaoYoutubeBrowser()
        {
            var args = CmdArgs.New();
            {
                args.Add("command", "ytstatus");
            };
            return ExecutarCMD(args);
        }

        public RobotResult VerificarConfiguracaoInstagramBrowser()
        {
            var args = CmdArgs.New();

            args.Add("command", "igstatus");

            return ExecutarCMD(args);
        }

        public RobotResult GerarImagem(string processoApi, string pasta, string nomeDoArquivo, string link, int largura, int altura)
        {
            var args = CmdArgs.New();

            args.Add("command", "imagem");
            args.Add("api", processoApi);

            args.Add("link", Uri.EscapeDataString(link));
            args.Add("pasta", pasta);
            args.Add("nomeDoArquivo", nomeDoArquivo);
            args.Add("largura", largura.ToString());
            args.Add("altura", altura.ToString());

            return ExecutarCMD(args);
        }
        public RobotResult GerarAudio(string processoApi, string pasta, string nomeDoArquivo)
        {
            var args = CmdArgs.New();

            args.Add("command", "audio");
            args.Add("api", processoApi);

            args.Add("pasta", pasta);
            args.Add("nomeDoArquivo", nomeDoArquivo);

            return ExecutarCMD(args);
        }
        public RobotResult GerarVideo(string processoApi, string pasta, string nomeDoArquivoImagem, string nomeDoArquivoAudio, string nomeDoArquivoVideo)
        {
            var args = CmdArgs.New();

            args.Add("command", "video");
            args.Add("api", processoApi);

            args.Add("pasta", pasta);
            args.Add("nomeDoArquivoImagem", nomeDoArquivoImagem);
            args.Add("nomeDoArquivoAudio", nomeDoArquivoAudio);
            args.Add("nomeDoArquivoVideo", nomeDoArquivoVideo);

            return ExecutarCMD(args);
        }
        public RobotResult PublicarVideo(string processoApi, string pasta, string nomeDoArquivoVideo, RedeSocialFinalidade redeSocial)
        {
            var args = CmdArgs.New();

            args.Add("command", "video");
            args.Add("api", processoApi);

            args.Add("pasta", pasta);
            args.Add("nomeDoArquivoVideo", nomeDoArquivoVideo);
            args.Add("redeSocial", $"{redeSocial}");

            return ExecutarCMD(args);
        }
        public RobotResult AbrirPasta(string pasta)
        {
            var args = CmdArgs.New();

            args.Add("command", "pasta");
            args.Add("pasta", pasta);

            return ExecutarCMD(args);
        }

        private RobotResult ExecutarCMD(CmdArgs args)
        {
            try
            {
                string botFolder = $"{Settings.ApplicationsRoot}Robot";

                var process = new Process();
                //process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                RobotResult datareceived = new RobotResult();
                process.OutputDataReceived += (args, b) =>
                {
                    var command = b?.Data;
                    datareceived.ReadLine(command);
                };
                process.ErrorDataReceived += (args, b) =>
                {
                    var command = b?.Data;
                    datareceived.ReadLine(command);
                };

                if (Settings.DEBUGMODE)
                {
                    EyeLog.Log("Debug mode, command line to debug robot");
                    EyeLog.Log($"cd /d \"{ botFolder }\"");
                    EyeLog.Log($"npm run rc { args.ToCommandLine() }");
                }

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.StandardInput.WriteLine($"cd /d \"{ botFolder }\"");
                process.StandardInput.Flush();

                process.StandardInput.WriteLine($"npm run rc { args.ToCommandLine() }");
                process.StandardInput.Flush();

                process.WaitForExit();

                return datareceived;
            }
            catch (Exception ex)
            {
                return new RobotResult(RobotResultCommand.ERROR, ex);
            }
        }
    }
}
