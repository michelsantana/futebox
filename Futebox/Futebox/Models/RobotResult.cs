using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class RobotResult
    {
        public RobotResultCommand command { get; set; }
        public List<string> stack { get; set; }

        public RobotResult(RobotResultCommand forceStatus, Exception ex)
        {
            this.command = forceStatus;
            this.stack = new List<string>();
            this.stack.Add(ex.Message);
        }
        public RobotResult()
        {
            this.command = RobotResultCommand.BLANK;
            this.stack = new List<string>();
        }

        public void ReadLine(string command)
        {
            command = (command ?? "");
            if (command.StartsWith("!"))
            {
                if (command.StartsWith("!OK")) this.command = RobotResultCommand.OK;
                else if (command.StartsWith("!ERROR")) this.command = RobotResultCommand.ERROR;
                else if (command.StartsWith("!AUTHFAILED")) this.command = RobotResultCommand.AUTHFAILED;
                else if (command.StartsWith("!BLANK")) this.command = RobotResultCommand.BLANK;
                else if (command.StartsWith("!INVALID")) this.command = RobotResultCommand.INVALID;
                else this.command = RobotResultCommand.BLANK;
            }
            if (!string.IsNullOrEmpty(command.Trim()))
            {
                this.stack.Add(command);
                EyeLog.Log(command);
            }
        }
    }

    public class RobotResultApi
    {
        public HttpStatusCode status { get; set; }
        public string mensagem { get; set; }
        public string resultado { get; set; }
        public string[] stack { get; set; }
    }
}
